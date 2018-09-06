using Autofac;
using MediatR;
using Ordering.Api.Applications.Commands;
using Ordering.Api.Applications.DomainEventHandlers.OrderStartedEvent;
using System.Reflection;

namespace Ordering.Api.Infrastructure.AutofacModules {
    /// <summary>
    /// MediatR注入
    /// </summary>
    public class MediatorModule : Autofac.Module {
        /// <summary>
        /// Load
        /// </summary>
        /// <param name="builder">ContainerBuilder</param>
        protected override void Load(ContainerBuilder builder) {
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
                .AsImplementedInterfaces();

            //Specifies that a type from a scanned assembly is registered 
            //if it implements an interface that closes the provided open generic interface type.
            //注入Send请求处理器,一对一的消息
            builder.RegisterAssemblyTypes(typeof(CreateOrderCommand).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IRequestHandler<,>))
                .AsImplementedInterfaces();

            //注入Publish处理器,一个Publish可以有多个NotificationHandler
            builder.RegisterAssemblyTypes(typeof(VerifyOrAddBuyerAggregateWhenOrderStartedDomainEventHandler).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(INotificationHandler<>))
                .AsImplementedInterfaces();
                

            builder.Register<ServiceFactory>(ctx =>
            {
                var componentContext = ctx.Resolve<IComponentContext>();
                return t => componentContext.TryResolve(t, out object o) ? o : null;
            });
        }
    }
}
