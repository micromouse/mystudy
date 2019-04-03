using FluentValidation;
using Ordering.Api.Applications.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ordering.Api.Applications.Validations
{
    /// <summary>
    /// 建立订单命令验证器
    /// </summary>
    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        /// <summary>
        /// 初始化建立订单命令验证器
        /// </summary>
        public CreateOrderCommandValidator()
        {
            RuleFor(command => command.OrderItems).Must(ExistOrderItems).WithMessage("No order itmes found");
        }

        /// <summary>
        /// 订单项是否存在
        /// </summary>
        /// <param name="orderItems">订单项集合</param>
        /// <returns>是否有订单项存在</returns>
        private bool ExistOrderItems(IEnumerable<OrderItemDTO> orderItems)
        {
            return orderItems.Any();
        }
    }
}
