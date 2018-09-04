using Ordering.Domain.AggregateModel.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ordering.Api.Applications.Queries {
    /// <summary>
    /// 订单查询接口
    /// </summary>
    public interface IOrderQuery {
        /// <summary>
        /// 由Id获得订单
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>订单</returns>
        Task<Order> GetOrderAsync(int id);

        /// <summary>
        /// 获得用户订单集合
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>用户订单集合</returns>
        Task<IEnumerable<Order>> GetOrdersFromUserAsync(Guid userId);
    }
}
