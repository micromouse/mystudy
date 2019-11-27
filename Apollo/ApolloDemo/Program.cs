using Com.Ctrip.Framework.Apollo;
using Com.Ctrip.Framework.Apollo.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace ApolloDemo {
    public class Program {
        public static void Main(string[] args) {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .ConfigureAppConfiguration(builder =>
                        {
                            var apolloBuilder = builder
                                .AddApollo(builder.Build().GetSection("apollo"))
                                .AddDefault(ConfigFileFormat.Json)
                                .AddDefault(ConfigFileFormat.Xml)
                                .AddDefault();
                            var nameSpaces = builder.Build()["apollo:NameSpaces"]?.Split(',');
                            if (nameSpaces?.Length > 0) {
                                foreach (var item in nameSpaces) {
                                    if (!string.IsNullOrEmpty(item)) {
                                        apolloBuilder.AddNamespace(item);
                                    }
                                }
                            }
                        })
                        .UseStartup<Startup>();
                });
    }
}
