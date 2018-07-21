using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace QuickStart2 {
    public class Program {
        /// <summary>
        /// Main
        /// </summary>
        /// <param name="args">参数</param>
        public static void Main(string[] args) {
            CreateWebHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// IWebHostBuilder
        /// </summary>
        /// <param name="args">参数</param>
        /// <returns>IWebHostBuilder</returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
