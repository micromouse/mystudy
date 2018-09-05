using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Ordering.Api.Applications.Queries;
using Ordering.Domain.AggregateModel.OrderAggregate;
using Ordering.Infrastructure.Idempotency;
using Ordering.Infrastructure.Repositories;

namespace Ordering.Api.Infrastructure.AutofacModules {
    /// <summary>
    /// 应用程序注入
    /// </summary>
    public class ApplicationModule : Autofac.Module {
        private readonly string _connectionString;

        /// <summary>
        /// 初始化应用程序注入
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        public ApplicationModule(string connectionString) {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Load
        /// </summary>
        /// <param name="builder">ContainerBuilder</param>
        protected override void Load(ContainerBuilder builder) {
            builder.Register(c => new OrderQuery(_connectionString))
                .As<IOrderQuery>()
                .InstancePerLifetimeScope();

            builder.RegisterType<RequestManager>()
                .As<IRequestManager>()
                .InstancePerLifetimeScope();

            builder.RegisterType<OrderRepository>()
                .As<IOrderRepository>()
                .InstancePerLifetimeScope();
        }
    }
}
