using Ordering.Domain.Events;
using Ordering.Domain.Exceptions;
using Ordering.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ordering.Domain.AggregateModel.BuyerAggregate {
    /// <summary>
    /// 卖家
    /// </summary>
    public class Buyer : Entity, IAggregateRoot {
        private List<PaymentMethod> _paymentMethods;

        #region 公共属性
        public string IdentityGuid { get; private set; }
        public string Name { get; private set; }
        public IEnumerable<PaymentMethod> PaymentMethods => _paymentMethods.AsReadOnly();
        #endregion

        /// <summary>
        /// 初始化卖家
        /// </summary>
        protected Buyer() {
            _paymentMethods = new List<PaymentMethod>();
        }

        /// <summary>
        /// 初始化卖家
        /// </summary>
        /// <param name="identity">卖家标识</param>
        /// <param name="name">名称</param>
        public Buyer(string identity, string name) : this() {
            IdentityGuid = !string.IsNullOrEmpty(identity) ? identity : throw new OrderingDomainException(nameof(identity));
            Name = !string.IsNullOrEmpty(name) ? name : throw new OrderingDomainException(nameof(name));
        }

        /// <summary>
        /// 验证/添加付款方式
        /// </summary>
        /// <param name="cardTypeId">卡类型Id</param>
        /// <param name="alias">别名</param>
        /// <param name="cardNumber">卡号</param>
        /// <param name="securityNumber">安全号</param>
        /// <param name="cardHolderName">持卡人姓名</param>
        /// <param name="expiration">过期时间</param>
        /// <param name="orderId">订单Id</param>
        /// <returns></returns>
        public PaymentMethod VerifyOrAddPaymentMethod(int cardTypeId, string alias, string cardNumber,
                                                      string securityNumber, string cardHolderName,
                                                      DateTime expiration, int orderId) {
            var existingPayment = _paymentMethods.SingleOrDefault(x => x.IsEqualTo(cardTypeId, cardNumber, expiration));

            if (existingPayment != null) {
                this.AddDomainEvent(new BuyerAndPaymentMethodVerifiedDomainEvent(this, existingPayment, orderId));
                return existingPayment;
            } else {
                var payment = new PaymentMethod(cardTypeId, alias, cardNumber, securityNumber, cardHolderName, expiration);
                _paymentMethods.Add(payment);
                this.AddDomainEvent(new BuyerAndPaymentMethodVerifiedDomainEvent(this, payment, orderId));

                return payment;
            }
        }
    }
}
