using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.AggregateModel.BuyerAggregate;

namespace Ordering.Infrastructure.EntityConfigurations {
    /// <summary>
    /// 卡类型实体类型配置
    /// </summary>
    public class CardTypeEntityTypeConfiguration : IEntityTypeConfiguration<CardType> {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="builder">配置器</param>
        public void Configure(EntityTypeBuilder<CardType> builder) {
            builder.ToTable("cardtypes", OrderingDbContext.DEFAULT_SCHEMA);
            builder.HasKey(o => o.Id);

            builder.Property(o => o.Id).IsRequired();
            builder.Property(o => o.Id).HasMaxLength(200).IsRequired();
        }
    }
}
