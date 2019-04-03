using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Api.Applications.Behaviors
{
    /// <summary>
    /// 请求管道:日志行为
    /// </summary>
    /// <typeparam name="TRequest">请求类型</typeparam>
    /// <typeparam name="TResponse">相应类型</typeparam>
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        /// <summary>
        /// 初始化请求管道:日志行为
        /// </summary>
        /// <param name="logger">日志</param>
        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 处理日志
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <param name="next">下一个请求</param>
        /// <returns>响应</returns>
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            _logger.LogInformation("----- Handling command {CommandName} ({@Command})", request.GetType().Name, request);
            var response = await next();
            _logger.LogInformation("----- Command {CommandName} handled - response:{@Response}", request.GetType().Name, response);

            return response;
        }
    }
}
