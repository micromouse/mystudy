using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ordering.Api {
    /// <summary>
    /// Ordering.Api启动
    /// </summary>
    public class Startup {
        public IConfiguration Configuration { get; }

        /// <summary>
        /// 初始化Ordering.Api启动
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

            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info {
                    Title = "Ordering HTTP API",
                    Version = "v1",
                    Description = "The Ordering Service HTTP API",
                    TermsOfService = "Terms Of Service"
                });
            });
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

            app.UseMvc(route => route.MapRoute(name: "default", template: "{controller=Home}/{action=Index}/{id?}"));
            app.UseSwagger()
               .UseSwaggerUI(c =>
               {
                   c.SwaggerEndpoint("http://localhost:5000/swagger/v1/swagger.json", "Ordering.API V1");
               });
        }
    }
}
