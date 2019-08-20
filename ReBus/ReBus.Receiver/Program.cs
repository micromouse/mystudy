using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace ReBus.Receiver {
    class Program {

        static void Main(string[] args) {/*
            var services = new ServiceCollection();

            services.AutoRegisterHandlersFromAssemblyOf<Handler1>();
            services.AddRebus(configure => configure
                .Transport(t => t.UseRabbitMqAsOneWayClient("amqp://dxq:dxq@localhost")));

            var provider = services.BuildServiceProvider();
            provider.UseRebus();
            */
            Initializer<DbContext>.Initial((x) =>
            {
                x.Registery(typeof(Program).Assembly);
            });

            var list = new List<string> { "1", "3", "5", "7" };
            var person = new Person() { Ids = list };
            var exp = "(x.Ids.Contains(\"1\") || x.Ids.Contains(\"9\")) && x.Ids.Contains(\"4\")";
            var xparam = Expression.Parameter(typeof(Person), "x");
            var e = System.Linq.Dynamic.Core.DynamicExpressionParser.ParseLambda(new[] { xparam }, typeof(bool), exp);

            var b = list.AsQueryable().Any("ToString()==\"-1\" || ToString()==\"0\"");

            var result = e.Compile().DynamicInvoke(person);
            Console.Read();
        }
    }

    public class Person {
        public IList<string> Ids { get; set; }
    }

    public class Initializer<TDbContext> where TDbContext : DbContext {
        private readonly MessageQueue messageQueue;

        private Initializer() {
            messageQueue = new MessageQueue();
        }

        public static void Initial(Action<Initializer<TDbContext>> configure) {
            configure(new Initializer<TDbContext>());
        }

        public void Registery(Assembly assembly) {
            messageQueue.Register<TDbContext>(assembly);
        }

        public void Subscribe<TIntegrationEvent, TIIntegrationEventHandler>()
            where TIntegrationEvent : IntegrationEvent
            where TIIntegrationEventHandler : IIntegrationEventHandler<TIntegrationEvent> {
            messageQueue.Subscribe<TDbContext, TIntegrationEvent, TIIntegrationEventHandler>();
        }
    }


    public class MessageQueue {
        public void Register<T>(Assembly assembly) where T : DbContext {
            Console.WriteLine($"T:{typeof(T).Name},assembly:{assembly.FullName}");
        }

        public void Subscribe<TDbContext, TIntegrationEvent, TIIntegrationEventHandler>()
            where TDbContext : DbContext
            where TIntegrationEvent : IntegrationEvent
            where TIIntegrationEventHandler : IIntegrationEventHandler<TIntegrationEvent> {

        }

    }

    public class IntegrationEvent {
        public Guid Id { get; set; }
    }

    public interface IIntegrationEventHandler<TIntegrationEvent> : IIntegrationEventHandler where TIntegrationEvent : IntegrationEvent {
        Task Handle(TIntegrationEvent @event);
    }

    public interface IIntegrationEventHandler {
    }
}
