using Grpc.Core;
using MagicOnion.HttpGateway.Swagger;
using MagicOnion.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace MagicOnion.Service.Server {
    /// <summary>
    /// ∆Ù∂Ø
    /// </summary>
    public class Startup {
        public IConfiguration Configuration { get; }

        /// <summary>
        /// ≥ı ºªØ∆Ù∂Ø
        /// </summary>
        /// <param name="configuration">≈‰÷√</param>
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        /// <summary>
        /// ≈‰÷√∑˛ŒÒ
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        public void ConfigureServices(IServiceCollection services) {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        /// <summary>
        /// ≈‰÷√≈‰÷√
        /// </summary>
        /// <param name="app">IApplicationBuilder</param>
        /// <param name="env">IHostingEnvironment</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            } else {
                app.UseExceptionHandler("/Error");
            }

            var magicOnion = app.ApplicationServices.GetService<MagicOnionServiceDefinition>();
            var file = Path.Combine(Directory.GetCurrentDirectory(), "Config/MagicOnion.Service.Implemention.xml");
            var swaggerOptions = new SwaggerOptions("MagicOnion.Server", "Swagger Integration Test", "/") {
                XmlDocumentPath = file
            };
            app.UseMagicOnionSwagger(magicOnion.MethodHandlers, swaggerOptions);
            app.UseMagicOnionHttpGateway(magicOnion.MethodHandlers, new Channel("192.168.2.22:5001", ChannelCredentials.Insecure));

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc();
        }
    }
}
