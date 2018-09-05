using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.AggregateModel.BuyerAggregate;
using Ordering.Domain.AggregateModel.OrderAggregate;
using System;

namespace Ordering.Infrastructure.EntityConfigurations {
    /// <summary>
    /// 订单实体类型配置
    /// </summary>
    internal class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order> {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="builder">配置器</param>
        public void Configure(EntityTypeBuilder<Order> builder) {
            builder.ToTable("orders", OrderingDbContext.DEFAULT_SCHEMA);
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id).ForSqlServerUseSequenceHiLo("orderseq", OrderingDbContext.DEFAULT_SCHEMA);

            builder.Ignore(o => o.DomainEvents);
            builder.OwnsOne(o => o.Address);

            builder.Property<DateTime>("OrderDate").IsRequired();
            builder.Property<int?>("BuyerId").IsRequired(false);
            builder.Property<int>("OrderStatusId").IsRequired();
            builder.Property<int?>("PaymentMethodId").IsRequired(false);
            builder.Property<string>("Description").IsRequired(false);

            //Set as field (New since EF 1.1) to access the OrderItem collection property through its field
            var navigation = builder.Metadata.FindNavigation(nameof(Order.OrderItems));
            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.HasOne<PaymentMethod>()
                .WithMany()
                .HasForeignKey("FK_PaymentMehtodId")
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<Buyer>()
                .WithMany()
                .HasForeignKey("FK_BuyerId");
            builder.HasOne<OrderStatus>()
                .WithMany()
                .HasForeignKey("FK_OrderStatusId");
        }
    }
}
