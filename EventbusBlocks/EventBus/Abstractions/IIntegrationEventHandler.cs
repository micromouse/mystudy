using DXQ.Study.EventbusBlocks.EventBus.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DXQ.Study.EventbusBlocks.EventBus.Abstractions {
    /// <summary>
    /// 处理集成事件的集成事件处理器接口
    /// </summary>
    /// <typeparam name="TIntegratioinEvent">集成事件</typeparam>
    public interface IIntegrationEventHandler<in TIntegratioinEvent> : IIntegrationEventHandler
        where TIntegratioinEvent : IntegrationEvent {
        /// <summary>
        /// 处理集成事件
        /// </summary>
        /// <param name="event">集成事件</param>
        /// <returns>Represents an asynchronous operation.</returns>
        Task Handle(TIntegratioinEvent @event);
    }

    /// <summary>
    /// 集成事件处理器接口
    /// </summary>
    public interface IIntegrationEventHandler {
    }
}
