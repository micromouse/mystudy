using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ordering.Api.Applications.Commands {
    /// <summary>
    /// 统一标识命令
    /// </summary>
    /// <typeparam name="T">要执行的请求</typeparam>
    /// <typeparam name="R">返回值</typeparam>
    public class IdentifiedCommand<T, R> : IRequest<R> where T : IRequest<R> {
        public T Command { get; }
        public Guid Id { get; }

        /// <summary>
        /// 初始化统一标识命令
        /// </summary>
        /// <param name="command">命令</param>
        /// <param name="id">请求Id</param>
        public IdentifiedCommand(T command, Guid id) {
            Command = command;
            Id = id;
        }
    }
}
