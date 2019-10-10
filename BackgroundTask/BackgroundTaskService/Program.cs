using DotNetCore.WindowsServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BackgroundTaskService {
    /// <summary>
    /// 后台任务服务
    /// </summary>
    /// <remarks>
    /// sc create MyBackgroundTaskService binPath= "c:\myservice\BackgroundTaskService.exe" DisplayName= "MyBackgroundTaskService"
    /// </remarks>
    class Program {
        /// <summary>
        /// Main
        /// </summary>
        /// <param name="args">参数</param>
        static async Task Main(string[] args) {
            var isService = !(Debugger.IsAttached || args.Contains("-console"));

            var builder = BuildHost(args);
            if (isService) {
                await builder.RunAsServiceAsync();
            } else {
                await builder.RunConsoleAsync();
            }
        }

        /// <summary>
        /// 构建IHostBuilder
        /// </summary>
        /// <param name="args">参数</param>
        /// <returns>IHostBuilder</returns>
        private static IHostBuilder BuildHost(string[] args) => new HostBuilder()
            .ConfigureHostConfiguration(config =>
            {
                config.SetBasePath(Directory.GetCurrentDirectory());
            })
            .ConfigureAppConfiguration((host, config) =>
            {
                config.AddJsonFile("appsettings.json");
            })
            .ConfigureLogging((context, logging) =>
            {
                logging.AddConsole();
                logging.AddDebug();
            })
            .UseSerilog((context, config) =>
            {
                var file = @"E:\mystudy\BackgroundTask\BackgroundTaskService\bin\Debug\netcoreapp2.2\win7-x64\log\log-{Date}.txt";
                var templete = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] [{SourceContext}] [{EventId}] {Message}{NewLine}{Exception}";
                config.MinimumLevel.Debug()
                    .Enrich.FromLogContext()
                    .WriteTo.RollingFile(file, retainedFileCountLimit: 5, outputTemplate: templete, flushToDiskInterval: TimeSpan.FromSeconds(1));
            })
            .ConfigureServices((context, services) =>
            {
                services.AddHostedService<LoggingService>();
            });
    }
}
