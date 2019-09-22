using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace ConsulAndOcelot.ServiceA {
    /// <summary>
    /// ConsulAndOcelot服务A
    /// </summary>
    public class Program {
        /// <summary>
        /// Main
        /// </summary>
        /// <param name="args">参数</param>
        public static void Main(string[] args) {
            CreateWebHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// 建立IWebHostBuilder
        /// </summary>
        /// <param name="args">参数</param>
        /// <returns>IWebHostBuilder</returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();
            var url = configuration["ApplicationUrl"] ?? "http://0.0.0.0:8100";

            return WebHost.CreateDefaultBuilder(args)
                .UseConfiguration(configuration)
                .UseKestrel()
                .UseUrls(url)
                .UseStartup<Startup>();
        }
    }
}
