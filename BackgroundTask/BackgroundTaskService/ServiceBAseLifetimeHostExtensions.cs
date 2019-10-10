using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace BackgroundTaskService {
    /// <summary>
    /// 服务生命期Host扩展
    /// </summary>
    public static class ServiceBaseLifetimeHostExtensions {
        /// <summary>
        /// 注册服务生命期
        /// </summary>
        /// <param name="hostBuilder">IHostBuilder</param>
        /// <returns>IHostBuilder</returns>
        public static IHostBuilder UseServiceBaseLifetime(this IHostBuilder hostBuilder) {
            return hostBuilder.ConfigureServices((context, services) =>
            {
                services.AddSingleton<IHostLifetime, ServiceBaseLifetime>();
            });
        }

        /// <summary>
        /// 异步服务运行IHost
        /// </summary>
        /// <param name="hostBuilder">IHostBuilder</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns>任务</returns>
        private static Task RunAsServiceAsync(this IHostBuilder hostBuilder, CancellationToken cancellationToken = default) {
            return hostBuilder.UseServiceBaseLifetime()
                .Build()
                .RunAsync(cancellationToken);
        }
    }
}
