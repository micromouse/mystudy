using DXQ.Study.EventbusBlocks.EventBus.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace DXQ.Study.EventbusBlocks.EventBusPublishTest {
    /// <summary>
    /// Hello集成事件
    /// </summary>
    public class HelloIntegrationEvent : IntegrationEvent {
        /// <summary>
        /// 初始化Hello集成事件
        /// </summary>
        /// <param name="messageTitle"></param>
        /// <param name="messageContent"></param>
        public HelloIntegrationEvent(string messageTitle,string messageContent) {
            this.MessageTitle = messageTitle;
            this.MessageContent = messageContent;
        }

        /// <summary>
        /// 消息标题
        /// </summary>
        public string MessageTitle { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string MessageContent { get; set; }
    }
}
