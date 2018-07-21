using Autofac;
using Autofac.Extensions.DependencyInjection;
using IntegrationEvent;
using MassTransit;
using MassTransit.Util;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuickStart2.Application.IntegrationEventHandler;
using QuickStart2.Models;
using System;
using System.Threading.Tasks;

namespace QuickStart2 {
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
        public IServiceProvider ConfigureServices(IServiceCollection services) {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            var builder = new ContainerBuilder();
            builder.Register(c =>
            {
                return Bus.Factory.CreateUsingRabbitMq(sbc =>
                {
                    var host = sbc.Host(host: "localhost", virtualHost: "/", configure: h =>
                    {
                        h.Username("dxq");
                        h.Password("dxq");
                    });

                    sbc.ReceiveEndpoint(host, typeof(IWorldIntegrationEvent).Name, e => e.Consumer<WorldIntegrationEventConsumer>());
                    sbc.ReceiveEndpoint(host, "IHelloIntegrationEventC", e => e.Consumer<Hello3IntegrationEventConsumer>());
                    sbc.ReceiveEndpoint(host, typeof(MyMessageIntegrationEvent).Name, e =>
                    {
                        e.BindMessageExchanges = false;
                        e.Bind(typeof(MyMessageIntegrationEvent).Name);
                        e.Consumer(typeof(MyMessageIntegrationEvent), cx =>
                        {
                            return Task.CompletedTask;
                        });
                        e.Handler<MyMessageIntegrationEvent>((context) =>
                        {
                            Console.Write(context.Message.CreatedBy);
                            return Task.CompletedTask;
                        });
                    });
                });
            })
            .As<IBusControl>()
            .As<IPublishEndpoint>()
            .SingleInstance();
            builder.Populate(services);
            return new AutofacServiceProvider(builder.Build());
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
            } else {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();

            var bus = app.ApplicationServices.GetRequiredService<IBusControl>();
            var busHandle = TaskUtil.Await(() => bus.StartAsync());
            lifetime.ApplicationStopping.Register(busHandle.Stop);

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
