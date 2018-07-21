using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DXQ.Study.Mediator.MediatorConsole.Application.Commands {
    /// <summary>
    /// 认定命令处理器
    /// </summary>
    /// <typeparam name="T">命令类型</typeparam>
    /// <typeparam name="R">命令返回值类型</typeparam>
    public class IdentifiedCommandHandler<T, R> : IRequestHandler<IdentifiedCommand<T, R>, R>
        where T : IRequest<R> {
        private IMediator mediator = null;

        /// <summary>
        /// 初始化认定命令处理器
        /// </summary>
        /// <param name="mediator">MediatR</param>
        public IdentifiedCommandHandler(IMediator mediator) {
            this.mediator = mediator;
        }

        /// <summary>
        /// 处理认定命令
        /// </summary>
        /// <param name="request">认定命令</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns></returns>
        public async Task<R> Handle(IdentifiedCommand<T, R> request, CancellationToken cancellationToken) {
            var result = await mediator.Send(request.Command);
            return result;
        }
    }
}
