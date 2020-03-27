using Microsoft.AspNetCore.Mvc;

namespace Ocelot.Api1.Controllers {
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase {
        public IActionResult Index() {
            return Ok("i need order");
        }
    }
}