using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace DXQ.Study.IdentityServer4.MvcClient {
    /// <summary>
    /// 启动
    /// </summary>
    public class Startup {
        public IConfiguration Configuration { get; }

        /// <summary>
        /// 初始化启动
        /// </summary>
        /// <param name="configuration">IConfiguration</param>
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        public void ConfigureServices(IServiceCollection services) {
            services.AddMvc();

            //我们关闭了JWT的Claim 类型映射, 以便允许well-known claims.这样做, 就保证它不会修改任何从Authorization Server返回的Claims.
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication(options =>
            {
                //这里我们使用Cookie作为验证用户的首选方式: DefaultScheme = "Cookies".
                //而把DefaultChanllangeScheme设为"oidc"是因为, 当用户需要登陆的时候, 将使用的是OpenId Connect Scheme.
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            //添加处理Cookie的处理器
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                 options.ExpireTimeSpan = TimeSpan.FromSeconds(60);
            })
            /*
            .AddOpenIdConnect(options =>
            {
                //AddOpenIdConnect是让上面的handler来执行OpenId Connect 协议.
                //Authority:是指信任的Identity Server(Authorization Server).
                //ClientId:
                //  是Client的识别标志,Client名字也暗示了我们要使用的是implicit flow, 这个flow主要应用于客户端应用程序, 这里的客户端应用程序主要是指javascript应用程序. 
                //  implicit flow是很简单的重定向flow, 它允许我们重定向到authorization server, 然后带着id token重定向回来, 这个 id token就是openid connect 用来识
                //  别用户是否已经登陆了.同时也可以获得access token. 很明显, 我们不希望access token出现在那个重定向中.一旦OpenId Connect协议完成, SignInScheme使用
                //  Cookie Handler来发布Cookie (中间件告诉我们已经重定向回到MvcClient了, 这时候有token了, 使用Cookie handler来处理).
                //SaveTokens为true表示要把从Authorization Server的Reponse中返回的token们持久化在cookie中.
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.Authority = "http://localhost:5000";
                options.RequireHttpsMetadata = false;
                options.ClientId = "mvc_implicit";
                options.SaveTokens = true;
                options.ResponseType = "id_token token";
            });
            */
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, (options) =>
             {
                 options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                 options.Authority = "http://localhost:5000";
                 options.RequireHttpsMetadata = false;
                 options.ClientId = "mvc_code";
                 options.ClientSecret = "secret";
                 options.ResponseType = "id_token code";
                 options.Scope.Add("socialnetwork");
                 options.Scope.Add("custom.profile");
                 options.Scope.Add("offline_access");
                 options.SaveTokens = true;
                 options.GetClaimsFromUserInfoEndpoint = true;
             });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">IApplicationBuilder</param>
        /// <param name="env">IHostingEnvironment</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            } else {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseAuthentication();

            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
