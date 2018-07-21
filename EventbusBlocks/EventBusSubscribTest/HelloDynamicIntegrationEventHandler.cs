using DXQ.Study.EventbusBlocks.EventBus.Abstractions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXQ.Study.EventbusBlocks.EventBusSubscribTest {
    /// <summary>
    /// Hello匿名集成事件处理器
    /// </summary>
    public class HelloDynamicIntegrationEventHandler : IDynamicIntegrationEventHandler {
        /// <summary>
        /// 处理匿名集成事件
        /// </summary>
        /// <param name="eventData">事件数据</param>
        /// <returns>Represents an asynchronous operation.</returns>
        public Task Handle(dynamic eventData) {
            Console.WriteLine($"HelloDynamicIntegrationEventHandler:{eventData.MessageContent}");
            return Task.FromResult(0);
        }
    }
}