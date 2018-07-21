using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace DXQ.Study.EventbusBlocks.EventBusRabbitMQ {
    /// <summary>
    /// RabbitMQ连接管理接口
    /// </summary>
    public interface IRabbitMQPersisterConnection {
        /// <summary>
        /// 是否已连接到RabbitMQ
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// 连接到RabbitMQ
        /// </summary>
        /// <returns>是否连接成功</returns>
        bool TryConnect();

        /// <summary>
        /// 建立通道
        /// </summary>
        /// <returns>到RabbitMQ的通道</returns>
        IModel CreateModel();
    }
}
