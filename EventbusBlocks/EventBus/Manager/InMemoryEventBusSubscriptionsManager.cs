using DXQ.Study.EventbusBlocks.EventBus.Abstractions;
using DXQ.Study.EventbusBlocks.EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DXQ.Study.EventbusBlocks.EventBus.Manager {
    /// <summary>
    /// 事件总线订阅管理器接口内存实现
    /// </summary>
    public class InMemoryEventBusSubscriptionsManager : IEventBusSubscriptionsManager {
        private readonly List<Type> eventTypes = null;
        private readonly Dictionary<string, List<SubscriptionInfo>> handlers = null;

        /// <summary>
        /// 初始化事件总线订阅管理器接口内存实现
        /// </summary>
        public InMemoryEventBusSubscriptionsManager() {
            this.eventTypes = new List<Type>();
            this.handlers = new Dictionary<string, List<SubscriptionInfo>>();
        }

        /// <summary>
        /// 订阅管理器集合是否为空
        /// </summary>
        public bool IsEmpty => handlers.Keys.Any();

        /// <summary>
        /// 事件是否已删除
        /// </summary>
        public event EventHandler<string> OnEventRemoved;

        /// <summary>
        /// 添加订阅
        /// </summary>
        /// <typeparam name="T">集成事件</typeparam>
        /// <typeparam name="TH">处理集成事件的集成事件处理器</typeparam>
        public void AddSubscription<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T> {
            var eventName = this.GetEventKey<T>();
            this.ExecuteAddSubscription(eventName, typeof(TH), isDynamic: false);
            eventTypes.Add(typeof(T));
        }

        /// <summary>
        /// 添加匿名订阅
        /// </summary>
        /// <typeparam name="TH">匿名集成事件处理器</typeparam>
        /// <param name="eventName">事件名称</param>
        public void AddDynamicSubscription<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler {
            this.ExecuteAddSubscription(eventName, typeof(TH), isDynamic: true);
        }

        /// <summary>
        /// 执行添加订阅
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <param name="handlerType">处理器类型</param>
        /// <param name="isDynamic">是否匿名订阅</param>
        private void ExecuteAddSubscription(string eventName, Type handlerType, bool isDynamic) {
            //事件没有订阅,初始化之
            if (!this.HasSubscriptionForEvent(eventName)) {
                handlers.Add(eventName, new List<SubscriptionInfo>());
            }

            //不能重复订阅
            if (handlers[eventName].Any(x => x.HandlerType == handlerType)) {
                throw new ArgumentException($"事件[{eventName}]处理器类型[{handlerType.Name}]已存在");
            }

            //添加
            var subscriptionInfo = isDynamic ? SubscriptionInfo.Dynamic(handlerType) : SubscriptionInfo.Typed(handlerType);
            handlers[eventName].Add(subscriptionInfo);
        }

        /// <summary>
        /// 解除订阅
        /// </summary>
        /// <typeparam name="T">集成事件</typeparam>
        /// <typeparam name="TH">处理集成事件的集成事件处理器</typeparam>
        public void RemoveSubscription<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T> {
            var eventName = this.GetEventKey<T>();
            var subscriptionToRemove = this.FindSubscriptionInfoToRemove(eventName, typeof(TH));
            this.ExecuteRemoveSubscription(eventName, subscriptionToRemove);
        }

        /// <summary>
        /// 解除匿名订阅
        /// </summary>
        /// <typeparam name="TH">匿名集成事件处理器</typeparam>
        /// <param name="eventName">事件名称</param>
        public void RemoveDynamicSubscription<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler {
            var subscriptionToRemove = this.FindSubscriptionInfoToRemove(eventName, typeof(TH));
            this.ExecuteRemoveSubscription(eventName, subscriptionToRemove);
        }

        /// <summary>
        /// 查找要删除的订阅信息
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <param name="handlerType">处理器信息</param>
        /// <returns>订阅信息</returns>
        private SubscriptionInfo FindSubscriptionInfoToRemove(string eventName,Type handlerType) {
            //事件没有订阅信息
            if (!this.HasSubscriptionForEvent(eventName)) return null;

            //查找
            return handlers[eventName].SingleOrDefault(x => x.HandlerType == handlerType);
        }

        /// <summary>
        /// 执行解除订阅
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <param name="subscriptionToRemove">要解除的订阅信息</param>
        private void ExecuteRemoveSubscription(string eventName,SubscriptionInfo subscriptionToRemove) {
            if (subscriptionToRemove != null) {
                handlers[eventName].Remove(subscriptionToRemove);

                //事件没有了,删除集合项
                if (!handlers[eventName].Any()) {
                    handlers.Remove(eventName);
                    var eventType = eventTypes.SingleOrDefault(x => x.Name == eventName);
                    if (eventType != null) eventTypes.Remove(eventType);

                    //触发事件已删除事件
                    var handler = OnEventRemoved;
                    handler?.Invoke(this, eventName);
                }
            }
        }

        /// <summary>
        /// 指定类型集成事件是否存在订阅
        /// </summary>
        /// <typeparam name="T">集成事件</typeparam>
        /// <returns>是否存在订阅</returns>
        public bool HasSubscriptionsForEvent<T>()
            where T : IntegrationEvent {
            return this.HasSubscriptionForEvent(this.GetEventKey<T>());
        }

        /// <summary>
        /// 指定名称匿名事件是否存在订阅
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <returns>是否存在订阅</returns>
        public bool HasSubscriptionForEvent(string eventName) {
            return handlers.ContainsKey(eventName);
        }

        /// <summary>
        /// 由事件名称查找事件类型
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <returns>事件类型</returns>
        public Type GetEventTypeByName(string eventName) {
            return eventTypes.SingleOrDefault(x => x.Name == eventName);
        }

        /// <summary>
        /// 清空订阅管理器集合
        /// </summary>
        public void Clear() {
            handlers.Clear();
        }

        /// <summary>
        /// 查找指定类型集成事件订阅信息集合
        /// </summary>
        /// <typeparam name="T">集成事件</typeparam>
        /// <returns>集成事件订阅信息集合</returns>
        public IEnumerable<SubscriptionInfo> GetHandlersForEvent<T>()
            where T : IntegrationEvent {
            return this.GetHandlersForEvent(this.GetEventKey<T>());
        }

        /// <summary>
        /// 查找指定名称匿名事件订阅信息集合
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <returns>匿名事件订阅信息集合</returns>
        public IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName) {
            return handlers[eventName];
        }

        /// <summary>
        /// 获得类型T的名称
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <returns>类型T名称</returns>
        public string GetEventKey<T>() {
            return typeof(T).Name;
        }
    }
}
