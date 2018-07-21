using System;
using System.Collections.Generic;
using System.Text;

namespace DXQ.Study.EventbusBlocks.EventBus.Manager {
    /// <summary>
    /// 订阅信息
    /// </summary>
    public class SubscriptionInfo {
        /// <summary>
        /// 是否匿名订阅
        /// </summary>
        public bool IsDynamic { get; }

        /// <summary>
        /// 处理器类型
        /// </summary>
        public Type HandlerType { get;  }

        /// <summary>
        /// 初始化订阅信息
        /// </summary>
        /// <param name="isDynamic">是否匿名订阅</param>
        /// <param name="handlerType">处理器类型</param>
        private SubscriptionInfo(bool isDynamic, Type handlerType) {
            this.IsDynamic = isDynamic;
            this.HandlerType = handlerType;
        }

        /// <summary>
        /// 获得匿名订阅信息
        /// </summary>
        /// <param name="handlerType">处理器类型</param>
        /// <returns>订阅信息</returns>
        public static SubscriptionInfo Dynamic(Type handlerType) {
            return new SubscriptionInfo(true, handlerType);
        }

        /// <summary>
        /// 获得非匿名订阅信息
        /// </summary>
        /// <param name="handlerType">处理器类型</param>
        /// <returns>订阅信息</returns>
        public static SubscriptionInfo Typed(Type handlerType) {
            return new SubscriptionInfo(false, handlerType);
        }
    }
}
