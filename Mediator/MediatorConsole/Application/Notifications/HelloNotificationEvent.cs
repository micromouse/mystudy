using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DXQ.Study.Mediator.MediatorConsole.Application.Notifications {
    /// <summary>
    /// 打招呼事件
    /// </summary>
    public class HelloNotificationEvent : INotification {
        /// <summary>
        /// 打招呼时间
        /// </summary>
        public DateTime HelloTime { get; private set; }

        /// <summary>
        /// 打招呼内容
        /// </summary>
        public string HelloContent { get; private set; }

        /// <summary>
        /// 初始化打招呼事件
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="helloTime">招呼时间</param>
        public HelloNotificationEvent(string content, DateTime helloTime) {
            this.HelloContent = content;
            this.HelloTime = helloTime;
        }
    }
}
