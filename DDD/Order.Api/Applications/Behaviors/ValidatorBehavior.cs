using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Api.Applications.Behaviors
{
    /// <summary>
    /// 请求管道：验证请求行为
    /// </summary>
    /// <typeparam name="TRequest">请求类型</typeparam>
    /// <typeparam name="TResponse">相应类型</typeparam>
    public class ValidatorBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IValidator<TRequest>[] _validators;
        private readonly ILogger<ValidatorBehavior<TRequest, TResponse>> _logger;

        /// <summary>
        /// 初始化请求管道：验证请求行为
        /// </summary>
        /// <param name="validators">验证器集合</param>
        /// <param name="logger">日志</param>
        public ValidatorBehavior(IValidator<TRequest>[] validators, ILogger<ValidatorBehavior<TRequest, TResponse>> logger)
        {
            _validators = validators;
            _logger = logger;
        }

        /// <summary>
        /// 处理验证
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="cancellationToken">取消Token</param>
        /// <param name="next">下一个请求</param>
        /// <returns>响应</returns>
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            _logger.LogInformation("----- Validating command {CommandType}", request.GetType().Name);

            var failures = _validators
                .Select(v => v.Validate(request))
                .SelectMany(result => result.Errors)
                .Where(error => error != null)
                .ToList();
            if (failures.Any())
            {
                _logger.LogWarning("Validation errors - {CommandType} - Command:{@Command} - Errors:{@ValidationErrors}", request.GetType().Name, request, failures);
                throw new OrderingDomainException(
                    $"Command Validation Errors for type {typeof(TRequest).Name}",
                    new ValidationException("Validation exception", failures)
                );
            }

            return await next();
        }
    }
}
