using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Rebus.Bus;
using ReBus.Sender.Handlers;

namespace ReBus.Sender.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase {
        private readonly IBus bus;

        /// <summary>
        /// 初始化Values控制器
        /// </summary>
        /// <param name="bus"></param>
        public ValuesController(IBus bus) {
            this.bus = bus;
        }

        // GET api/values
        [HttpGet]
        public async Task<ActionResult<string>> Get() {
            await bus.Send(new Message1 { Text = "my name is dxq" });
            return "message is sent";
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id) {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value) {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value) {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id) {
        }
    }
}
