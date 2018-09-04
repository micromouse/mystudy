using Ordering.Domain.SeedWork;
using System.Threading.Tasks;

namespace Ordering.Domain.AggregateModel.OrderAggregate {
    /// <summary>
    /// 订单仓储接口
    /// </summary>
    public interface IOrderRepository : IRepository<Order> {
        /// <summary>
        /// 添加订单
        /// </summary>
        /// <param name="order">订单</param>
        /// <returns>添加的订单</returns>
        Order Add(Order order);

        /// <summary>
        /// 更新订单
        /// </summary>
        /// <param name="order">订单</param>
        void Update(Order order);

        /// <summary>
        /// 获得订单
        /// </summary>
        /// <param name="orderId">订单Id</param>
        /// <returns>订单</returns>
        Task<Order> GetAsync(int orderId);
    }
}
