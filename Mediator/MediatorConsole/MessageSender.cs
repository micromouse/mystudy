using DXQ.Study.Mediator.MediatorConsole.Application.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DXQ.Study.Mediator.MediatorConsole {
    /// <summary>
    /// 发送消息
    /// </summary>
    public class MessageSender {
        private IMediator mediator = null;

        /// <summary>
        /// 初始化消息发送
        /// </summary>
        /// <param name="mediator">MediatR</param>
        public MessageSender(IMediator mediator) {
            this.mediator = mediator;
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message">消息</param>
        public void Send(string message) {
            mediator.Send(new IdentifiedCommand<HelloCommand, bool>(new HelloCommand(message), Guid.NewGuid()));
        }
    }
}
