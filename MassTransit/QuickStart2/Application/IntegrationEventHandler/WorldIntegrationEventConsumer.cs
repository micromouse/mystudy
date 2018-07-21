using IntegrationEvent;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace QuickStart2.Application.IntegrationEventHandler {
    /// <summary>
    /// World集成事件消费者
    /// </summary>
    public class WorldIntegrationEventConsumer : IConsumer<IWorldIntegrationEvent> {
        /// <summary>
        /// 消费World集成事件
        /// </summary>
        /// <param name="context">消费上下文</param>
        /// <returns>任务</returns>
        public async Task Consume(ConsumeContext<IWorldIntegrationEvent> context) {
            await Console.Out.WriteLineAsync($"我消费了World集成事件:{context.Message.Createby}");
        }
    }
}
