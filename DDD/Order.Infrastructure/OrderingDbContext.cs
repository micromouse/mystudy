using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Storage;
using Ordering.Domain.AggregateModel.BuyerAggregate;
using Ordering.Domain.AggregateModel.OrderAggregate;
using Ordering.Domain.SeedWork;
using Ordering.Infrastructure.EntityConfigurations;
using System;
using System.Data;
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

        #region 数据库属性
        public const string DEFAULT_SCHEMA = "ordering";
        public DbSet<Order> Orders { get; set; }
        public DbSet<Buyer> Buyers { get; set; }
        #endregion

        #region 公共属性
        public IDbContextTransaction CurrentTransaction { get; private set; }
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
        /// 异步开启事务
        /// </summary>
        /// <returns>事务对象</returns>
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            if (CurrentTransaction != null) return null;

            CurrentTransaction = await this.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);
            return CurrentTransaction;
        }

        /// <summary>
        /// 异步提交事务
        /// </summary>
        /// <param name="transaction">事务</param>
        /// <returns>任务</returns>
        public async Task CommitTransactionAsync(IDbContextTransaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            if (transaction != CurrentTransaction) throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

            try
            {
                await this.SaveChangesAsync();
                transaction.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (CurrentTransaction != null)
                {
                    CurrentTransaction.Dispose();
                    CurrentTransaction = null;
                }
            }
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        public void RollbackTransaction()
        {
            try
            {
                CurrentTransaction?.Rollback();
            }
            finally
            {
                if (CurrentTransaction != null)
                {
                    CurrentTransaction.Dispose();
                    CurrentTransaction = null;
                }
            }
        }

        /// <summary>
        /// 分发事件,保存所有改变
        /// </summary>
        /// <param name="cancellationToken">取消Token</param>
        /// <returns>是否成功保存</returns>
        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            await _mediator.DispatchDomainEventsAsync(this);
            await base.SaveChangesAsync(cancellationToken);
            return true;
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
