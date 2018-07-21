using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NServiceBus_Sender.IntegrationEvents {
    /// <summary>
    /// 我的消息
    /// </summary>
    public class MyMessage : IMessage {
        /// <summary>
        /// 消息文本
        /// </summary>
        public string Text { get; set; }
    }
}
