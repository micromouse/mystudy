using Autofac;
using MediatR;
using Ordering.Api.Applications.Commands;
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
            builder.RegisterAssemblyTypes(typeof(CreateOrderCommand).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IRequestHandler<,>));

            builder.Register<ServiceFactory>(ctx =>
            {
                var componentContext = ctx.Resolve<IComponentContext>();
                return t => componentContext.TryResolve(t, out object o) ? o : null;
            });
        }
    }
}
