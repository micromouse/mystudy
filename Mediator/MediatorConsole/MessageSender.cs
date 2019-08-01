using DXQ.Study.Mediator.MediatorConsole.Application.Commands;
using DXQ.Study.Mediator.MediatorConsole.Application.Notifications;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DXQ.Study.Mediator.MediatorConsole {
    /// <summary>
    /// 发送消息
    /// </summary>
    public class MessageSender {
        private readonly IMediator mediator;

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

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="message">消息</param>
        public void Publish(string message) {
            mediator.Publish(new HelloNotificationEvent(message, DateTime.Now));
        }
    }
}
