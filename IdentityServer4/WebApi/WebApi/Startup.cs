using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace DXQ.Study.IdentityServer4.WebApi {
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
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        public void ConfigureServices(IServiceCollection services) {
            services.AddMvc();
            services.AddMvcCore()
                .AddAuthorization()
                .AddJsonFormatters();

            //这里AddAuthentication()是把验证服务注册到DI, 并配置了Bearer作为默认模式.
            //AddIdentityServerAuthentication()是在DI注册了token验证的处理者.
            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    //由于是本地运行, 所以就不使用https了, RequireHttpsMetadata = false.如果是生产环境, 一定要使用https.
                    //Authority指定Authorization Server的地址.
                    //ApiName要和Authorization Server里面配置ApiResource的name一样.
                    options.RequireHttpsMetadata = false;
                    options.Authority = "http://localhost:5000";
                    options.ApiName = "lysalesplatform";
                });

            //Adds cross-origin resource sharing services to the specified Microsoft.Extensions.DependencyInjection.IServiceCollection.
            services.AddCors(options =>
            {
                options.AddPolicy("default", policy =>
                {
                    policy.WithOrigins("http://localhost:5003")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            //Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My Api", Version = "v1" });
            });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">IApplicationBuilder</param>
        /// <param name="env">IHostingEnvironment</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            //Adds a CORS middleware to your web application pipeline to allow cross domain requests.
            app.UseCors("default");

            //Enable middleware to serve generated Swagger as a JSON endpoint.
            //Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My Api V1");
            });

            //把验证中间件添加到管道里, 这样每次请求就会调用验证服务了. 一定要在UserMvc()之前调用.
            //当在controller或者Action使用[Authorize]属性的时候, 这个中间件就会基于传递给api的Token来验证Authorization, 如果没有token或者token不正确, 这个中间件就会告诉我们这个请求是UnAuthorized(未授权的).
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
