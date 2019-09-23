using Consul;
using DnsClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;

namespace ConsulAndOcelot.ServiceA {
    /// <summary>
    /// 启动ConsulAndOcelot服务A
    /// </summary>
    public class Startup {
        public IConfiguration Configuration { get; }

        /// <summary>
        /// 初始化启动ConsulAndOcelot服务A
        /// </summary>
        /// <param name="configuration">配置</param>
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        /// <summary>
        /// 配置服务
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        public void ConfigureServices(IServiceCollection services) {
            services.AddSingleton<IConsulClient>(new ConsulClient(config => config.Address = new System.Uri("http://192.168.2.136:8500")))
                .AddSingleton<IDnsQuery>(new LookupClient(IPAddress.Parse("192.168.2.136"), 8600));

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="app">IApplicationBuilder</param>
        /// <param name="env">IHostingEnvironment</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            var uri = new Uri(Configuration["ApplicationUrl"] ?? "http://0.0.0.0:8100");
            app.ApplicationServices
                .GetService<IConsulClient>()
                .Agent.ServiceRegister(new AgentServiceRegistration {
                    ID = $"ConsulAndOcelot.ServiceA:{uri.Port}",
                    Name = "ConsulAndOcelot.ServiceA",
                    Address = "192.168.2.22",
                    Port = uri.Port
                })
                .Wait();

            app.UseMvc();
        }
    }
}
