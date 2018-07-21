using Autofac;
using DXQ.Study.EventbusBlocks.EventBus.Manager;
using DXQ.Study.EventbusBlocks.EventBusRabbitMQ;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;

namespace DXQ.Study.EventbusBlocks.EventBusSubscribTest {
    /// <summary>
    /// 应用程序入口
    /// </summary>
    class Program {
        /// <summary>
        /// 入口
        /// </summary>
        /// <param name="args">参数</param>
        static void Main(string[] args) {
            var service = new ServiceCollection();
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterType<HelloIntegrationEventHandler>();
            containerBuilder.RegisterType<HelloDynamicIntegrationEventHandler>();
            service.AddLogging(log => log.AddConsole());
            var provider = service.BuildServiceProvider(); 

            var container = containerBuilder.Build();
            var factory = new ConnectionFactory();
            var manager = new InMemoryEventBusSubscriptionsManager();
            var connection = new DefaultIRabbitMQPersisterConnection(factory, provider.GetRequiredService<ILogger<DefaultIRabbitMQPersisterConnection>>());
            var eventbus = new EventBusRabbitMQ.EventBusRabbitMQ(connection, provider.GetRequiredService<ILogger<EventBusRabbitMQ.EventBusRabbitMQ>>(), container.BeginLifetimeScope(), manager, "EventBusSubscribTest");

            eventbus.Subscribe<HelloIntegrationEvent, HelloIntegrationEventHandler>();
            eventbus.SubscrbieDynamic<HelloDynamicIntegrationEventHandler>("HelloIntegrationEvent");
        }
    }
}
