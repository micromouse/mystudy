using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.AggregateModel.BuyerAggregate;
using System;

namespace Ordering.Infrastructure.EntityConfigurations {
    /// <summary>
    /// 付款方式实体类型配置
    /// </summary>
    public class PaymentMethodEntityTypeConfiguration : IEntityTypeConfiguration<PaymentMethod> {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="builder">配置器</param>
        public void Configure(EntityTypeBuilder<PaymentMethod> builder) {
            builder.ToTable("paymentmethods", OrderingDbContext.DEFAULT_SCHEMA);
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id).ForSqlServerUseSequenceHiLo("paymentseq", OrderingDbContext.DEFAULT_SCHEMA);
            builder.Ignore(o => o.DomainEvents);

            builder.Property<int>("BuyerId").IsRequired();
            builder.Property<string>("CardHolderName").HasMaxLength(200).IsRequired();
            builder.Property<string>("Alias").HasMaxLength(200).IsRequired();
            builder.Property<string>("CardNumber").HasMaxLength(25).IsRequired();
            builder.Property<DateTime>("Expiration").IsRequired();
            builder.Property<int>("CardTypeId").IsRequired();

            builder.HasOne(o => o.CardType)
                .WithMany()
                .HasForeignKey("FK_CardTypeId");
        }
    }
}
