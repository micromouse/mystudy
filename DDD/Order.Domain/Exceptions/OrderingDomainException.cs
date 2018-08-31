using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Domain.Exceptions {
    /// <summary>
    /// 订单领域异常
    /// </summary>
    public class OrderingDomainException : Exception {
        /// <summary>
        /// 初始化订单领域异常
        /// </summary>
        public OrderingDomainException() {

        }

        /// <summary>
        /// 初始化订单领域异常
        /// </summary>
        /// <param name="message">消息</param>
        public OrderingDomainException(string message) : base(message) {

        }

        /// <summary>
        /// 初始化订单领域异常
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="innerException">内部异常</param>
        public OrderingDomainException(string message,Exception innerException) : base(message, innerException) {

        }
    }
}
