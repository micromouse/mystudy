using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Ordering.Domain.AggregateModel.OrderAggregate;
using Ordering.Infrastructure.Idempotency;

namespace Ordering.Api.Applications.Commands {
    /// <summary>
    /// 建立订单命令处理器
    /// </summary>
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, bool> {
        private readonly IMediator _mediator;
        private readonly IOrderRepository _orderRepository;

        /// <summary>
        /// 初始化建立订单命令处理器
        /// </summary>
        /// <param name="mediator">IMediator</param>
        /// <param name="orderRepository">订单仓储</param>
        public CreateOrderCommandHandler(IMediator mediator, IOrderRepository orderRepository) {
            _mediator = mediator;
            _orderRepository = orderRepository;
        }

        /// <summary>
        /// 处理建立订单命令
        /// </summary>
        /// <param name="request">建立订单命令</param>
        /// <param name="cancellationToken">取消Token</param>
        /// <returns>处理结果</returns>
        public async Task<bool> Handle(CreateOrderCommand request, CancellationToken cancellationToken) {
            var address = new Address(request.Street, request.City, request.State, request.Country, request.ZipCode);
            var order = new Order(request.UserId, request.UserName, address, request.CardTypeId, request.CardNumber, request.CardSecurityNumber, request.CardHolderName, request.CardExpiration);
            foreach (var item in request.OrderItems) {
                order.AddOrderItem(item.ProductId, item.ProductName, item.UnitPrice, item.Discount, item.PictureUrl, item.Units);
            }
            _orderRepository.Add(order);

            return await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }

    /*
    /// <summary>
    /// 建立订单统一标识命令处理器
    /// </summary>
    public class CreateOrderIdentifiedCommandHandler : IdentifiedCommandHandler<CreateOrderCommand, bool> {
        /// <summary>
        /// 初始化建立订单统一标识命令处理器
        /// </summary>
        /// <param name="mediator">IMediator</param>
        /// <param name="requestManager">请求管理器</param>
        public CreateOrderIdentifiedCommandHandler(IMediator mediator, IRequestManager requestManager) : base(mediator, requestManager) {
        }

        /// <summary>
        /// 为重复请求建立返回值
        /// </summary>
        /// <returns>重复请求返回值</returns>
        protected override bool CreateResultForDuplicateRequest() {
            return true;
        }
    }
    */
}
