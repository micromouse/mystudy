using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ordering.Api.Infrastructure {
    /// <summary>
    /// IWebHost扩展
    /// </summary>
    public static class IWebHostExtensions {
        /// <summary>
        /// 迁移DbContext
        /// </summary>
        /// <typeparam name="TDbContext">DbContext</typeparam>
        /// <param name="host">IWebHost</param>
        /// <returns>IWebHost</returns>
        public static IWebHost MigrateDbContext<TDbContext>(this IWebHost host) where TDbContext : DbContext {
            using (var scope = host.Services.CreateScope()) {
                var context = scope.ServiceProvider.GetRequiredService<TDbContext>();
                context.Database.Migrate();
            }

            return host;
        }
    }
}
