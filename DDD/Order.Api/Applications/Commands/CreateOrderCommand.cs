using MediatR;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Ordering.Api.Applications.Commands {
    /// <summary>
    /// 建立订单命令
    /// </summary>
    [DataContract]
    public class CreateOrderCommand : IRequest<bool> {
        [DataMember]
        private readonly List<OrderItemDTO> _orderItems;

        #region 属性
        /// <summary>
        /// 用户Id
        /// </summary>
        [DataMember]
        public string UserId { get; private set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        [DataMember]
        public string UserName { get; private set; }
        /// <summary>
        /// 城市
        /// </summary>
        [DataMember]
        public string City { get; private set; }
        /// <summary>
        /// 街道
        /// </summary>
        [DataMember]
        public string Street { get; private set; }
        /// <summary>
        /// 州
        /// </summary>
        [DataMember]
        public string State { get; private set; }
        /// <summary>
        /// 国家
        /// </summary>
        [DataMember]
        public string Country { get; private set; }
        /// <summary>
        /// 邮编
        /// </summary>
        [DataMember]
        public string ZipCode { get; private set; }
        /// <summary>
        /// 卡号
        /// </summary>
        [DataMember]
        public string CardNumber { get; private set; }
        /// <summary>
        /// 持卡人姓名
        /// </summary>
        [DataMember]
        public string CardHolderName { get; private set; }
        /// <summary>
        /// 卡有效期
        /// </summary>
        [DataMember]
        public DateTime CardExpiration { get; private set; }
        /// <summary>
        /// 卡安全号
        /// </summary>
        [DataMember]
        public string CardSecurityNumber { get; private set; }
        /// <summary>
        /// 卡类型Id
        /// </summary>
        [DataMember]
        public int CardTypeId { get; private set; }
        /// <summary>
        /// 订单项集合
        /// </summary>
        [DataMember]
        public IEnumerable<OrderItemDTO> OrderItems => _orderItems;
        #endregion

        /// <summary>
        /// 初始化建立订单命令
        /// </summary>
        public CreateOrderCommand() {
            _orderItems = new List<OrderItemDTO>();
        }

        /// <summary>
        /// 初始化建立订单命令
        /// </summary>
        /// <param name="orderItems">订单项集合</param>
        /// <param name="userId">用户Id</param>
        /// <param name="userName">用户名称</param>
        /// <param name="city">城市</param>
        /// <param name="street">街道</param>
        /// <param name="state">州</param>
        /// <param name="country">国家</param>
        /// <param name="zipcode">邮编</param>
        /// <param name="cardNumber">卡号</param>
        /// <param name="cardHolderName">持卡人姓名</param>
        /// <param name="cardExpiration">卡有效期</param>
        /// <param name="cardSecurityNumber">卡安全号</param>
        /// <param name="cardTypeId">卡类型Id</param>
        public CreateOrderCommand(List<OrderItemDTO> orderItems, string userId, string userName, string city,
                                string street, string state, string country, string zipcode, string cardNumber,
                                string cardHolderName, DateTime cardExpiration, string cardSecurityNumber,
                                int cardTypeId) : this() {
            _orderItems = orderItems;
            UserId = UserId;
            UserName = userName;
            City = city;
            Street = street;
            State = state;
            Country = country;
            ZipCode = zipcode;
            CardNumber = cardNumber;
            CardHolderName = cardHolderName;
            CardExpiration = cardExpiration;
            CardSecurityNumber = cardSecurityNumber;
            CardTypeId = cardTypeId;
        }
    }

    /// <summary>
    /// 订单项DTO
    /// </summary>
    public class OrderItemDTO {
        /// <summary>
        /// 产品Id
        /// </summary>
        public int ProductId { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public decimal UnitPrice { get; set; }
        /// <summary>
        /// 折扣
        /// </summary>
        public decimal Discount { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Units { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        public string PictureUrl { get; set; }
    }

}
