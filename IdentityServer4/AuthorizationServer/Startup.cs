using DXQ.Study.IdentityServer4.AuthorizationServer.Certificates;
using DXQ.Study.IdentityServer4.AuthorizationServer.Quickstart.Account;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace DXQ.Study.IdentityServer4.AuthorizationServer {
    /// <summary>
    /// 启动
    /// </summary>
    public class Startup {
        private IConfiguration Configuration { get; }

        /// <summary>
        /// 初始化启动
        /// </summary>
        /// <param name="configuration">配置</param>
        public Startup(IConfiguration configuration) {
            this.Configuration = configuration;
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        /// </summary>
        /// <param name="services">配置服务</param>
        public void ConfigureServices(IServiceCollection services) {
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            var userconnectionString = Configuration.GetConnectionString("UserinfoConnection");
            var assembly = typeof(Startup).GetTypeInfo().Assembly;

            //注入应用程序DbContext
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(userconnectionString));

            //这里不仅要把IdentityServer注册到容器中, 还要至少对其配置三点内容:
            //1.哪些API可以使用这个authorization server.
            //2.那些客户端Client(应用)可以使用这个authorization server.
            //3.指定可以使用authorization server授权的用户.
            services.AddIdentityServer(config =>
            {
                config.Authentication.CookieLifetime = TimeSpan.FromSeconds(60 * 5);
                config.Authentication.CookieSlidingExpiration = true;
            })
            //.AddDeveloperSigningCredential()                                              //添加开发者凭据
            //.AddInMemoryClients(InmemoryConfiguration.Clients())                          //添加Clients
            //.AddInMemoryApiResources(InmemoryConfiguration.ApiResources())                //添加ApiResource
            //.AddInMemoryIdentityResources(InmemoryConfiguration.GetIdentityResources())   //添加IdentityResource
            //.AddTestUsers(InmemoryConfiguration.Users().ToList())                         //添加测试用户
            .AddSigningCredential(Certificate.Get())                                        //添加证书凭据
            .AddConfigurationStore(options =>
            {
                //配置配置存储(clients, resources)
                options.ConfigureDbContext = (builder) => builder.UseSqlServer(connectionString, (sql) => sql.MigrationsAssembly(assembly.GetName().Name));
            })
            .AddOperationalStore(options =>
            {
                //配置操作存储(tokens, codes, consents)
                options.ConfigureDbContext = (builder) => builder.UseSqlServer(connectionString, (sql) => sql.MigrationsAssembly(assembly.GetName().Name));
                //options.EnableTokenCleanup = true;
                //options.TokenCleanupInterval = 30;
            })
            .AddProfileService<ProfileService>()
            .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>();

            services.AddTransient<IUserRepository, UserRepository>();

            services.AddMvc();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">IApplicationBuilder</param>
        /// <param name="env">IHostingEnviroment</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseIdentityServer();
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }
}
