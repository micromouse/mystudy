using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.AggregateModel.OrderAggregate;

namespace Ordering.Infrastructure.EntityConfigurations {
    /// <summary>
    /// 订单项实体类型配置
    /// </summary>
    public class OrderItemEntityTypeConfiguration : IEntityTypeConfiguration<OrderItem> {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="builder">配置器</param>
        public void Configure(EntityTypeBuilder<OrderItem> builder) {
            builder.ToTable("orderitems", OrderingDbContext.DEFAULT_SCHEMA);
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id).ForSqlServerUseSequenceHiLo("orderitemseq", OrderingDbContext.DEFAULT_SCHEMA);
            builder.Ignore(o => o.DomainEvents);

            builder.Property<int>("OrderId").IsRequired();
            builder.Property<decimal>("Discount").HasColumnType(typeof(decimal).Name).IsRequired();
            builder.Property<int>("ProductId").IsRequired();
            builder.Property<string>("ProductName").IsRequired(false);
            builder.Property<decimal>("UnitPrice").HasColumnType(typeof(decimal).Name).IsRequired();
            builder.Property<int>("Units").IsRequired();
            builder.Property<string>("PictureUrl").IsRequired(false);
        }
    }
}
