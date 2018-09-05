using MediatR;
using Ordering.Domain.AggregateModel.OrderAggregate;
using Ordering.Domain.Events;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Api.Applications.DomainEventHandlers.BuyerAndPaymentMethodVerified {
    /// <summary>
    /// 买家付款方式已验证领域事件处理器,更新订单
    /// </summary>
    public class UpdateOrderWhenBuyerAndPaymentMethodVerifiedDomainEventHandler : INotificationHandler<BuyerAndPaymentMethodVerifiedDomainEvent> {
        private readonly IOrderRepository _orderRepository;

        /// <summary>
        /// 初始化买家付款方式已验证领域事件处理器
        /// </summary>
        /// <param name="orderRepository">订单仓储</param>
        public UpdateOrderWhenBuyerAndPaymentMethodVerifiedDomainEventHandler(IOrderRepository orderRepository) {
            _orderRepository = orderRepository;
        }

        /// <summary>
        /// 处理
        /// </summary>
        /// <param name="buyerPaymentMehtodVerifiedEvent">买家付款方式已验证领域事件</param>
        /// <param name="cancellationToken">取消Token</param>
        /// <returns>任务</returns>
        public async Task Handle(BuyerAndPaymentMethodVerifiedDomainEvent buyerPaymentMehtodVerifiedEvent, CancellationToken cancellationToken) {
            var orderToUpdate = await _orderRepository.GetAsync(buyerPaymentMehtodVerifiedEvent.OrderId);
            orderToUpdate.SetPaymentId(buyerPaymentMehtodVerifiedEvent.PaymentMethod.Id);
            orderToUpdate.SetBuyerId(buyerPaymentMehtodVerifiedEvent.Buyer.Id);
        }
    }
}
