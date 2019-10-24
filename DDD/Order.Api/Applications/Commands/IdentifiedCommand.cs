using MediatR;
using System;

namespace Ordering.Api.Applications.Commands {
    /// <summary>
    /// 统一标识命令
    /// </summary>
    /// <typeparam name="TCommand">要执行的请求</typeparam>
    /// <typeparam name="TResult">返回值</typeparam>
    public class IdentifiedCommand<TCommand, TResult> : IRequest<TResult> where TCommand : IRequest<TResult> {
        public TCommand Command { get; }
        public Guid Id { get; }

        /// <summary>
        /// 初始化统一标识命令
        /// </summary>
        /// <param name="command">命令</param>
        /// <param name="id">请求Id</param>
        public IdentifiedCommand(TCommand command, Guid id) {
            Command = command;
            Id = id;
        }
    }
}
