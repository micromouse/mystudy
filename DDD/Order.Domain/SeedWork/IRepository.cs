namespace Ordering.Domain.SeedWork {
    /// <summary>
    /// 仓储接口
    /// </summary>
    /// <typeparam name="T">聚合根类型</typeparam>
    public interface IRepository<T> where T : IAggregateRoot {
        /// <summary>
        /// 工作单元接口实例
        /// </summary>
        IUnitOfWork UnitOfWork { get; }
    }
}
