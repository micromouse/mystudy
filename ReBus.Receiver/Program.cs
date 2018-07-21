using Microsoft.Extensions.DependencyInjection;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using Rebus.ServiceProvider;
using System;

namespace ReBus.Receiver
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();

            services.AutoRegisterHandlersFromAssemblyOf<Handler1>();
            services.AddRebus(configure => configure
                .Transport(t => t.UseRabbitMqAsOneWayClient("amqp://dxq:dxq@localhost")));

            var provider = services.BuildServiceProvider();
            provider.UseRebus();
        }
    }
}
