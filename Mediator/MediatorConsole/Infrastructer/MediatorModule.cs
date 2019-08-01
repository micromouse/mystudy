using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Autofac;
using DXQ.Study.Mediator.MediatorConsole.Application.Commands;
using DXQ.Study.Mediator.MediatorConsole.Application.Notifications;
using MediatR;

namespace DXQ.Study.Mediator.MediatorConsole.Infrastructer {
    /// <summary>
    /// 注入MediatR模块相关
    /// </summary>
    public class MediatorModule : Autofac.Module {
        /// <summary>
        /// Load
        /// </summary>
        /// <param name="builder">ContainerBuilder</param>
        protected override void Load(ContainerBuilder builder) {
            //注入MediatR程序集所有接口,注册类型提供所有其公共接口作为服务(不包括IDisposable接口)
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly).AsImplementedInterfaces();

            //注入HelloCommand所在程序集所有IRequestHandler,可分配给注册类型一个接近开放泛型类型的实例
            builder.RegisterAssemblyTypes(typeof(HelloCommand).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(IRequestHandler<,>));

            //注入HelloeventHandler所在程序集所有INotificationHandler,可分配给注册类型一个接近开放泛型类型的实例
            builder.RegisterAssemblyTypes(typeof(MyHelloeventHandler).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(INotificationHandler<>));

            builder.Register<ServiceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => c.TryResolve(t, out object o) ? o : null;
            });
            /*
            builder.Register<SingleInstanceFactory>(context =>
            {
                var componentContext = context.Resolve<IComponentContext>();
                return t => componentContext.TryResolve(t, out object o) ? o : null;
            });
            builder.Register<MultiInstanceFactory>(context =>
            {
                var componentContext = context.Resolve<IComponentContext>();
                return t => (IEnumerable<object>)componentContext.Resolve(typeof(IEnumerable<>).MakeGenericType(t));
            });
            */
        }
    }
}
