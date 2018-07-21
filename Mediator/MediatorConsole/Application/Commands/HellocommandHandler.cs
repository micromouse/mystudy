using DXQ.Study.Mediator.MediatorConsole.Application.Notifications;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DXQ.Study.Mediator.MediatorConsole.Application.Commands {
    /// <summary>
    /// 打招呼命令处理器
    /// </summary>
    public class HellocommandHandler : IRequestHandler<HelloCommand, bool> {
        private IMediator mediator = null;

        /// <summary>
        /// 初始化打招呼命令处理器
        /// </summary>
        /// <param name="mediator">MediatR</param>
        public HellocommandHandler(IMediator mediator) {
            this.mediator = mediator;
        }

        /// <summary>
        /// 处理打招呼命令
        /// </summary>
        /// <param name="request">打招呼命令</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns></returns>
        public Task<bool> Handle(HelloCommand request, CancellationToken cancellationToken) {
            Console.WriteLine($"处理了HelloCommand命令,内容是:{request.Content}");
            mediator.Publish(new HelloNotificationEvent(request.Content, DateTime.Now));
            return Task.FromResult(true);
        }
    }

    /// <summary>
    /// 打招呼命令认定命令处理器
    /// </summary>
    public class HelloCommandIdentifiedCommandHandler : IdentifiedCommandHandler<HelloCommand, bool> {
        /// <summary>
        /// 初始化打招呼命令认定命令处理器
        /// </summary>
        /// <param name="mediator">MediatR</param>
        public HelloCommandIdentifiedCommandHandler(IMediator mediator) : base(mediator) {
        }
    }
}
