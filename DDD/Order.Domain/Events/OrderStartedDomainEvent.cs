using MediatR;
using Ordering.Domain.AggregateModel.OrderAggregate;
using System;

namespace Ordering.Domain.Events {
    /// <summary>
    /// 单据已开始领域事件
    /// </summary>
    public class OrderStartedDomainEvent : INotification {
        public string UserId { get; }
        public string UserName { get; }
        public int CardTypeId { get; }
        public string CardNumber { get; }
        public string CardSecurityNumber { get; }
        public string CardHolderName { get; }
        public DateTime CardExpiration { get; }
        public Order Order { get; }

        /// <summary>
        /// 初始化单据已开始领域事件
        /// </summary>
        /// <param name="order">单据实体</param>
        /// <param name="userId">用户Id</param>
        /// <param name="userName">用户名</param>
        /// <param name="cardTypeId">卡类型Id</param>
        /// <param name="cardNumber">卡号</param>
        /// <param name="cardSecurityNumber">卡安全号</param>
        /// <param name="cardHolderName">持卡人姓名</param>
        /// <param name="cardExpiration">卡过期时间</param>
        public OrderStartedDomainEvent(Order order, string userId, string userName,
                                        int cardTypeId, string cardNumber, string cardSecurityNumber,
                                        string cardHolderName, DateTime cardExpiration) {
            Order = order;
            UserId = UserId;
            UserName = userName;
            CardTypeId = cardTypeId;
            CardNumber = cardNumber;
            CardSecurityNumber = cardSecurityNumber;
            CardHolderName = cardHolderName;
            CardExpiration = cardExpiration;
        }

    }
}
