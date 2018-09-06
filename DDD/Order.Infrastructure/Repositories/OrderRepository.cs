using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ordering.Domain.AggregateModel.OrderAggregate;
using Ordering.Domain.SeedWork;

namespace Ordering.Infrastructure.Repositories {
    /// <summary>
    /// 订单仓储
    /// </summary>
    public class OrderRepository : IOrderRepository {
        private readonly OrderingDbContext _context;

        /// <summary>
        /// 初始化订单仓储
        /// </summary>
        /// <param name="unitOfWork">工作单元</param>
        public OrderRepository(OrderingDbContext context) {
            _context = context;
        }

        /// <summary>
        /// 工作单元
        /// </summary>
        public IUnitOfWork UnitOfWork => _context;

        /// <summary>
        /// 添加订单
        /// </summary>
        /// <param name="order">订单</param>
        /// <returns>添加的订单</returns>
        public Order Add(Order order) {
            return _context.Orders
                .Add(order)
                .Entity;
        }

        /// <summary>
        /// 更新订单
        /// </summary>
        /// <param name="order">订单</param>
        public async Task<Order> GetAsync(int orderId) {
            return await _context.Orders.FindAsync(orderId);
        }

        /// <summary>
        /// 获得订单
        /// </summary>
        /// <param name="orderId">订单Id</param>
        /// <returns>订单</returns>
        public void Update(Order order) {
            _context.Entry(order).State = EntityState.Modified;
        }
    }
}
