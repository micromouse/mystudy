using Ordering.Domain.Events;
using Ordering.Domain.SeedWork;
using System;
using System.Collections.Generic;

namespace Ordering.Domain.AggregateModel.OrderAggregate {
    /// <summary>
    /// 订单实体聚合根
    /// </summary>
    public class Order : Entity, IAggregateRoot {
        #region 私有字段
        //Using private fields, allowed since EF Core 1.1, is a much better encapsulation
        private DateTime _orderDate;
        private int? _buyerId;
        private int _orderStatusId;
        private string _description;
        private bool _isDraft;
        private int? _paymentMethodId;
        private readonly List<OrderItem> _orderItems;
        #endregion

        #region 公共属性
        /// <summary>
        /// 地址
        /// </summary>
        public Address Address { get; private set; }
        /// <summary>
        /// 买家Id
        /// </summary>
        public int? GetBuyerId => _buyerId;
        /// <summary>
        /// 单据状态
        /// </summary>
        public OrderStatus OrderStatus { get; private set; }
        /// <summary>
        /// 单据项集合
        /// </summary>
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;
        #endregion

        /// <summary>
        /// 新草稿单
        /// </summary>
        /// <returns>草稿订单</returns>
        public static Order NewDraft() {
            var order = new Order {
                _isDraft = true
            };
            return order;
        }

        /// <summary>
        /// 初始化新订单
        /// </summary>
        protected Order() {
            _orderItems = new List<OrderItem>();
            _isDraft = false;
        }

        /// <summary>
        /// 初始化新订单
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="userName">用户名</param>
        /// <param name="address">地址</param>
        /// <param name="cardTypeId">卡类型Id</param>
        /// <param name="cardNumber">卡号</param>
        /// <param name="cardSecurityNumber">卡安全号</param>
        /// <param name="cardHolderName">持卡人姓名</param>
        /// <param name="cardExpiration">卡过期时间</param>
        /// <param name="buyerId">购买人Id</param>
        /// <param name="paymentMethodId">付款方式Id</param>
        public Order(string userId, string userName, Address address, int cardTypeId, string cardNumber,
                    string cardSecurityNumber, string cardHolderName, DateTime cardExpiration,
                    int? buyerId = null, int? paymentMethodId = null) : this() {
            _buyerId = buyerId;
            _paymentMethodId = paymentMethodId;
            _orderDate = DateTime.Now;
            Address = address;
            this.AddDomainEvent(new OrderStartedDomainEvent(this, userId, userName, cardTypeId, 
                                                            cardNumber, cardSecurityNumber, 
                                                            cardHolderName, cardExpiration));
        }

        /// <summary>
        /// 添加订单项
        /// </summary>
        public void AddOrderItem() {

        }
    }
}
