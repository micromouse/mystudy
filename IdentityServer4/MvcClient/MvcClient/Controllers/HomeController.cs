using DXQ.Study.IdentityServer4.MvcClient.Models;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DXQ.Study.IdentityServer4.MvcClient.Controllers {
    /// <summary>
    /// Home控制器
    /// </summary>
    public class HomeController : Controller {
        [HttpGet]
        public async Task<IActionResult> Check(string echo) {
            string bodyAsText = await new StreamReader(Request.Body).ReadToEndAsync();
            return Content(bodyAsText);
        }

        [HttpPost]
        public async Task<IActionResult> Check(int i) {
            return await Task.Factory.StartNew(() => Content($"ace{i}"));
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns>Represents an asynchronous operation.</returns>
        public async Task Logout() {
            //这里需要确保同时登出本地应用(MvcClient)的Cookies和OpenId Connect(去Identity Server清除单点登录的Session).
            await HttpContext.SignOutAsync("Cookies");
            await HttpContext.SignOutAsync("oidc");
        }

        /// <summary>
        /// 获得Identity
        /// </summary>
        /// <returns>Represents an asynchronous operation.</returns>
        [Authorize]
        public async Task<IActionResult> GetIdentity() {
            var token = await HttpContext.GetTokenAsync("access_token");

            using (var client = new HttpClient()) {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var content = await client.GetStringAsync("http://localhost:5001/api/identity");
                return Ok(new { value = content });
            }
        }

        /// <summary>
        /// 刷新Token
        /// </summary>
        /// <returns>Represents an asynchronous operation.</returns>
        public async Task RefreshTokens() {
            var authorizationServerInfo = await DiscoveryClient.GetAsync("http://localhost:5000/");
            var client = new TokenClient(authorizationServerInfo.TokenEndpoint, "mvc_code", "secret");
            var refreshToken = await HttpContext.GetTokenAsync("refresh_token");
            var response = await client.RequestRefreshTokenAsync(refreshToken);
            var identityToken = await HttpContext.GetTokenAsync("identity_token");
            var expiresAt = DateTime.UtcNow + TimeSpan.FromSeconds(response.ExpiresIn);
            var tokens = new[]
            {
                new AuthenticationToken{
                    Name = OpenIdConnectParameterNames.IdToken,
                    Value = identityToken
                },
                new AuthenticationToken
                {
                    Name = OpenIdConnectParameterNames.AccessToken,
                    Value = response.AccessToken
                },
                new AuthenticationToken
                {
                    Name = OpenIdConnectParameterNames.RefreshToken,
                    Value = response.RefreshToken
                },
                new AuthenticationToken
                {
                    Name = "expires_at",
                    Value = expiresAt.ToString("o", CultureInfo.InvariantCulture)
                }
            };
            var authenticationInfo = await HttpContext.AuthenticateAsync("Cookies");
            authenticationInfo.Properties.StoreTokens(tokens);
            await HttpContext.SignInAsync("Cookies", authenticationInfo.Principal, authenticationInfo.Properties);
        }

        /// <summary>
        /// Index视图
        /// </summary>
        /// <returns>Index视图</returns>
        public IActionResult Index() {
            return View();
        }

        /// <summary>
        /// About视图
        /// </summary>
        /// <returns>About视图</returns>
        [Authorize]
        public IActionResult About() {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        /// <summary>
        /// Contact视图
        /// </summary>
        /// <returns>Contact视图</returns>
        public IActionResult Contact() {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        /// <summary>
        /// 错误视图
        /// </summary>
        /// <returns>错误视图</returns>
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
