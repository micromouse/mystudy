using Autofac;
using FluentValidation;
using MediatR;
using Ordering.Api.Applications.Behaviors;
using Ordering.Api.Applications.Commands;
using Ordering.Api.Applications.DomainEventHandlers.OrderStartedEvent;
using Ordering.Api.Applications.Validations;
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

            //注入验证处理器
            builder.RegisterAssemblyTypes(typeof(CreateOrderCommandValidator).GetTypeInfo().Assembly)
                .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
                .AsImplementedInterfaces();

            builder.Register<ServiceFactory>(ctx =>
            {
                var componentContext = ctx.Resolve<IComponentContext>();
                return t => componentContext.TryResolve(t, out object o) ? o : null;
            });

            //注入请求管道
            builder.RegisterGeneric(typeof(LoggingBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(ValidatorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(TransactionBehavior<,>)).As(typeof(IPipelineBehavior<,>));
        }
    }
}
