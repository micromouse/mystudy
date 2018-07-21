using MassTransit;
using Microsoft.AspNetCore.Mvc;
using QuickStart2.Models;
using System.Diagnostics;

namespace QuickStart2.Controllers {
    /// <summary>
    /// Home控制器
    /// </summary>
    public class HomeController : Controller {
        private readonly IPublishEndpoint publish;

        /// <summary>
        /// 初始化Home控制器
        /// </summary>
        /// <param name="publish"></param>
        public HomeController(IPublishEndpoint publish) {
            this.publish = publish;
        }

        public IActionResult Index() {
            return View();
        }

        public IActionResult About() {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact() {
            ViewData["Message"] = "Your contact page.";

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
