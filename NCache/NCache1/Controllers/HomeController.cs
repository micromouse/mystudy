using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using NCache1.Models;

namespace NCache1.Controllers {
    /// <summary>
    /// Home控制器
    /// </summary>
    public class HomeController : Controller {
        private readonly IDistributedCache cache;

        /// <summary>
        /// 初始化Home控制器
        /// </summary>
        /// <param name="cache">分布式缓存</param>
        public HomeController(IDistributedCache cache) {
            this.cache = cache;
        }

        /// <summary>
        /// Index视图
        /// </summary>
        /// <returns>Index视图</returns>
        public IActionResult Index() {
            return View();
        }

        /// <summary>
        /// 关于视图
        /// </summary>
        /// <returns>关于视图</returns>
        public IActionResult About() {
            ViewData["Message"] = $"this rabbitmqhost is:{cache.GetString("rabbitmqhost")}";

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
