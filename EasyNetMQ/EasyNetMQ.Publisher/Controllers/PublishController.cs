using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using EasyNetMQ.TextMessage;
using EasyNetQ;
using EasyNetQ.Topology;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EasyNetMQ.Publisher.Controllers {
    /// <summary>
    /// 发布控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PublishController : ControllerBase {
        private readonly IBus bus;

        /// <summary>
        /// 初始化发布控制器
        /// </summary>
        /// <param name="bus"></param>
        public PublishController(IBus bus) {
            this.bus = bus;
        }

        /// <summary>
        /// 简单消息发布
        /// </summary>
        /// <param name="message">消息</param>
        /// <returns>任务</returns>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> SimpleMessage(MyTextMessage message) {
            await bus.PublishAsync(message);
            return Ok("已发布");
        }

        /// <summary>
        /// Topic消息发布
        /// </summary>
        /// <param name="message">消息</param>
        /// <returns>任务</returns>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> TopicMessage(MyTextMessage message) {
            await bus.PublishAsync(JsonConvert.SerializeObject(message), typeof(MyTextMessage).FullName);
            return Ok("已发布");
        }

        /// <summary>
        /// 高级消息发布
        /// </summary>
        /// <param name="message">消息</param>
        /// <returns>任务</returns>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> AdvanceMessage(MyTextMessage message) {
            var myMessage = new Message<string>(JsonConvert.SerializeObject(message));
            var exchange = new Exchange("EasyNetMQ.Exchange");

            await bus.Advanced.PublishAsync(exchange, routingKey: typeof(MyTextMessage).Name, mandatory: true, message: myMessage).ContinueWith(task =>
            {
                if (task.IsCompleted) {
                    Console.WriteLine($"任务[{message.Text}]完成");
                }

                if (task.IsFaulted) {
                    Console.WriteLine($"任务[{message.Text}]失败:{task.Exception.ToString()}");
                }

            });
            return Ok("已发布");
        }
    }
}
