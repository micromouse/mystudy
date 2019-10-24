using Autofac;
using FluentValidation;
using MediatR;
using Ordering.Api.Applications.Behaviors;
using Ordering.Api.Applications.Commands;
using Ordering.Api.Applications.DomainEventHandlers.OrderStartedEvent;
using Ordering.Api.Applications.Validations;
using System.Linq;
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

            //注入:IRequestHandler<TCommand<Bool>,Bool>的实现IdentifiedCommandHandler<TCommand<Bool>,Bool>
            //比如:IRequestHandler<CreateOrderCommand,Bool>的实现IdentifiedCommandHandler<CreateOrderCommand,Bool>
            var handlers = typeof(CreateOrderCommand).GetTypeInfo().Assembly
                .GetTypes()
                .Where(t => t.IsClosedTypeOf(typeof(IRequestHandler<,>)))
                .ToList();
            foreach (var handler in handlers) {
                var localHandlers = handler.GetInterfaces().Where(t => t.IsClosedTypeOf(typeof(IRequestHandler<,>)));
                foreach (var localHandler in localHandlers) {
                    var implementation = typeof(IdentifiedCommandHandler<,>).MakeGenericType(localHandler.GenericTypeArguments);
                    var arg0 = typeof(IdentifiedCommand<,>).MakeGenericType(localHandler.GenericTypeArguments);
                    var arg1 = localHandler.GenericTypeArguments[1];

                    var service = typeof(IRequestHandler<,>).MakeGenericType(arg0, arg1);
                    builder.RegisterType(implementation).As(service);
                }
            }
            builder.RegisterAssemblyTypes(typeof(IdentifiedCommand<,>).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IRequestHandler<,>));

            //注入Publish处理器,一个Publish可以有多个NotificationHandler
            builder.RegisterAssemblyTypes(typeof(VerifyOrAddBuyerAggregateWhenOrderStartedDomainEventHandler).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(INotificationHandler<>))
                .AsImplementedInterfaces();

            builder.Register<ServiceFactory>(ctx =>
            {
                var componentContext = ctx.Resolve<IComponentContext>();
                return t => componentContext.TryResolve(t, out object o) ? o : null;
            });

            //注入验证处理器
            builder.RegisterAssemblyTypes(typeof(CreateOrderCommandValidator).GetTypeInfo().Assembly)
                .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
                .AsImplementedInterfaces();

            //注入请求管道
            builder.RegisterGeneric(typeof(LoggingBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(ValidatorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(TransactionBehavior<,>)).As(typeof(IPipelineBehavior<,>));
        }
    }
}
