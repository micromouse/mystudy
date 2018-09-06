using Microsoft.Extensions.Configuration;
using Ordering.Api;

namespace Ordering.IntegrationTests {
    /// <summary>
    /// 订单测试启动
    /// </summary>
    public class OrderingTestsStartup : Startup {
        /// <summary>
        /// 初始化订单测试启动
        /// </summary>
        /// <param name="configuration">配置</param>
        public OrderingTestsStartup(IConfiguration configuration) : base(configuration) {
        }
    }
}
