using Microsoft.AspNetCore.Mvc;

namespace Ordering.Api.Controllers {
    /// <summary>
    /// Home控制器
    /// </summary>
    public class HomeController : Controller {
        /// <summary>
        /// Index
        /// </summary>
        /// <returns>视图</returns>
        public IActionResult Index() {
            return new RedirectResult("~/swagger");
        }
    }
}