using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DXQ.Study.IdentityServer4.WebApi {
    /// <summary>
    /// 应用程序入口
    /// </summary>
    public class Program {
        /// <summary>
        /// Main
        /// </summary>
        /// <param name="args">参数</param>
        public static void Main(string[] args) {
            BuildWebHost(args).Run();
        }

        /// <summary>
        /// 创建IWebHost
        /// </summary>
        /// <param name="args">参数</param>
        /// <returns></returns>
        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
