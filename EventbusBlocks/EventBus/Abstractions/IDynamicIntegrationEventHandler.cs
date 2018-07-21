using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DXQ.Study.EventbusBlocks.EventBus.Abstractions {
    /// <summary>
    /// 匿名集成事件处理器接口
    /// </summary>
    public interface IDynamicIntegrationEventHandler {
        /// <summary>
        /// 处理匿名集成事件
        /// </summary>
        /// <param name="eventData">事件数据</param>
        /// <returns>Represents an asynchronous operation.</returns>
        Task Handle(dynamic eventData);
    }
}
