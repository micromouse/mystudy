using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DXQ.Study.IdentityServer4.AuthorizationServer.Configuration {
    /// <summary>
    /// IWebHost扩展
    /// </summary>
    public static class IWebHostExtensions {
        /// <summary>
        /// 迁移数据库
        /// </summary>
        /// <typeparam name="TContext">DbContext类型</typeparam>
        /// <param name="webHost">IWebHost</param>
        /// <param name="seeder">迁移后回掉</param>
        /// <returns>IWebHost</returns>
        public static IWebHost MigrateDbContext<TContext>(this IWebHost webHost, Action<TContext, IServiceProvider> seeder) where TContext : DbContext {
            using (var scope = webHost.Services.CreateScope()) {
                var provider = scope.ServiceProvider;
                var context = provider.GetRequiredService<TContext>();

                context.Database.Migrate();
                seeder?.Invoke(context, provider);

                return webHost;
            }
        }
    }
}
