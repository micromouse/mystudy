using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Ordering.Api.Applications.Commands {
    /// <summary>
    /// 取消订单命令处理器
    /// </summary>
    public class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand, string> {
        /// <summary>
        /// 处理取消订单命令
        /// </summary>
        /// <param name="request">取消订单命令</param>
        /// <param name="cancellationToken">取消Token</param>
        /// <returns>处理结果</returns>
        public Task<string> Handle(CancelOrderCommand request, CancellationToken cancellationToken) {
            throw new System.NotImplementedException();
        }
    }
}
