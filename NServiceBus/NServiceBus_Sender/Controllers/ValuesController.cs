using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NServiceBus;
using NServiceBus_Sender.IntegrationEvents;

namespace NServiceBus_Sender.Controllers {
    /// <summary>
    /// 值控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase {
        private readonly IMessageSession messageSession;

        /// <summary>
        /// 初始化值控制器
        /// </summary>
        /// <param name="messageSession">IMessageSession</param>
        public ValuesController(IMessageSession messageSession) {
            this.messageSession = messageSession;
        }

        /// <summary>
        /// 获得值集合
        /// </summary>
        /// <returns>值集合</returns>
        [HttpGet]
        public async Task<ActionResult<string>> Get() {
            await messageSession.Send(new MyMessage { Text = "this is a message" }).ConfigureAwait(false);
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
