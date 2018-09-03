using Ordering.Domain.Exceptions;
using Ordering.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ordering.Domain.AggregateModel.OrderAggregate {
    /// <summary>
    /// 单据状态扩展枚举值
    /// </summary>
    public class OrderStatus : Enumeration {
        public static OrderStatus Submitted = new OrderStatus(1, nameof(Submitted).ToLowerInvariant());
        public static OrderStatus AwaitingValidation = new OrderStatus(2, nameof(AwaitingValidation).ToLowerInvariant());
        public static OrderStatus StockConfirmed = new OrderStatus(3, nameof(StockConfirmed).ToLowerInvariant());
        public static OrderStatus Paid = new OrderStatus(4, nameof(Paid).ToLowerInvariant());
        public static OrderStatus Shipped = new OrderStatus(5, nameof(Shipped).ToLowerInvariant());
        public static OrderStatus Cancelled = new OrderStatus(6, nameof(Cancelled).ToLowerInvariant());

        /// <summary>
        /// 初始化订单状态扩展枚举
        /// </summary>
        protected OrderStatus() {

        }

        /// <summary>
        /// 初始化订单状态扩展枚举值
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="name">名称</param>
        public OrderStatus(int id, string name) : base(id, name) {

        }

        /// <summary>
        /// 订单状态值集合
        /// </summary>
        /// <returns>订单状态枚举值集合</returns>
        public static IEnumerable<OrderStatus> List() {
            return new[] { Submitted, AwaitingValidation, StockConfirmed, Paid, Shipped, Cancelled };
        }

        /// <summary>
        /// 由名称获得订单状态扩展枚举值
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>订单状态扩展枚举值</returns>
        public static OrderStatus From(string name) {
            var state = List().SingleOrDefault(x => string.Equals(x.Name, name, StringComparison.CurrentCultureIgnoreCase));
            if (state == null) {
                throw new OrderingDomainException($"Possible values for OrderStatus:{string.Join(",", List().Select(x => x.Name))}");
            }

            return state;
        }

        /// <summary>
        /// 由值获得订单状态扩展枚举值
        /// </summary>
        /// <param name="id">值</param>
        /// <returns>订单状态扩展枚举值</returns>
        public static OrderStatus From(int id) {
            var state = List().SingleOrDefault(x => x.Id == id);
            if (state == null) {
                throw new OrderingDomainException($"Possible values for OrderStatus:{string.Join(",", List().Select(x => x.Name))}");
            }

            return state;
        }
    }


}
