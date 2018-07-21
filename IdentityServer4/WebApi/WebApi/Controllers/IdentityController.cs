using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DXQ.Study.IdentityServer4.WebApi.Controllers {
    /// <summary>
    /// Identity控制器
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    public class IdentityController : Controller {
        /// <summary>
        /// 获得email
        /// </summary>
        /// <returns>email</returns>
        [HttpGet]
        public IActionResult Get() {
            var username = User.Claims.First(x => x.Type == "Username").Value;
            return Ok(username + DateTime.Now.ToString());
        }
    }
}
