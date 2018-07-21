using System;
using System.Collections.Generic;
using System.IdentityModel.Configuration;
using System.IdentityModel.Tokens;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace DXQ.Study.IdentityServer4.WcfService {
    /// <summary>
    /// 应用程序入口
    /// </summary>
    class Program {
        /// <summary>
        /// Main
        /// </summary>
        /// <param name="args">参数</param>
        static void Main(string[] args) {
            var url = "http://192.168.2.22:5004/Service";
            var host = new ServiceHost(typeof(Service), new Uri(url));

            //配置凭据
            /*
            host.Credentials.IdentityConfiguration = CreateIdentityConfiguration();
            host.Credentials.UseIdentityConfiguration = true;
            host.Description.Behaviors.Find<ServiceAuthorizationBehavior>().PrincipalPermissionMode = PrincipalPermissionMode.Always;
            */
            host.AddServiceEndpoint(typeof(IService), CreateBinding(), url);
            
            host.Open();
            Console.WriteLine("server running...");
            Console.ReadLine();
            host.Close();
        }

        /// <summary>
        /// 建立Identity配置
        /// </summary>
        /// <returns>Identity配置</returns>
        private static IdentityConfiguration CreateIdentityConfiguration() {
            var identityConfiguration = new IdentityConfiguration();

            identityConfiguration.SecurityTokenHandlers.Clear();
            identityConfiguration.SecurityTokenHandlers.Add(new IdentityServerWrappedJwtHandler("http://localhost:5000", "socialnetwork"));
            identityConfiguration.ClaimsAuthorizationManager = new RequireAuthenticationAuthorizationManager();

            return identityConfiguration;
        }

        /// <summary>
        /// 建立绑定
        /// </summary>
        /// <returns>绑定</returns>
        private static Binding CreateBinding() {
            /*
            var binding = new WS2007FederationHttpBinding(WSFederationHttpSecurityMode.TransportWithMessageCredential);

            binding.HostNameComparisonMode = HostNameComparisonMode.Exact;
            binding.Security.Message.EstablishSecurityContext = false;
            binding.Security.Message.IssuedKeyType = SecurityKeyType.BearerKey;

            return binding;
            */
            return new WebHttpBinding(WebHttpSecurityMode.None);
        }

    }
}
