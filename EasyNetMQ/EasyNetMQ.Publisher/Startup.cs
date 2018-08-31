using EasyNetQ;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EasyNetMQ.Publisher {
    /// <summary>
    /// 启动
    /// </summary>
    public class Startup {
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
            services.AddSingleton(RabbitHutch.CreateBus("host=localhost;username=dxq;password=dxq;persistentMessages=true"));
        }

        /// <summary>
        /// 配置配置
        /// </summary>
        /// <param name="app">IApplicationBuilder</param>
        /// <param name="env">IHostingEnvironment</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
