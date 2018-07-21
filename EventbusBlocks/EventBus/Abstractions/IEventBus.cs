using DXQ.Study.EventbusBlocks.EventBus.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace DXQ.Study.EventbusBlocks.EventBus.Abstractions {
    /// <summary>
    /// 事件总线接口
    /// </summary>
    public interface IEventBus {
        /// <summary>
        /// 发布集成事件
        /// </summary>
        /// <param name="event">集成事件</param>
        void Publish(IntegrationEvent @event);

        /// <summary>
        /// 订阅集成事件
        /// </summary>
        /// <typeparam name="T">集成事件</typeparam>
        /// <typeparam name="TH">处理集成事件的集成事件处理器</typeparam>
        void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>;

        /// <summary>
        /// 订阅匿名集成事件
        /// </summary>
        /// <typeparam name="TH">匿名集成事件处理器</typeparam>
        /// <param name="eventName">事件名称</param>
        void SubscrbieDynamic<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler;

        /// <summary>
        /// 解除订阅集成事件
        /// </summary>
        /// <typeparam name="T">集成事件</typeparam>
        /// <typeparam name="TH">处理集成事件的集成事件处理器</typeparam>
        void Unsubscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>;

        /// <summary>
        /// 解除订阅匿名集成事件
        /// </summary>
        /// <typeparam name="TH">匿名集成事件处理器</typeparam>
        /// <param name="eventData">事件数据</param>
        void UnsubscribeDynamic<TH>(dynamic eventData)
            where TH : IDynamicIntegrationEventHandler;
    }
}
