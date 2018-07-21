using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NServiceBus;

namespace NServiceBus_Sender {
    /// <summary>
    /// 启动
    /// </summary>
    public class Startup {
        private IEndpointInstance endpoint;

        /// <summary>
        /// 配置
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// 初始化启动
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            var endpointConfiguration = new EndpointConfiguration("NServiceBus.Sender");
            var transport = endpointConfiguration.UseTransport<LearningTransport>();
            endpointConfiguration.SendOnly();

            transport.Routing().RouteToEndpoint(assembly: typeof(Startup).Assembly, destination: "NServiceBus.Receiver");
            endpoint = Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();
            services.AddSingleton<IMessageSession>(endpoint);
        }

        /// <summary>
        /// 配置配置
        /// </summary>
        /// <param name="app">IApplicationBuilder</param>
        /// <param name="env">IHostingEnvironment</param>
        /// <param name="lifetime">IApplicationLifetime</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime lifetime) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            lifetime.ApplicationStopping.Register(async () => await endpoint?.Stop());
            app.UseMvc();
        }
    }
}
