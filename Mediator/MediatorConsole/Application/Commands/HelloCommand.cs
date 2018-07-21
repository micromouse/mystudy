using MediatR;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace DXQ.Study.Mediator.MediatorConsole.Application.Commands {
    /// <summary>
    /// 打招呼命令
    /// </summary>
    public class HelloCommand : IRequest<bool> {
        /// <summary>
        /// 内容
        /// </summary>
        [DataMember]
        public string Content { get; private set; }

        /// <summary>
        /// 初始化打招呼命令
        /// </summary>
        /// <param name="content">内容</param>
        public HelloCommand(string content) {
            this.Content = content;
        }
    }
}
