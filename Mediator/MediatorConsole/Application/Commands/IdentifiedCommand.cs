using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DXQ.Study.Mediator.MediatorConsole.Application.Commands {
    /// <summary>
    /// 认定命令
    /// </summary>
    /// <typeparam name="T">命令类型</typeparam>
    /// <typeparam name="R">命令返回值类型</typeparam>
    public class IdentifiedCommand<T, R> : IRequest<R>
        where T : IRequest<R> {

        /// <summary>
        /// 命令
        /// </summary>
        public T Command { get; }

        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// 初始化认定命令
        /// </summary>
        /// <param name="command">命令</param>
        /// <param name="id">Id</param>
        public IdentifiedCommand(T command,Guid id) {
            this.Command = command;
            this.Id = id;
        }
    }
}
