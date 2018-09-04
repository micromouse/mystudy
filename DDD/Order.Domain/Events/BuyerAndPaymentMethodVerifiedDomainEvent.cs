using MediatR;
using Ordering.Domain.AggregateModel.BuyerAggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Domain.Events {
    /// <summary>
    /// 卖家付款方式验证领域事件
    /// </summary>
    public class BuyerAndPaymentMethodVerifiedDomainEvent : INotification {
        public Buyer Buyer { get; private set; }
        public PaymentMethod PaymentMethod { get; private set; }
        public int OrderId { get; private set; }

        /// <summary>
        /// 初始化卖家付款方式验证领域事件
        /// </summary>
        /// <param name="buyer">卖家</param>
        /// <param name="paymentMethod">付款方式</param>
        /// <param name="orderId">订单Id</param>
        public BuyerAndPaymentMethodVerifiedDomainEvent(Buyer buyer,PaymentMethod paymentMethod,int orderId) {
            Buyer = buyer;
            PaymentMethod = paymentMethod;
            OrderId = orderId;
        }
    }
}
