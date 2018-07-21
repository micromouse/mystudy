using System.Security.Claims;

namespace DXQ.Study.IdentityServer4.WcfService {
    /// <summary>
    /// 
    /// </summary>
    internal class RequireAuthenticationAuthorizationManager : ClaimsAuthorizationManager {
        /// <summary>
        /// 认证访问
        /// </summary>
        /// <param name="context">认证上下文</param>
        /// <returns>是否已认证</returns>
        public override bool CheckAccess(AuthorizationContext context) {
            return !context.Principal.Identity.IsAuthenticated;            
        }
    }
}