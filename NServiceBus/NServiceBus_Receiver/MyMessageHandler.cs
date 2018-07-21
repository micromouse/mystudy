using NServiceBus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NServiceBus_Receiver {
    /// <summary>
    /// 我的消息处理器
    /// </summary>
    public class MyMessageHandler : IHandleMessages<MyMessage> {
        /// <summary>
        /// 处理消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="context">上下文</param>
        /// <returns>任务</returns>
        public Task Handle(MyMessage message, IMessageHandlerContext context) {
            Console.WriteLine(message.Text);
            return Task.CompletedTask;
        }
    }
}
