using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ordering.Infrastructure;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Api.Applications.Behaviors
{
    /// <summary>
    /// 请求管道：事务行为
    /// </summary>
    /// <typeparam name="TRequest">请求类型</typeparam>
    /// <typeparam name="TResponse">相应类型</typeparam>
    public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly OrderingDbContext _dbContext;
        private readonly ILogger<TransactionBehavior<TRequest, TResponse>> _logger;

        /// <summary>
        /// 初始化请求管道:事务行为
        /// </summary>
        public TransactionBehavior(OrderingDbContext dbContext, ILogger<TransactionBehavior<TRequest, TResponse>> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// 处理事务
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="cancellationToken">取消Token</param>
        /// <param name="next">下一个请求</param>
        /// <returns>响应</returns>
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var response = default(TResponse);
            var typeName = request.GetType().Name;

            try
            {
                if (_dbContext.HasActiveTransaction)
                {
                    return await next();
                }

                var strategy = _dbContext.Database.CreateExecutionStrategy();
                await strategy.ExecuteAsync(async () =>
                {
                    using (var transaction = await _dbContext.BeginTransactionAsync())
                    {
                        response = await next();
                        await _dbContext.CommitTransactionAsync(transaction);
                    }
                });

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR Handling transaction  for {CommandName} ({@Command})", typeName, request);
                throw;
            }
        }
    }
}
