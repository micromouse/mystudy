using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace JavaScriptClient {
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
        /// 构建IWebHost
        /// </summary>
        /// <param name="args">参数</param>
        /// <returns></returns>
        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
