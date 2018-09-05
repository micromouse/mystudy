using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.AggregateModel.OrderAggregate;

namespace Ordering.Infrastructure.EntityConfigurations {
    /// <summary>
    /// 订单状态实体类型配置
    /// </summary>
    public class OrderStatusEntityTypeConfiguration : IEntityTypeConfiguration<OrderStatus> {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="builder">配置器</param>
        public void Configure(EntityTypeBuilder<OrderStatus> builder) {
            builder.ToTable("orderstatus", OrderingDbContext.DEFAULT_SCHEMA);
            builder.HasKey(o => o.Id).ForSqlServerIsClustered();

            builder.Property(o => o.Id).IsRequired();
            builder.Property(o => o.Name).HasMaxLength(200).IsRequired();
        }
    }
}
