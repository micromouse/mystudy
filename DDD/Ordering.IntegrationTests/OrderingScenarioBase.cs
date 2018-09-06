using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Ordering.Api.Infrastructure;
using Ordering.Infrastructure;

namespace Ordering.IntegrationTests {
    /// <summary>
    /// 订单集成测试基类
    /// </summary>
    public class OrderingScenarioBase {
        /// <summary>
        /// 建立Ordering.Api测试服务器
        /// </summary>
        /// <returns>测试服务器</returns>
        public TestServer CreateServer() {
            var path = Assembly.GetAssembly(typeof(OrderingScenarioBase)).Location;

            var hostBuilder = new WebHostBuilder()
                .UseContentRoot(Path.GetDirectoryName(path))
                .ConfigureAppConfiguration(cb =>
                {
                    cb.AddJsonFile("appsettings.json", optional: false)
                    .AddEnvironmentVariables();
                })
                .UseStartup<OrderingTestsStartup>();
            var testServer = new TestServer(hostBuilder);
            testServer.Host.MigrateDbContext<OrderingDbContext>();

            return testServer;
        }
    }
}
