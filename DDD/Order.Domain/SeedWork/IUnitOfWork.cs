using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Domain.SeedWork {
    /// <summary>
    /// 工作单元接口
    /// </summary>
    public interface IUnitOfWork : IDisposable {
        /// <summary>
        /// 异步保存改变
        /// </summary>
        /// <param name="cancellationToken">取消Token</param>
        /// <returns>受影响行数</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// 分发事件,异步单个事务保存所有实体
        /// </summary>
        /// <param name="cancellationToken">取消Token</param>
        /// <returns>是否成功保存</returns>
        Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
