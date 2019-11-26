using MediatR;
using System;
using System.Runtime.Serialization;

namespace Ordering.Api.Applications.Commands {
    /// <summary>
    /// 取消订单命令
    /// </summary>
    [DataContract]
    public class CancelOrderCommand : IRequest<string> {
        #region 属性
        /// <summary>
        /// 订单Id
        /// </summary>
        [DataMember]
        public Guid Id { get; private set; }
        #endregion

        /// <summary>
        /// 初始化取消订单命令
        /// </summary>
        public CancelOrderCommand() : this(Guid.Empty) {

        }

        /// <summary>
        /// 初始化取消订单命令
        /// </summary>
        /// <param name="id">订单Id</param>
        public CancelOrderCommand(Guid id) {
            this.Id = id;
        }
    }
}
