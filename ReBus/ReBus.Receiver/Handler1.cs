using Rebus.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReBus.Receiver {
    /// <summary>
    /// 消息处理
    /// </summary>
    public class Handler1 : IHandleMessages<Message1> {
        /// <summary>
        /// 处理
        /// </summary>
        /// <param name="message">消息</param>
        /// <returns>任务</returns>
        public Task Handle(Message1 message) {
            Console.WriteLine(message.Text);
            return Task.CompletedTask;
        }
    }
}
