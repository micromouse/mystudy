using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace DXQ.Study.IdentityServer4.MvcClient {
    /// <summary>
    /// 应用程序入口
    /// </summary>
    public class Program {
        /// <summary>
        /// 入口
        /// </summary>
        /// <param name="args">参数</param>
        public static void Main(string[] args) {
            BuildWebHost(args).Run();
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
