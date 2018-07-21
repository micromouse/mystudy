using IntegrationEvent;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace QuickStart2.Application.IntegrationEventHandler {
    /// <summary>
    /// Hello3集成事件消费者
    /// </summary>
    public class Hello3IntegrationEventConsumer : IConsumer<IHelloIntegrationEvent> {
        /// <summary>
        /// 消费Hello集成事件
        /// </summary>
        /// <param name="context">消费上下文</param>
        /// <returns>任务</returns>
        public async Task Consume(ConsumeContext<IHelloIntegrationEvent> context) {
            await Console.Out.WriteLineAsync($"Hello3消费了Hello集成事件:{context.Message.Text}");
        }
    }
}
