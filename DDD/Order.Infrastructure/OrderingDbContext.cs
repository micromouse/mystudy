using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Ordering.Domain.AggregateModel.BuyerAggregate;
using Ordering.Domain.AggregateModel.OrderAggregate;
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
        private readonly IMediator _mediator;

        #region 公共属性
        public const string DEFAULT_SCHEMA = "ordering";
        public DbSet<Order> Orders { get; set; }
        public DbSet<Buyer> Buyers { get; set; }
        #endregion

        /// <summary>
        /// 初始化订单DbContext
        /// </summary>
        /// <param name="options">配置</param>
        public OrderingDbContext(DbContextOptions<OrderingDbContext> options) : base(options) {

        }

        /// <summary>
        /// 初始化订单DbContext
        /// </summary>
        /// <param name="options">配置</param>
        /// <param name="mediator">IMediator</param>
        public OrderingDbContext(DbContextOptions<OrderingDbContext> options, IMediator mediator) : this(options) {
            _mediator = mediator;
        }

        /// <summary>
        /// 建立模型
        /// </summary>
        /// <param name="modelBuilder">模型构建器</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.ApplyConfiguration(new ClientRequestEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PaymentMethodEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new OrderEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new OrderItemEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CardTypeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new OrderStatusEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new BuyerEntityTypeConfiguration());
        }

        /// <summary>
        /// 分发事件,保存改变实体
        /// </summary>
        /// <param name="cancellationToken">取消Token</param>
        /// <returns>是否成功保存</returns>
        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken)) {
            await _mediator.DispatchDomainEventsAsync(this);
            var result = await base.SaveChangesAsync(cancellationToken);
            return true;
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
