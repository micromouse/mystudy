using IntegrationEvent;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using QuickStart.Applications;
using QuickStart.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace QuickStart.Controllers {
    /// <summary>
    /// Home控制器
    /// </summary>
    public class HomeController : Controller {
        private readonly IPublishEndpoint publish;
        private readonly ISendEndpointProvider send;

        /// <summary>
        /// 初始化Home控制器
        /// </summary>
        /// <param name="publish">消息发布</param>
        public HomeController(IPublishEndpoint publish, ISendEndpointProvider send) {
            this.publish = publish;
            this.send = send;
        }

        /// <summary>
        /// Index视图
        /// </summary>
        /// <returns>Index视图</returns>
        public IActionResult Index() {
            var endpoint = send.GetSendEndpoint(new Uri($"rabbitmq://localhost/{typeof(MyMessageIntegrationEvent).Name}")).Result;
            endpoint.Send(new MyMessageIntegrationEvent("dxq", DateTime.Now));
            return View();
        }

        /// <summary>
        /// 关于视图
        /// </summary>
        /// <returns>关于视图</returns>
        public async Task<IActionResult> About() {
            ViewData["Message"] = "Your application description page.";

            await publish.Publish<IHelloIntegrationEvent>(new { Text = "this is my message" });            
            return View();
        }

        public async Task<IActionResult> Contact() {
            ViewData["Message"] = "Your contact page.";
            var endpoint = await send.GetSendEndpoint(new Uri($"rabbitmq://localhost/{typeof(IWorldIntegrationEvent).Name}"));
            await endpoint.Send<IWorldIntegrationEvent>(new { Createby = "my name is dxq" });

            return View();
        }

        public IActionResult Privacy() {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
