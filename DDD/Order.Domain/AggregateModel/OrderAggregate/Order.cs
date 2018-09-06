using Ordering.Domain.Events;
using Ordering.Domain.Exceptions;
using Ordering.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ordering.Domain.AggregateModel.OrderAggregate {
    /// <summary>
    /// 订单
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
            _orderStatusId = OrderStatus.Submitted.Id;
            _orderDate = DateTime.Now;
            Address = address;
            this.AddDomainEvent(new OrderStartedDomainEvent(this, userId, userName, cardTypeId, cardNumber, cardSecurityNumber, cardHolderName, cardExpiration));
        }

        /// <summary>
        /// 添加订单项
        /// </summary>
        /// <param name="productId">产品Id</param>
        /// <param name="productName">产品名称</param>
        /// <param name="unitPrice">单价</param>
        /// <param name="discount">折扣</param>
        /// <param name="pictureUrl">图片Url</param>
        /// <param name="units">数量</param>
        public void AddOrderItem(int productId, string productName, decimal unitPrice, decimal discount, string pictureUrl, int units = 1) {
            var existingOrderItemForProduct = _orderItems.SingleOrDefault(x => x.ProductId == productId);

            if (existingOrderItemForProduct != null) {
                if (discount > existingOrderItemForProduct.GetCurrentDiscount()) {
                    existingOrderItemForProduct.SetNewDiscount(discount);
                }
                existingOrderItemForProduct.AddUnits(units);
            } else {
                var orderItem = new OrderItem(productId, productName, unitPrice, discount, pictureUrl, units);
                _orderItems.Add(orderItem);
            }
        }

        /// <summary>
        /// 设置订单状态为已取消
        /// </summary>
        public void SetCancelledStatus() {
            if (_orderStatusId == OrderStatus.Paid.Id || _orderStatusId == OrderStatus.Shipped.Id) {
                this.StatusChangeException(OrderStatus.Cancelled);
            }

            _orderStatusId = OrderStatus.Cancelled.Id;
            _description = "The order wa cancelled.";
            this.AddDomainEvent(new OrderCancelledDomainEvent(this));
        }

        /// <summary>
        /// 设置付款方式Id
        /// </summary>
        /// <param name="id">付款方式Id</param>
        public void SetPaymentId(int id) {
            _paymentMethodId = id;
        }

        /// <summary>
        /// 设置买家Id
        /// </summary>
        /// <param name="id">买家Id</param>
        public void SetBuyerId(int id) {
            _buyerId = id;
        }

        /// <summary>
        /// 订单状态改变异常
        /// </summary>
        /// <param name="orderStatusToChange">要改变到的状态</param>
        private void StatusChangeException(OrderStatus orderStatusToChange) {
            throw new OrderingDomainException($"Is not possible to change the order status from {OrderStatus.Name} to {orderStatusToChange.Name}.");
        }
    }
}
