using Grpc.Core;
using Grpc.Core.Logging;
using MagicOnion.Server;
using MagicOnion.Service.Implemention;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace MagicOnion.Service.Server {
    /// <summary>
    /// 应用程序
    /// </summary>
    public class Program {
        /// <summary>
        /// Main
        /// </summary>
        /// <param name="args">参数</param>
        public static void Main(string[] args) {
            Environment.SetEnvironmentVariable("SETTINGS_MAX_HEADER_LIST_SIZE", "1000000");
            GrpcEnvironment.SetLogger(new ConsoleLogger());

            var options = new MagicOnionOptions(true) {
                MagicOnionLogger = new MagicOnionLogToGrpcLoggerWithNamedDataDump(),
                GlobalFilters = new MagicOnionFilterAttribute[] { },
                EnableCurrentContext = true
            };
            var service = MagicOnionEngine.BuildServerServiceDefinition(new Assembly[] { typeof(MyCalculateService).Assembly }, options);
            var server = new Grpc.Core.Server() {
                Services = { service },
                Ports = { new ServerPort("192.168.2.22", 5001, ServerCredentials.Insecure) },
            };
            server.Start();

            CreateWebHostBuilder(service, args).Build().Run();
        }

        /// <summary>
        /// 建立IWebHostBuilder
        /// </summary>
        /// <param name="service">服务定义</param>
        /// <param name="args">参数</param>
        /// <returns>IWebHostBuilder</returns>
        public static IWebHostBuilder CreateWebHostBuilder(MagicOnionServiceDefinition service, string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.Add(new ServiceDescriptor(typeof(MagicOnionServiceDefinition), service));
                })
                .UseStartup<Startup>();
    }
}
