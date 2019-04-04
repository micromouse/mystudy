using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Ordering.Domain.AggregateModel.BuyerAggregate;
using Ordering.Domain.Events;

namespace Ordering.Api.Applications.DomainEventHandlers.OrderStartedEvent {
    /// <summary>
    /// 订单已开始领域事件处理器,验证/添加买家聚合
    /// </summary>
    public class VerifyOrAddBuyerAggregateWhenOrderStartedDomainEventHandler : INotificationHandler<OrderStartedDomainEvent> {
        private readonly IBuyerRepository _buyerRepository;

        /// <summary>
        /// 初始化订单已开始领域事件处理器
        /// </summary>
        /// <param name="buyerRepository">买家仓储</param>
        public VerifyOrAddBuyerAggregateWhenOrderStartedDomainEventHandler(IBuyerRepository buyerRepository) {
            _buyerRepository = buyerRepository;
        }

        /// <summary>
        /// 处理
        /// </summary>
        /// <param name="orderStartedDomainEvent">订单已开始领域事件</param>
        /// <param name="cancellationToken">取消Token</param>
        /// <returns>任务</returns>
        public async Task Handle(OrderStartedDomainEvent orderStartedDomainEvent, CancellationToken cancellationToken) {
            var buyer = await _buyerRepository.FindAsync(orderStartedDomainEvent.UserId);
            var existBuyer = buyer != null;

            if (!existBuyer) {
                buyer = new Buyer(orderStartedDomainEvent.UserId, orderStartedDomainEvent.UserName);
            }
            buyer.VerifyOrAddPaymentMethod(orderStartedDomainEvent.CardTypeId,
                                           $"Payment Mehtod on {DateTime.Now}",
                                           orderStartedDomainEvent.CardNumber,
                                           orderStartedDomainEvent.CardSecurityNumber,
                                           orderStartedDomainEvent.CardHolderName,
                                           orderStartedDomainEvent.CardExpiration,
                                           orderStartedDomainEvent.Order.Id);
            _ = existBuyer ?
                _buyerRepository.Update(buyer) :
                _buyerRepository.Add(buyer);

            //分发领域事件
            await _buyerRepository.UnitOfWork
                .SaveEntitiesAsync();
        }
    }
}
