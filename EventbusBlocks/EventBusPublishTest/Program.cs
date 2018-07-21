using Autofac;
using DXQ.Study.EventbusBlocks.EventBus.Abstractions;
using DXQ.Study.EventbusBlocks.EventBus.Manager;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;

namespace DXQ.Study.EventbusBlocks.EventBusPublishTest {
    /// <summary>
    /// 应用程序入口
    /// </summary>
    class Program {
        /// <summary>
        /// 入口
        /// </summary>
        /// <param name="args">参数</param>
        static void Main(string[] args) {
            var containerBuilder = new ContainerBuilder();
            var factory = new ConnectionFactory() { HostName = "localhost", UserName = "dxq", Password = "dxq" };

            var service = new ServiceCollection();            
            service.AddLogging(log => log.AddConsole());
            var provide = service.BuildServiceProvider();

            var container = containerBuilder.Build();
            var connection = new EventBusRabbitMQ.DefaultIRabbitMQPersisterConnection(factory, provide.GetRequiredService<ILogger<EventBusRabbitMQ.DefaultIRabbitMQPersisterConnection>>());
            var eventbusrabbitMQ = new EventBusRabbitMQ.EventBusRabbitMQ(connection, provide.GetRequiredService<ILogger<EventBusRabbitMQ.EventBusRabbitMQ>>(), container.BeginLifetimeScope(), new InMemoryEventBusSubscriptionsManager(), "EventBusPublishTest");

            var hello = new HelloIntegrationEvent("title", "content");
            eventbusrabbitMQ.Publish(hello);
            
            Console.WriteLine("已发布");
            Console.Read();
        }
    }
}
