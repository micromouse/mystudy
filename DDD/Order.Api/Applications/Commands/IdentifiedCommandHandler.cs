using MediatR;
using Ordering.Infrastructure.Idempotency;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Api.Applications.Commands {
    /// <summary>
    /// 统一标识命令处理器
    /// </summary>
    /// <typeparam name="TCommand">要执行的请求</typeparam>
    /// <typeparam name="TResult">返回值</typeparam>
    public class IdentifiedCommandHandler<TCommand, TResult> : IRequestHandler<IdentifiedCommand<TCommand, TResult>, TResult> where TCommand : IRequest<TResult> {
        private readonly IMediator _mediator;
        private readonly IRequestManager _requestManager;

        /// <summary>
        /// 初始化统一标识命令处理器
        /// </summary>
        /// <param name="mediator">IMediator</param>
        /// <param name="requestManager">请求管理器</param>
        public IdentifiedCommandHandler(IMediator mediator, IRequestManager requestManager) {
            _mediator = mediator;
            _requestManager = requestManager;
        }

        /// <summary>
        /// 为重复请求建立返回值
        /// </summary>
        /// <returns>重复请求返回值</returns>
        protected virtual TResult CreateResultForDuplicateRequest() {
            return default;
        }

        /// <summary>
        /// 处理请求
        /// </summary>
        /// <param name="request">统一标识命令</param>
        /// <param name="cancellationToken">取消Token</param>
        /// <returns>处理结果</returns>
        public async Task<TResult> Handle(IdentifiedCommand<TCommand, TResult> request, CancellationToken cancellationToken) {
            var exists = await _requestManager.ExistAsync(request.Id);
            if (exists) {
                return this.CreateResultForDuplicateRequest();
            }

            //添加请求Id,防止重复请求
            await _requestManager.CreateRequestForCommandASync<TCommand>(request.Id);

            //发送实际命令
            var result = await _mediator.Send(request.Command, cancellationToken);
            return result;
        }
    }
}
