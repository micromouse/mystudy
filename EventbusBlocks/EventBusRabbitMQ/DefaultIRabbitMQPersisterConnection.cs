using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace DXQ.Study.EventbusBlocks.EventBusRabbitMQ {
    /// <summary>
    /// 缺省RabbitMQ连接管理接口实现
    /// </summary>
    public class DefaultIRabbitMQPersisterConnection : IRabbitMQPersisterConnection, IDisposable {
        private readonly IConnectionFactory connectionFactory = null;
        private readonly ILogger<DefaultIRabbitMQPersisterConnection> logger = null;
        private readonly int retryCount = 1;
        private IConnection connection = null;
        private bool disposed = false;
        private readonly object synclock = new object();

        /// <summary>
        /// 初始化缺省RabbitMQ连接管理接口实现
        /// </summary>
        /// <param name="connectionFactory">RabbitMQ连接工厂</param>
        /// <param name="logger">日志</param>
        /// <param name="retryCount">连接重试次数</param>
        public DefaultIRabbitMQPersisterConnection(IConnectionFactory connectionFactory, ILogger<DefaultIRabbitMQPersisterConnection> logger, int retryCount = 5) {
            this.connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.retryCount = retryCount;
        }

        /// <summary>
        /// 是否已连接到RabbitMQ
        /// </summary>
        public bool IsConnected => connection != null && connection.IsOpen && !disposed;

        /// <summary>
        /// 建立通道
        /// </summary>
        /// <returns>到RabbitMQ的通道</returns>
        public IModel CreateModel() {
            if (!this.IsConnected) throw new InvalidOperationException("RabbitMQ连接无效");
            return connection.CreateModel();
        }

        /// <summary>
        /// 连接到RabbitMQ
        /// </summary>
        /// <returns>是否连接成功</returns>
        public bool TryConnect() {
            logger.LogInformation("RabbitMQ Client正在尝试连接");

            lock (synclock) {
                var policy = RetryPolicy.Handle<SocketException>()
                    .Or<BrokerUnreachableException>()
                    .WaitAndRetry(retryCount, (retryAttempt) => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) => logger.LogWarning(ex.ToString()));

                //建立连接,添加事件侦听
                policy.Execute(() => connection = connectionFactory.CreateConnection());
                if (this.IsConnected) {
                    connection.ConnectionShutdown += new EventHandler<ShutdownEventArgs>(this.Connection_ConnectionShutdown);
                    connection.CallbackException += new EventHandler<CallbackExceptionEventArgs>(this.Connection_CallbackException);
                    connection.ConnectionBlocked += new EventHandler<ConnectionBlockedEventArgs>(this.Connection_ConnectionBlocked);

                    logger.LogInformation($"RabbitMQ连接获得了一个连接[{connection.Endpoint.HostName}],并且订阅了故障事件");
                    return true;
                } else {
                    logger.LogCritical("FATAL ERROR:RabbitMQ连接不能被建立并打开");
                    return false;
                }
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose() {
            if (disposed) return;

            disposed = true;
            try {
                connection.Dispose();
            } catch (Exception ex) {
                logger.LogCritical(ex.ToString());
            }
        }

        /// <summary>
        /// 连接关闭
        /// </summary>
        private void Connection_ConnectionShutdown(object sender, ShutdownEventArgs e) {
            if (disposed) return;
            logger.LogWarning("RabbitMQ连接已关闭，正在尝试重新连接");
            this.TryConnect();
        }

        /// <summary>
        /// 回掉异常
        /// </summary>
        private void Connection_CallbackException(object sender, CallbackExceptionEventArgs e) {
            if (disposed) return;
            logger.LogWarning("RabbitMQ连接抛出异常，正在尝试重新连接");
            this.TryConnect();
        }

        /// <summary>
        /// 连接被阻止
        /// </summary>
        private void Connection_ConnectionBlocked(object sender, ConnectionBlockedEventArgs e) {
            if (disposed) return;
            logger.LogWarning("RabbitMQ连接已被阻止，正在尝试重新连接");
            this.TryConnect();
        }


    }
}
