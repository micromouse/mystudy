using Newtonsoft.Json;
using Ordering.Api.Applications.Commands;
using Ordering.Domain.AggregateModel.BuyerAggregate;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ordering.IntegrationTests {
    /// <summary>
    /// 订单测试
    /// </summary>
    public class OrderingScenario : OrderingScenarioBase {
        /// <summary>
        /// 建立订单，返回ok状态码
        /// </summary>
        /// <returns>任务</returns>
        [Fact]
        public async Task Create_order_and_response_ok_status_code() {
            using (var server = CreateServer()) {
                var content = new StringContent(this.GetCreateOrder(), Encoding.UTF8, "application/json");
                var response = await server.CreateIdempotentClient()
                    .PostAsync("api/v1/Orders/create", content);
                response.EnsureSuccessStatusCode();
            }
        }

        /// <summary>
        /// 获得建立订单
        /// </summary>
        /// <returns>建立订单</returns>
        private string GetCreateOrder() {
            var orderItems = new List<OrderItemDTO> {
                new OrderItemDTO{ProductId=1,ProductName="vivo x21",Discount=0.9m,UnitPrice=1500,Units=2,PictureUrl="http://www.baidu.com/images/1.jpg"},
                new OrderItemDTO{ProductId=2,ProductName="vivo x22",Discount=0.8m,UnitPrice=2500,Units=2,PictureUrl="http://www.baidu.com/images/2.jpg"},
                new OrderItemDTO{ProductId=3,ProductName="vivo x23",Discount=0.7m,UnitPrice=3500,Units=2,PictureUrl="http://www.baidu.com/images/3.jpg"}
            };
            var order = new CreateOrderCommand(orderItems: orderItems, userId: Guid.NewGuid().ToString(), userName: "张三",
                                               city: "成都市", street: "东大街", state: "四川省", country: "中国", zipcode: "610051",
                                               cardNumber: "4392250804822970", cardHolderName: "张三", cardExpiration: DateTime.Now.AddYears(10),
                                               cardSecurityNumber: "632", cardTypeId: CardType.Visa.Id);

            return JsonConvert.SerializeObject(order);
        }
    }
}
