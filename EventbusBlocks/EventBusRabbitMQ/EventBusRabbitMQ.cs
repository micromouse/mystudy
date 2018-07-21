using Autofac;
using DXQ.Study.EventbusBlocks.EventBus.Abstractions;
using DXQ.Study.EventbusBlocks.EventBus.Events;
using DXQ.Study.EventbusBlocks.EventBus.Manager;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DXQ.Study.EventbusBlocks.EventBusRabbitMQ {
    /// <summary>
    /// 事件总线RabbitMQ实现
    /// </summary>
    public class EventBusRabbitMQ : IEventBus {
        private const string BROKER_NAME= "dxqstudy_event_bus";
        private readonly IRabbitMQPersisterConnection persisterConnection = null;
        private readonly ILogger<EventBusRabbitMQ> logger = null;
        private readonly IEventBusSubscriptionsManager subscriptionsManager = null;
        private readonly ILifetimeScope lifetimeScope = null;
        private readonly int retryCount = 1;
        private IModel consumerChannel = null;
        private string queueName = "";
        
        /// <summary>
        /// 初始化事件总线RabbitMQ实现
        /// </summary>
        /// <param name="persisterConnection">RabbitMQ连接管理</param>
        /// <param name="logger">日志</param>
        /// <param name="lifetimeScope">Autofac范围</param>
        /// <param name="subscriptionsManager">事件总线订阅管理器</param>
        /// <param name="queueName">队列名称</param>
        /// <param name="retryCount">重试次数</param>
        public EventBusRabbitMQ(IRabbitMQPersisterConnection persisterConnection, ILogger<EventBusRabbitMQ> logger,
            ILifetimeScope lifetimeScope, IEventBusSubscriptionsManager subscriptionsManager, string queueName = null, int retryCount = 5) {
            this.persisterConnection = persisterConnection;
            this.logger = logger;
            this.subscriptionsManager = subscriptionsManager;
            this.lifetimeScope = lifetimeScope;
            this.queueName = queueName;
            this.retryCount = retryCount;
            this.consumerChannel = this.CreateConsumerChannel();
            this.subscriptionsManager.OnEventRemoved += new EventHandler<string>(this.SubscriptionsManager_OnEventRemoved);
        }

        /// <summary>
        /// 处理事件已删除事件
        /// </summary>
        private void SubscriptionsManager_OnEventRemoved(object sender, string eventName) {
            //建立连接
            if (!persisterConnection.IsConnected) persisterConnection.TryConnect();

            using (var channel = persisterConnection.CreateModel()) {
                channel.QueueUnbind(queue: queueName, exchange: BROKER_NAME, routingKey: eventName);

                //没有事件订阅者
                if (subscriptionsManager.IsEmpty) {
                    queueName = "";
                    consumerChannel.Close();
                }
            }
        }

        /// <summary>
        /// 发布集成事件
        /// </summary>
        /// <param name="event">集成事件</param>
        public void Publish(IntegrationEvent @event) {
            //建立连接
            if (!persisterConnection.IsConnected) persisterConnection.TryConnect();

            var policy = RetryPolicy.Handle<BrokerUnreachableException>()
                .Or<SocketException>()
                .WaitAndRetry(retryCount, (retryAttempt) => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) => logger.LogWarning(ex.ToString()));

            //开启一个通道以发送消息
            using (var channel = persisterConnection.CreateModel()) {
                var eventName = @event.GetType().Name;

                //建立交换机,所有发送到Direct Exchange的消息被转发到具有指定RouteKey的Queue
                channel.ExchangeDeclare(exchange: BROKER_NAME, type: ExchangeType.Direct);

                //消息是以二进制数组的形式传输的，所以如果消息是实体对象的话，需要序列化和然后转化为二进制数组。
                var message = JsonConvert.SerializeObject(@event);
                var body = Encoding.UTF8.GetBytes(message);

                //发送消息
                policy.Execute(() =>
                {
                    //构建一个完全空白的内容头,用于基本内容类,消息是持久的,存在并不会受服务器重启影响
                    var properties = channel.CreateBasicProperties();
                    properties.DeliveryMode = 2;

                    //发布消息
                    //当mandatory标志位设置为true时，如果exchange根据自身类型和消息routingKey无法找到一个合适的queue存储消息，那么broker会调用basic.return方法将消息返还给生产者;
                    //当mandatory设置为false时，出现上述情况broker会直接将消息丢弃;
                    //通俗的讲，mandatory标志告诉broker代理服务器至少将消息route到一个队列中，否则就将消息return给发送者;
                    channel.BasicPublish(
                        exchange: BROKER_NAME,
                        routingKey: eventName,
                        mandatory: true,
                        basicProperties: properties,
                        body: body
                    );
                });
            }
        }

        /// <summary>
        /// 订阅集成事件
        /// </summary>
        /// <typeparam name="T">集成事件</typeparam>
        /// <typeparam name="TH">处理集成事件的集成事件处理器</typeparam>
        public void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T> {
            var eventName = subscriptionsManager.GetEventKey<T>();

            this.ExecuteInternalSubscription(eventName);
            subscriptionsManager.AddSubscription<T, TH>();
        }

        /// <summary>
        /// 订阅匿名集成事件
        /// </summary>
        /// <typeparam name="TH">匿名集成事件处理器</typeparam>
        /// <param name="eventName">事件名称</param>
        public void SubscrbieDynamic<TH>(string eventName) where TH : IDynamicIntegrationEventHandler {
            this.ExecuteInternalSubscription(eventName);
            subscriptionsManager.AddDynamicSubscription<TH>(eventName);
        }

        /// <summary>
        /// 执行内部订阅
        /// </summary>
        /// <param name="eventName">事件名称</param>
        private void ExecuteInternalSubscription(string eventName) {
            var containsKey = subscriptionsManager.HasSubscriptionForEvent(eventName);

            //事件没有被订阅过
            if (!containsKey) {
                if (!persisterConnection.IsConnected) persisterConnection.TryConnect();

                //绑定队列和交换机
                using (var channel = persisterConnection.CreateModel()) {
                    channel.QueueBind(
                        queue: queueName,               //队列名称
                        exchange: BROKER_NAME,          //交换机名称
                        routingKey: eventName           //路由Key(使用事件名称作为路由key,发布事件后只有相同接收路由key才会接收到消息)
                    );
                }
            }
        }

        /// <summary>
        /// 解除订阅集成事件
        /// </summary>
        /// <typeparam name="T">集成事件</typeparam>
        /// <typeparam name="TH">处理集成事件的集成事件处理器</typeparam>
        public void Unsubscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T> {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 解除订阅匿名集成事件
        /// </summary>
        /// <typeparam name="TH">匿名集成事件处理器</typeparam>
        /// <param name="eventData">事件数据</param>
        public void UnsubscribeDynamic<TH>(dynamic eventData) where TH : IDynamicIntegrationEventHandler {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 建立消息接收消费者通道
        /// </summary>
        /// <returns>消息接收消费者通道</returns>
        private IModel CreateConsumerChannel() {
            //建立连接
            if (!persisterConnection.IsConnected) persisterConnection.TryConnect();

            //建立通道,交换器,队列
            var channel = persisterConnection.CreateModel();
            channel.ExchangeDeclare(exchange: BROKER_NAME, type: ExchangeType.Direct);
            channel.QueueDeclare(
                queue: queueName,           //队列名称
                durable: true,              //持久化,RabbitMQ重启数据不会丢失
                exclusive: false,           //不排外,允许多个消费者访问
                autoDelete: false,          //不自动删除
                arguments: null);

            //建立基本事件消费者,添加消息接收处理,设置消息处理方式
            //autoAck:true,不需要回复，接收到消息后，queue上的消息就会清除
            //autoAck:false,需要回复，接收到消息后，queue上的消息不会被清除，直到调用channel.basicAck(deliveryTag, false); queue上的消息才会被清除
            var consumer = new EventingBasicConsumer(channel);
            channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
            consumer.Received += async (sender, e) => {
                var eventName = e.RoutingKey;
                var message = Encoding.UTF8.GetString(e.Body);

                //处理事件
                await this.ProcessEvent(eventName, message);

                //从队列中移除消息
                channel.BasicAck(e.DeliveryTag, multiple: false);
            };

            //发送异常，重建通道
            channel.CallbackException += (sender, e) =>
            {
                channel.Dispose();
                consumerChannel = this.CreateConsumerChannel();
            };

            return channel;
        }

        /// <summary>
        /// 处理事件
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <param name="message">消息</param>
        /// <returns></returns>
        private async Task ProcessEvent(string eventName, string message) {
            //事件有订阅者
            if (subscriptionsManager.HasSubscriptionForEvent(eventName)) {
                using (var scope = lifetimeScope.BeginLifetimeScope("dxqstudy_event_bus")) {
                    //获得事件所有订阅者，然后处理之
                    var subscriptions = subscriptionsManager.GetHandlersForEvent(eventName);
                    foreach (var subscription in subscriptions) {
                        //按是否匿名事件不同处理
                        if (subscription.IsDynamic) {
                            var handler = (IDynamicIntegrationEventHandler)scope.ResolveOptional(subscription.HandlerType);
                            dynamic eventData = JObject.Parse(message);
                            await handler.Handle(eventData);
                        } else {
                            var eventType = subscriptionsManager.GetEventTypeByName(eventName);
                            var integrationEvent = JsonConvert.DeserializeObject(message, eventType);
                            var handler = scope.ResolveOptional(subscription.HandlerType);
                            var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);

                            //处理事件
                            await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent });
                        }
                    }
                }
            }
        }
    }
}
