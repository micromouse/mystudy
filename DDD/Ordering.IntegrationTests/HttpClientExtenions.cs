using Microsoft.AspNetCore.TestHost;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Ordering.IntegrationTests {
    /// <summary>
    /// HttpClient扩展
    /// </summary>
    public static class HttpClientExtenions {
        /// <summary>
        /// 建立幂等请求
        /// </summary>
        /// <param name="server">测试服务器</param>
        /// <returns>HttpClient</returns>
        public static HttpClient CreateIdempotentClient(this TestServer server) {
            var client = server.CreateClient();
            client.DefaultRequestHeaders.Add("x-requestid", Guid.NewGuid().ToString());
            return client;
        }
    }
}
