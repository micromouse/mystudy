using System;
using Ordering.Domain.Exceptions;
using Ordering.Domain.SeedWork;

namespace Ordering.Domain.AggregateModel.OrderAggregate {
    /// <summary>
    /// 订单项
    /// </summary>
    public class OrderItem : Entity {
        #region 私有字段
        //Using private fields, allowed since EF Core 1.1, is a much better encapsulation
        private string _productName;
        private string _pictureUrl;
        private decimal _unitPrice;
        private decimal _discount;
        private int _units;
        #endregion

        /// <summary>
        /// 产品Id
        /// </summary>
        public int ProductId { get; private set; }

        /// <summary>
        /// 初始化订单项
        /// </summary>
        protected OrderItem() {

        }

        /// <summary>
        /// 初始化订单项
        /// </summary>
        /// <param name="productId">产品Id</param>
        /// <param name="productName">产品名称</param>
        /// <param name="unitPrice">单价</param>
        /// <param name="discount">折扣</param>
        /// <param name="pictureUrl">产品图片Url</param>
        /// <param name="units">数量</param>
        public OrderItem(int productId, string productName, decimal unitPrice, decimal discount, string pictureUrl, int units = 1) {
            if (units < 1) throw new OrderingDomainException("Invalid number of units");
            if (unitPrice * units < discount) throw new OrderingDomainException("The total of order item is lower than applied discount");

            ProductId = productId;
            _productName = productName;
            _unitPrice = unitPrice;
            _discount = discount;
            _units = units;
            _pictureUrl = pictureUrl;
        }

        /// <summary>
        /// 获得当前折扣
        /// </summary>
        /// <returns>当前折扣</returns>
        public decimal GetCurrentDiscount() {
            return _discount;
        }

        /// <summary>
        /// 设置新折扣
        /// </summary>
        /// <param name="discount">折扣</param>
        public void SetNewDiscount(decimal discount) {
            if (discount < 0) throw new OrderingDomainException("Discount is not valid");
            _discount = discount;
        }

        /// <summary>
        /// 添加数量
        /// </summary>
        /// <param name="units">数量</param>
        public void AddUnits(int units) {
            if (units < 0) throw new OrderingDomainException("Invalid units");
            _units += units;
        }
    }
}
