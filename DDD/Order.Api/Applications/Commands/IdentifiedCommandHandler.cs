using MediatR;
using Ordering.Infrastructure.Idempotency;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Api.Applications.Commands {
    /// <summary>
    /// 统一标识命令处理器
    /// </summary>
    /// <typeparam name="T">要执行的请求</typeparam>
    /// <typeparam name="R">返回值</typeparam>
    public class IdentifiedCommandHandler<T, R> : IRequestHandler<IdentifiedCommand<T, R>, R> where T : IRequest<R> {
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
        protected virtual R CreateResultForDuplicateRequest() {
            return default(R);
        }
        
        /// <summary>
        /// 处理请求
        /// </summary>
        /// <param name="request">统一标识命令</param>
        /// <param name="cancellationToken">取消Token</param>
        /// <returns>处理结果</returns>
        public async Task<R> Handle(IdentifiedCommand<T, R> request, CancellationToken cancellationToken) {
            var exists = await _requestManager.ExistAsync(request.Id);

            if (exists) {
                return this.CreateResultForDuplicateRequest();
            } else {
                //添加请求Id,防止重复请求
                await _requestManager.CreateRequestForCommandASync<T>(request.Id);

                //发送实际命令
                try {
                    var result = await _mediator.Send(request.Command);
                    return result;
                } catch (Exception ex) {
                    throw ex;
                    //return default(R);
                }
            }
        }
    }
}
