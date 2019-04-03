using DXQ.Study.IdentityServer4.AuthorizationServer.Configuration;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System.Linq;

namespace DXQ.Study.IdentityServer4.AuthorizationServer {
    /// <summary>
    /// 应用程序入口
    /// </summary>
    public class Program {
        /// <summary>
        /// Main Method
        /// </summary>
        /// <param name="args">参数</param>
        public static void Main(string[] args) {
            BuildWebHost(args)
                .MigrateDbContext<PersistedGrantDbContext>(null)
                .MigrateDbContext<ConfigurationDbContext>((context, provider) =>
                {
                    //添加Clients,IdentityResources,ApiResources
                    //context.Clients.RemoveRange(context.Clients.ToList());
                    if (!context.Clients.Any()) context.Clients.AddRange(InmemoryConfiguration.Clients().Select(x => x.ToEntity()));
                    if (!context.IdentityResources.Any()) context.IdentityResources.AddRange(InmemoryConfiguration.GetIdentityResources().Select(x => x.ToEntity()));
                    if (!context.ApiResources.Any()) context.ApiResources.AddRange(InmemoryConfiguration.ApiResources().Select(x => x.ToEntity()));
                    context.SaveChanges();
                })
                .Run();
        }

        /// <summary>
        /// 创建IWebHost
        /// </summary>
        /// <param name="args">参数</param>
        /// <returns>IWebHost</returns>
        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
