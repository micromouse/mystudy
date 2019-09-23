using Consul;
using DnsClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using System.Net;

namespace ConsulAndOcelot.ApiGetway {
    /// <summary>
    /// 启动ConsuleAndOcelot网关
    /// </summary>
    public class Startup {
        public IConfiguration Configuration { get; }

        /// <summary>
        /// 初始化启动ConsuleAndOcelot网关
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
            services.AddOcelot()
                .AddConsul()
                .AddConfigStoredInConsul();
            services.AddSingleton<IConsulClient>(new ConsulClient(config => config.Address = new System.Uri("http://192.168.2.136:8500")))
                .AddSingleton<IDnsQuery>(new LookupClient(IPAddress.Parse("192.168.2.136"), 8600));
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
            app.ApplicationServices
                .GetService<IConsulClient>()
                .Agent.ServiceRegister(new AgentServiceRegistration {
                    ID = "ConsulAndOcelot.ApiGetway",
                    Name = "ConsulAndOcelot.ApiGetway",
                    Address = "127.0.0.1",
                    Port = 8000
                })
                .Wait();

            app.UseOcelot()
                .Wait();
        }
    }
}
