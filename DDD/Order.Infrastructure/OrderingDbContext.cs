using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Ordering.Domain.AggregateModel.BuyerAggregate;
using Ordering.Domain.AggregateModel.OrderAggregate;
using Ordering.Domain.SeedWork;
using Ordering.Infrastructure.EntityConfigurations;
using System;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Infrastructure
{
    /// <summary>
    /// 订单DbContext
    /// </summary>
    public class OrderingDbContext : DbContext, IUnitOfWork
    {
        private readonly IMediator _mediator;
        private IDbContextTransaction _currentTransaction;

        #region 数据库属性
        public const string DEFAULT_SCHEMA = "ordering";
        public DbSet<Order> Orders { get; set; }
        public DbSet<Buyer> Buyers { get; set; }
        #endregion

        #region 公共属性
        public bool HasActiveTransaction => _currentTransaction != null;
        #endregion


        #region 构造函数
        /// <summary>
        /// 初始化订单DbContext
        /// </summary>
        /// <param name="options">配置</param>
        public OrderingDbContext(DbContextOptions<OrderingDbContext> options) : base(options)
        {

        }

        /// <summary>
        /// 初始化订单DbContext
        /// </summary>
        /// <param name="options">配置</param>
        /// <param name="mediator">IMediator</param>
        public OrderingDbContext(DbContextOptions<OrderingDbContext> options, IMediator mediator) : this(options)
        {
            _mediator = mediator;
        }
        #endregion

        /// <summary>
        /// 建立模型
        /// </summary>
        /// <param name="modelBuilder">模型构建器</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ClientRequestEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PaymentMethodEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new OrderEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new OrderItemEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CardTypeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new OrderStatusEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new BuyerEntityTypeConfiguration());
            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// 异步提交事务
        /// </summary>
        /// <param name="transaction">事务</param>
        /// <returns>任务</returns>
        public async Task CommitTransactionAsync(IDbContextTransaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            if (transaction != _currentTransaction) throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

            try
            {
                await this.SaveChangesAsync();
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
            }
        }

        /// <summary>
        /// 异步开启事务
        /// </summary>
        /// <returns>事务对象</returns>
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            if (_currentTransaction != null) return null;

            _currentTransaction = await this.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);
            return _currentTransaction;
        }

        /// <summary>
        /// 分发事件,保存所有改变
        /// </summary>
        /// <param name="cancellationToken">取消Token</param>
        /// <param name="isOnlyDispatchDomainEvents">是否仅仅分发领域事件,缺省为是</param>
        /// <returns>是否成功保存</returns>
        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken), bool isOnlyDispatchDomainEvents = true)
        {
            await _mediator.DispatchDomainEventsAsync(this);
            if (!isOnlyDispatchDomainEvents)
            {
                await base.SaveChangesAsync(cancellationToken);
            }
            return true;

            #region 使用数据库事务方式
            /*
            try {
                //根聚合开始事务
                var existsTransaction = this.Database.CurrentTransaction != null;
                if (!existsTransaction) await this.Database.BeginTransactionAsync();

                await _mediator.DispatchDomainEventsAsync(this);

                //只有顶级的聚合根才能提交事务
                if (!existsTransaction) {
                    await base.SaveChangesAsync(cancellationToken);
                    this.Database.CommitTransaction();
                }

                return true;
            } catch {
                this.Database.RollbackTransaction();
                throw;
            }
            */
            #endregion
        }
    }

    /// <summary>
    /// 订单DbContext设计时工厂
    /// </summary>
    public class OrderingDbContextDesignFactory : IDesignTimeDbContextFactory<OrderingDbContext>
    {
        /// <summary>
        /// 生成表脚本
        /// </summary>
        /// <param name="args">参数</param>
        /// <returns></returns>
        public OrderingDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<OrderingDbContext>();
            builder.UseSqlServer("Server=.;Initial Catalog=OrderingDb;Integrated Security=true");
            return new OrderingDbContext(builder.Options);
        }
    }

}
