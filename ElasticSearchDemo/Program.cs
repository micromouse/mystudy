using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Serilog.Formatting.Elasticsearch;
using Serilog.Sinks.Elasticsearch;
using System;
using System.Collections.Generic;

namespace ElasticSearchDemo
{
    /// <summary>
    /// ElasticSearch日志例子
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main
        /// </summary>
        /// <param name="args">参数</param>
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// 建立IWebHostBuilder
        /// </summary>
        /// <param name="args">参数</param>
        /// <returns>IWebHostBuilder</returns>
        static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
           WebHost.CreateDefaultBuilder(args)
               .UseSerilog((context, configuration) =>
               {
                   configuration
                       .ReadFrom.Configuration(context.Configuration)
                       .WriteTo.ColoredConsole()
                       .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
                       {
                           IndexDecider = (@event, offset) => $"innerlylog-{offset.DateTime.ToString("yyyy.MM.dd")}",
                           TypeName = "log",
                           OverwriteTemplate = true,
                           AutoRegisterTemplate = true,
                           AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv6,
                           CustomFormatter = new MyElasticsearchJsonFormatter(rootFields: new string[] { "Application", "ExecuteTicks" })
                       });
               })
               .UseStartup<Startup>();
    }
}
