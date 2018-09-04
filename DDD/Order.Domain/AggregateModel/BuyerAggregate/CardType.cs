using Ordering.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Domain.AggregateModel.BuyerAggregate {
    /// <summary>
    /// 卡类型扩展枚举值
    /// </summary>
    public class CardType : Enumeration {
        public static CardType Amex = new CardType(1, "Amex");
        private static CardType Visa = new CardType(2, "Visa");
        private static CardType MasterCard = new MasterCard();

        /// <summary>
        /// 初始化卡类型扩展枚举值
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="name">名称</param>
        public CardType(int id, string name) : base(id, name) {

        }
    }

    /// <summary>
    /// MasterCard卡类型扩展枚举值
    /// </summary>
    public class MasterCard : CardType {
        /// <summary>
        /// 初始化MasterCard卡类型扩展枚举值
        /// </summary>
        public MasterCard() : base(3, "MasterCard") {
        }
    }
}
