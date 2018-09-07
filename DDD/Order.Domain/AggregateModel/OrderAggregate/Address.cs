using Ordering.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Domain.AggregateModel.OrderAggregate {
    /// <summary>
    /// 地址值对象
    /// </summary>
    public class Address : ValueObject {
        #region 公共属性
        public string Street { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }
        public string Country { get; private set; }
        public string ZipCode { get; private set; }
        #endregion

        /// <summary>
        /// 初始化地址值对象
        /// </summary>
        private Address() {

        }

        /// <summary>
        /// 初始化地址值对象
        /// </summary>
        /// <param name="street">街道</param>
        /// <param name="city">城市</param>
        /// <param name="state">州</param>
        /// <param name="country">国家</param>
        /// <param name="zipcode">邮编</param>
        public Address(string street, string city, string state, string country, string zipcode) {
            Street = street;
            City = city;
            State = state;
            Country = country;
            ZipCode = zipcode;
        }

        /// <summary>
        /// 获得原子值集合
        /// </summary>
        /// <returns>原子值集合</returns>
        protected override IEnumerable<object> GetAtomicValues() {
            //Using a yield return statement to return each element on at a time
            yield return Street;
            yield return City;
            yield return State;
            yield return Country;
            yield return ZipCode;
        }
    }
}
