using MediatR;
using Ordering.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure {
    /// <summary>
    /// MediatR扩展
    /// </summary>
    public static class MediatorExtension {
        /// <summary>
        /// 分发订单DbContext领域事件
        /// </summary>
        /// <param name="mediator">IMediator</param>
        /// <param name="context">订单DbContext</param>
        /// <returns>任务</returns>
        public static async Task DispatchDomainEventsAsync(this IMediator mediator, OrderingDbContext context) {
            var domainEntities = context.ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());
            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();

            //清空所有领域事件
            domainEntities.ToList()
                .ForEach(x => x.Entity.ClearDomainEvents());

            //异步分发所有领域事件
            var tasks = domainEvents
                .Select(async (domainEvent) =>
                {
                    await mediator.Publish(domainEvent);
                });
            await Task.WhenAll(tasks);
        }
    }
}
