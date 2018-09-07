using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Api.Applications.Commands;
using Ordering.Api.Applications.Queries;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Ordering.Api.Controllers {
    /// <summary>
    /// 订单控制器
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase {
        private readonly IMediator _mediator;
        private readonly IOrderQuery _orderQuery;

        /// <summary>
        /// 初始化订单控制器
        /// </summary>
        /// <param name="mediator">IMediator</param>
        /// <param name="orderQuery">订单查询</param>
        public OrdersController(IMediator mediator, IOrderQuery orderQuery) {
            _mediator = mediator;
            _orderQuery = orderQuery;
        }

        /// <summary>
        /// 建立订单
        /// </summary>
        /// <param name="command">建立订单命令</param>
        /// <param name="requestId">请求Id</param>
        /// <returns>结果</returns>
        [Route("create")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand command, [FromHeader(Name = "x-requestid")] string requestId) {
            var commandResult = false;

            if (Guid.TryParse(requestId, out Guid guid) && guid != Guid.Empty) {
                var requestCreateOrder = new IdentifiedCommand<CreateOrderCommand, bool>(command, guid);
                commandResult = await _mediator.Send(requestCreateOrder);
            }

            return commandResult ? (IActionResult)Ok() : (IActionResult)BadRequest();
        }


    }
}