using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagicOnion.Service.Definition {
    /// <summary>
    /// 消息
    /// </summary>
    [MessagePackObject]
    public class Message {
        /// <summary>
        /// 消息文本
        /// </summary>
        [Key(0)]
        public string Text { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        [Key(1)]
        public string CreatedBy { get; set; }
    }
}
