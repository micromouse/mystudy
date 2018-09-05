using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ordering.Domain.AggregateModel.OrderAggregate;

namespace Ordering.Api.Applications.Queries {
    /// <summary>
    /// 订单查询
    /// </summary>
    public class OrderQuery : IOrderQuery {
        private readonly string _connectionString;

        /// <summary>
        /// 订单查询
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        public OrderQuery(string connectionString) {
            _connectionString = connectionString;
        }

        /// <summary>
        /// 由Id获得订单
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>订单</returns>
        public Task<Order> GetOrderAsync(int id) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获得用户订单集合
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>用户订单集合</returns>
        public Task<IEnumerable<Order>> GetOrdersFromUserAsync(Guid userId) {
            throw new NotImplementedException();
        }
    }
}
