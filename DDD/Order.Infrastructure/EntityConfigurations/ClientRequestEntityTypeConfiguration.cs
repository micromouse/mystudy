using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Infrastructure.Idempotency;

namespace Ordering.Infrastructure.EntityConfigurations {
    /// <summary>
    /// 客户端请求实体类型配置
    /// </summary>
    public class ClientRequestEntityTypeConfiguration : IEntityTypeConfiguration<ClientRequest> {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="builder">配置器</param>
        public void Configure(EntityTypeBuilder<ClientRequest> builder) {
            builder.ToTable("clientrequests", OrderingDbContext.DEFAULT_SCHEMA);
            builder.HasKey(o => o.Id);

            builder.Property(o => o.Id).IsRequired();
            builder.Property(o => o.Name).HasMaxLength(200).IsRequired();
            builder.Property(o => o.Time).IsRequired();
        }
    }
}
