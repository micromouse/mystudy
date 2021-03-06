﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace ConsulAndOcelot.ApiGetway {
    /// <summary>
    /// ConsulAndOcelot网关
    /// </summary>
    public class Program {
        private static string url = "http://0.0.0.0:8000";
        /// <summary>
        /// Main
        /// </summary>
        /// <param name="args">参数</param>
        public static void Main(string[] args) {
            CreateWebHostBuilder(args)
                .Build()
                .Run();
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
                .AddJsonFile("ocelot.json", optional: true, reloadOnChange: false)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();
            url = configuration["ApplicationUrl"] ?? url;

            return WebHost.CreateDefaultBuilder(args)
                .UseConfiguration(configuration)
                .UseKestrel()
                .UseUrls(url)
                .UseStartup<Startup>();
        }
    }
}
