using System;
using System.Security.Claims;
using System.ServiceModel;
using System.Text;

namespace DXQ.Study.IdentityServer4.WcfService {
    /// <summary>
    /// 服务接口
    /// </summary>
    [ServiceContract(Name = "Service")]
    public interface IService {
        /// <summary>
        /// Ping
        /// </summary>
        /// <returns>结果</returns>
        [OperationContract]
        string Ping();
    }

    /// <summary>
    /// 服务契约实现
    /// </summary>
    class Service : IService {
        /// <summary>
        /// Ping
        /// </summary>
        /// <returns>结果</returns>
        public string Ping() {
            var builder = new StringBuilder(DateTime.Now.ToString());
            /*
            foreach (var claim in ClaimsPrincipal.Current.Claims) {
                builder.AppendFormat("{0}::{1}\n", claim.Type, claim.Value);
            }
            */
            return builder.ToString();
        }
    }
}
