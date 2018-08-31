using System;

namespace EasyNetMQ.TextMessage {
    /// <summary>
    /// 我的文本消息
    /// </summary>
    public class MyTextMessage {
        /// <summary>
        /// 消息文本
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 建立时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
