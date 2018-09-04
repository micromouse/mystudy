using Ordering.Domain.Exceptions;
using Ordering.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Domain.AggregateModel.BuyerAggregate {
    /// <summary>
    /// 付款方式
    /// </summary>
    public class PaymentMethod : Entity {
        #region 私有字段
        private string _alias;
        private string _cardNumber;
        private string _securityNumber;
        private string _cardHolderName;
        private DateTime _expiration;
        private int _cardTypeId;
        #endregion

        public CardType CardType { get; private set; }

        /// <summary>
        /// 初始化付款方式
        /// </summary>
        /// <param name="cardTypeId">卡类型Id</param>
        /// <param name="alias">别名</param>
        /// <param name="cardNubmer">卡号</param>
        /// <param name="securityNumber">安全号</param>
        /// <param name="cardHolderName">持卡人姓名</param>
        /// <param name="expiration">过期时间</param>
        public PaymentMethod(int cardTypeId, string alias, string cardNubmer, string securityNumber, string cardHolderName, DateTime expiration) {
            _cardNumber = !string.IsNullOrWhiteSpace(cardNubmer) ? cardNubmer : throw new OrderingDomainException(nameof(_cardNumber));
            _securityNumber = !string.IsNullOrWhiteSpace(securityNumber) ? securityNumber : throw new OrderingDomainException(nameof(securityNumber));
            _cardHolderName = !string.IsNullOrWhiteSpace(cardHolderName) ? cardHolderName : throw new OrderingDomainException(nameof(cardHolderName));

            if (expiration < DateTime.Now) {
                throw new OrderingDomainException(nameof(expiration));
            }

            _alias = alias;
            _expiration = expiration;
            _cardTypeId = cardTypeId;
        }

        /// <summary>
        /// 是否相同的卡
        /// </summary>
        /// <param name="cardTypeId">卡类型Id</param>
        /// <param name="cardNumber">卡号</param>
        /// <param name="expiration">过期时间</param>
        /// <returns>是否相同卡</returns>
        public bool IsEqualTo(int cardTypeId,string cardNumber,DateTime expiration) {
            return _cardTypeId == cardTypeId
                && _cardNumber == cardNumber
                && _expiration == expiration;
        }
    }
}
