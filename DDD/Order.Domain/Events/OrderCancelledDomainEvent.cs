using MediatR;
using Ordering.Domain.AggregateModel.OrderAggregate;

namespace Ordering.Domain.Events {
    /// <summary>
    /// 订单已取消领域事件
    /// </summary>
    public class OrderCancelledDomainEvent : INotification {
        public Order Order { get; }

        /// <summary>
        /// 初始化订单已取消领域事件
        /// </summary>
        /// <param name="order">订单</param>
        public OrderCancelledDomainEvent(Order order) {
            Order = order;
        }
    }
}
