using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.AggregateModel.BuyerAggregate;

namespace Ordering.Infrastructure.EntityConfigurations {
    /// <summary>
    /// 买家实体类型配置
    /// </summary>
    public class BuyerEntityTypeConfiguration : IEntityTypeConfiguration<Buyer> {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="builder">配置器</param>
        public void Configure(EntityTypeBuilder<Buyer> builder) {
            builder.ToTable("buyers", OrderingDbContext.DEFAULT_SCHEMA);
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id).ForSqlServerUseSequenceHiLo("buyerseq", OrderingDbContext.DEFAULT_SCHEMA);
            builder.Ignore(o => o.DomainEvents);

            builder.Property(o => o.IdentityGuid).HasMaxLength(200).IsRequired();
            builder.HasIndex("IdentityGuid").HasName("IX_IdentityGuid").IsUnique();

            builder.Property(o => o.Name).IsRequired();
            builder.HasMany(o => o.PaymentMethods)
                .WithOne()
                .HasForeignKey("FK_BuyerId")
                .OnDelete(DeleteBehavior.Cascade);

            var navigation = builder.Metadata.FindNavigation(nameof(Buyer.PaymentMethods));
            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
