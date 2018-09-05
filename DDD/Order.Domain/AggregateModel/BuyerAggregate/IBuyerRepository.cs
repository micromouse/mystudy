using System.Threading.Tasks;

namespace Ordering.Domain.AggregateModel.BuyerAggregate {
    /// <summary>
    /// 卖家接口
    /// </summary>
    public interface IBuyerRepository {
        /// <summary>
        /// 添加买家
        /// </summary>
        /// <param name="buyer">买家</param>
        /// <returns>新添加的买家</returns>
        Buyer Add(Buyer buyer);

        /// <summary>
        /// 更新买家
        /// </summary>
        /// <param name="buyer">买家</param>
        /// <returns>买家</returns>
        Buyer Update(Buyer buyer);

        /// <summary>
        /// 查找买家
        /// </summary>
        /// <param name="buyerIdentityGuid">买家标识</param>
        /// <returns>买家</returns>
        Task<Buyer> FindAsync(string buyerIdentityGuid);

        /// <summary>
        /// 查找买家
        /// </summary>
        /// <param name="id">买家Id</param>
        /// <returns>买家</returns>
        Task<Buyer> FindByIdAsync(string id);
    }
}
