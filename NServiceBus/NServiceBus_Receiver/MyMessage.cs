using NServiceBus;
using System;
using System.Collections.Generic;
using System.Text;

namespace NServiceBus_Receiver {
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
