using DXQ.Study.EventbusBlocks.EventBus.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXQ.Study.EventbusBlocks.EventBusSubscribTest {
    /// <summary>
    /// Hello集成事件处理器
    /// </summary>
    public class HelloIntegrationEventHandler : IIntegrationEventHandler<HelloIntegrationEvent> {
        /// <summary>
        /// 处理
        /// </summary>
        /// <param name="event">Hello集成事件</param>
        /// <returns>任务</returns>
        public Task Handle(HelloIntegrationEvent @event) {
            Console.WriteLine($"messagetitle:{@event.MessageTitle},messagecontent:{@event.MessageContent}");
            return Task.FromResult(0);
        }
    }
}