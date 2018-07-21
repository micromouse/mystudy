using DXQ.Study.EventbusBlocks.EventBus.Abstractions;
using DXQ.Study.EventbusBlocks.EventBus.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace DXQ.Study.EventbusBlocks.EventBus.Manager {
    /// <summary>
    /// 事件总线订阅管理器接口
    /// </summary>
    public interface IEventBusSubscriptionsManager {
        /// <summary>
        /// 订阅管理器集合是否为空
        /// </summary>
        bool IsEmpty { get; }

        /// <summary>
        /// 事件是否已删除
        /// </summary>
        event EventHandler<string> OnEventRemoved;

        /// <summary>
        /// 添加订阅
        /// </summary>
        /// <typeparam name="T">集成事件</typeparam>
        /// <typeparam name="TH">处理集成事件的集成事件处理器</typeparam>
        void AddSubscription<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>;

        /// <summary>
        /// 添加匿名订阅
        /// </summary>
        /// <typeparam name="TH">匿名集成事件处理器</typeparam>
        /// <param name="eventName">事件名称</param>
        void AddDynamicSubscription<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler;

        /// <summary>
        /// 解除订阅
        /// </summary>
        /// <typeparam name="T">集成事件</typeparam>
        /// <typeparam name="TH">处理集成事件的集成事件处理器</typeparam>
        void RemoveSubscription<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>;

        /// <summary>
        /// 解除匿名订阅
        /// </summary>
        /// <typeparam name="TH">匿名集成事件处理器</typeparam>
        /// <param name="eventName">事件名称</param>
        void RemoveDynamicSubscription<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler;

        /// <summary>
        /// 指定类型集成事件是否存在订阅
        /// </summary>
        /// <typeparam name="T">集成事件</typeparam>
        /// <returns>是否存在订阅</returns>
        bool HasSubscriptionsForEvent<T>()
            where T : IntegrationEvent;

        /// <summary>
        /// 指定名称匿名事件是否存在订阅
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <returns>是否存在订阅</returns>
        bool HasSubscriptionForEvent(string eventName);

        /// <summary>
        /// 由事件名称查找事件类型
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <returns>事件类型</returns>
        Type GetEventTypeByName(string eventName);

        /// <summary>
        /// 清空订阅管理器集合
        /// </summary>
        void Clear();

        /// <summary>
        /// 查找指定类型集成事件订阅信息集合
        /// </summary>
        /// <typeparam name="T">集成事件</typeparam>
        /// <returns>集成事件订阅信息集合</returns>
        IEnumerable<SubscriptionInfo> GetHandlersForEvent<T>()
            where T : IntegrationEvent;

        /// <summary>
        /// 查找指定名称匿名事件订阅信息集合
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <returns>匿名事件订阅信息集合</returns>
        IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName);

        /// <summary>
        /// 获得类型T的名称
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <returns>类型T名称</returns>
        string GetEventKey<T>();
    }
}
