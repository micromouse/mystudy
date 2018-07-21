using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;

namespace DXQ.Study.IdentityServer4.WcfService {
    /// <summary>
    /// Represents a security token handler that creates security tokens from SAML 2.0 Assertions
    /// 表示一个安全token处理器,它从SAML2.0断言中创建安全Token
    /// </summary>
    internal class IdentityServerWrappedJwtHandler : Saml2SecurityTokenHandler {
        private string issuerName = null;
        private readonly string[] requiredScopes = null;
        private X509Certificate2 signinCert = null;

        /// <summary>
        /// 初始化安全Token处理器
        /// </summary>
        /// <param name="authority">认证URL</param>
        /// <param name="requiredScopes">范围</param>
        public IdentityServerWrappedJwtHandler(string authority, params string[] requiredScopes) {
            var discoveryEndpoint = $"{authority}/.well-known/openid-configuration";
            var configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(discoveryEndpoint);
            var config = configurationManager.GetConfigurationAsync().Result;

            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();
            this.signinCert = new X509Certificate2(Convert.FromBase64String(config.JsonWebKeySet.Keys.First().X5c.First()));
            this.issuerName = this.signinCert.Issuer;
            this.requiredScopes = requiredScopes;
        }

        /// <summary>
        /// 验证Token
        /// </summary>
        /// <param name="token">Token</param>
        /// <returns>Identity令牌集合</returns>
        public override ReadOnlyCollection<ClaimsIdentity> ValidateToken(SecurityToken token) {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            //配置验证
            var samlattributeStatement = ((Saml2SecurityToken)token).Assertion.Statements.OfType<Saml2AttributeStatement>().FirstOrDefault();
            var jwt = samlattributeStatement.Attributes.Where(x => x.Name.Equals("jwt", StringComparison.OrdinalIgnoreCase)).SingleOrDefault().Values.Single();
            var parameters = new TokenValidationParameters {
                ValidAudience = $"{this.issuerName}/resources",
                ValidIssuer = this.issuerName,
                IssuerSigningToken = new X509SecurityToken(this.signinCert)
            };

            //验证
            var prinpical = handler.ValidateToken(jwt, parameters, out SecurityToken validatedToken);
            if (this.requiredScopes.Any()) {
                if (!this.requiredScopes.Any(scope => prinpical.HasClaim("scope", scope))) throw new SecurityTokenValidationException();
            }
            
            return new ReadOnlyCollection<ClaimsIdentity>(new List<ClaimsIdentity> { prinpical.Identities.First() });


        }
    }
}