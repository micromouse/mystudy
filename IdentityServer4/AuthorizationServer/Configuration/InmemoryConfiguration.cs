using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DXQ.Study.IdentityServer4.AuthorizationServer.Configuration {
    /// <summary>
    /// 配置
    /// </summary>
    public class InmemoryConfiguration {
        /// <summary>
        /// 获得ApiResource集合
        /// </summary>
        /// <returns>ApiResource集合</returns>
        public static IEnumerable<ApiResource> ApiResources() {
            return new[] {
                new ApiResource("socialnetwork", "社交网络") {
                    UserClaims = new string[]{ "Username", "Status", "Registerdate", "Token", "Email", "Role" }
                },
                new ApiResource("lysalesplatform", "领英销售平台") {
                    UserClaims = new string[]{ "Username", "Status", "Registerdate", "Token", "Email", "Role" }
                }
            };
        }

        /// <summary>
        /// 获得IdentityResource集合
        /// </summary>
        /// <returns>IdentityResource集合</returns>
        public static IEnumerable<IdentityResource> GetIdentityResources() {
            return new List<IdentityResource> {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource(
                   name: "custom.profile",
                   displayName: "custom.profile",
                   claimTypes: new[] { "Email", "Token" })
            };
        }

        /// <summary>
        /// 获得Client集合
        /// </summary>
        /// <returns>Client集合</returns>
        public static IEnumerable<Client> Clients() {
            return new[] {
                //ClientSecrets:Client用来获取Token用的
                //AllowedGrantType:通过用户名密码和ClientCredentials来换取token的方式,ClientCredentials允许Client只使用ClientSecrets来获取token. 这比较适合那种没有用户参与的api动作.
                //AllowedScopes:Client允许请求的范围,这里只用socialnetwork
                new Client {
                    ClientId = "socialnetwork",
                    ClientSecrets = new[]{ new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    AllowedScopes = new[]{ "socialnetwork" }
                },
                new Client {
                    ClientId = "mvc_implicit",
                    ClientName = "MVC Client",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    RedirectUris = { "http://localhost:5002/signin-oidc" },
                    PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc" },
                    AllowedScopes = new List<string> {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "socialnetwork"
                    },
                    AllowAccessTokensViaBrowser = true
                },
                new Client {
                    ClientId = "mvc_code",
                    ClientName = "MVC Code Client",
                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    RedirectUris = { "http://localhost:5002/signin-oidc" },
                    PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc" },
                    AllowedScopes = new List<string> {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "socialnetwork",
                        "custom.profile"
                    },
                    AllowOfflineAccess = true,
                    AllowAccessTokensViaBrowser = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                },
                new Client {
                    ClientId = "javascript",
                    ClientName = "JavaScript Client",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    AccessTokenLifetime = 60,
                    IdentityTokenLifetime = 60,
                    AllowOfflineAccess = true,
                    RedirectUris = { "http://localhost:5003/callback.html" },
                    PostLogoutRedirectUris = { "http://localhost:5003/index.html" },
                    AllowedCorsOrigins = { "http://localhost:5003" },
                    AllowedScopes = {
                        //最后还需要指定OpenId Connect使用的Scopes, 之前我们指定的socialnetwork是一个ApiResource. 
                        //而这里我们需要添加的是让我们能使用OpenId Connect的Scopes, 这里就要使用Identity Resources. 
                        //Identity Server带了几个常量可以用来指定OpenId Connect预包装的Scopes. 上面的AllowedScopes设定的就是我们要用的scopes, 
                        //他们包括 openid Connect和用户的profile, 同时也包括我们之前写的api resource: "socialnetwork". 要注意区分, 
                        //这里有Api resources, 还有openId connect scopes(用来限定client可以访问哪些信息), 而为了使用这些openid connect scopes, 
                        //我们需要设置这些identity resoruces, 这和设置ApiResources差不多:
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "socialnetwork",
                        "lysalesplatform"
                    }
                }
            };
        }

        /*
        /// <summary>
        /// 获得TestUser集合
        /// </summary>
        /// <returns>TestUser集合</returns>
        public static IEnumerable<TestUser> Users() {
            //这里的内存用户的类型是TestUser, 只适合学习和测试使用, 实际生产环境中还是需要使用数据库来存储用户信息的, 例如asp.net core identity
            return new[] {
                new TestUser {
                    SubjectId = "1",
                    Username = "mail@qq.com",
                    Password = "password",
                    Claims = new[]{ new Claim("email", "mail@qq.com") }
                }
            };
        }
        */
    }
}
