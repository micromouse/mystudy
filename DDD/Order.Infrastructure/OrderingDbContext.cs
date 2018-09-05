using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Ordering.Domain.SeedWork;
using Ordering.Infrastructure.EntityConfigurations;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Infrastructure {
    /// <summary>
    /// 订单DbContext
    /// </summary>
    public class OrderingDbContext : DbContext, IUnitOfWork {
        public const string DEFAULT_SCHEMA = "ordering";

        /// <summary>
        /// 初始化订单DbContext
        /// </summary>
        /// <param name="options">配置</param>
        public OrderingDbContext(DbContextOptions<OrderingDbContext> options) : base(options) {

        }

        /// <summary>
        /// 建立模型
        /// </summary>
        /// <param name="modelBuilder">模型构建器</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.ApplyConfiguration(new BuyerEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CardTypeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ClientRequestEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new OrderEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new OrderItemEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new OrderStatusEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PaymentMethodEntityTypeConfiguration());
        }

        /// <summary>
        /// 分发事件,异步单个事务保存所有实体
        /// </summary>
        /// <param name="cancellationToken">取消Token</param>
        /// <returns>是否成功保存</returns>
        public Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken)) {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 订单DbContext设计时工厂
    /// </summary>
    public class OrderingDbContextDesignFactory : IDesignTimeDbContextFactory<OrderingDbContext> {
        /// <summary>
        /// 生成表脚本
        /// </summary>
        /// <param name="args">参数</param>
        /// <returns></returns>
        public OrderingDbContext CreateDbContext(string[] args) {
            var builder = new DbContextOptionsBuilder<OrderingDbContext>();
            builder.UseSqlServer("Server=.;Initial Catalog=OrderingDb;Integrated Security=true");
            return new OrderingDbContext(builder.Options);
        }
    }

}
