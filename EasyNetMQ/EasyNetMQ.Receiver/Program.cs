using EasyNetMQ.TextMessage;
using EasyNetQ;
using EasyNetQ.SystemMessages;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;

namespace EasyNetMQ.Receiver {
    /// <summary>
    /// EasyNetMQ接收端
    /// </summary>
    class Program {
        /// <summary>
        /// 入口
        /// </summary>
        /// <param name="args">参数</param>
        static void Main(string[] args) {
            var bus = RabbitHutch.CreateBus("host=localhost;username=dxq;password=dxq");

            //接收简单消息
            bus.Subscribe<MyTextMessage>("EasyNetMQ.Receiver.MyTextMessage", x =>
            {
                Console.WriteLine($"接收了MyTextMessage消息:{x.Text}");
            });

            //接收Topic消息
            bus.Subscribe<string>("EasyNetMQ.Receiver.TopicMyTextMessage", x =>
            {
                var message = JsonConvert.DeserializeObject<MyTextMessage>(x);
                Console.WriteLine($"接收了Topic消息:{message.Text}");
            }, o =>
            {
                o.WithTopic(typeof(MyTextMessage).FullName);
            });

            //接收高级消息
            var queue = bus.Advanced.QueueDeclare("EasyNetMQ.Receiver.AdvanceMyTextMessage");
            var exchange = bus.Advanced.ExchangeDeclare("EasyNetMQ.Exchange", ExchangeType.Topic);
            bus.Advanced.Bind(exchange, queue, routingKey: typeof(MyTextMessage).Name);
            bus.Advanced.Consume<string>(queue, (message, info) =>
            {
                throw new Exception("处理消息时发送了错误");
                //Console.WriteLine($"接收了高级消息:{message.Body}");

            });

            var errorqueue = new EasyNetQ.Topology.Queue("EasyNetQ_Default_Error_Queue", isExclusive: false);
            bus.Advanced.Consume<Error>(errorqueue, (message, info) =>
            {
                Console.WriteLine($"接收了处理时发生异常的消息:{message.Body.Message}");
            });

            Console.WriteLine("按任意键退出");
            Console.ReadKey();
            bus.Dispose();
        }
    }
}
