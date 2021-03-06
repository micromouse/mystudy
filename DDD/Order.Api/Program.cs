﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Ordering.Infrastructure;
using Ordering.Api.Infrastructure;

namespace Ordering.Api {
    /// <summary>
    /// Ordering.API
    /// </summary>
    public class Program {
        /// <summary>
        /// 入口
        /// </summary>
        /// <param name="args">参数</param>
        public static void Main(string[] args) {
            CreateWebHostBuilder(args)
                .Build()
                .MigrateDbContext<OrderingDbContext>()
                .Run();
        }

        /// <summary>
        /// 建立WebHostBuilder
        /// </summary>
        /// <param name="args">参数</param>
        /// <returns>WebHostBuilder</returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) {
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
        }
    }
}
