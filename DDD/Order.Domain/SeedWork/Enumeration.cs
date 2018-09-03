using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ordering.Domain.SeedWork {
    /// <summary>
    /// 扩展枚举值
    /// </summary>
    public abstract class Enumeration : IComparable {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; private set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 初始化枚举值
        /// </summary>
        protected Enumeration() {

        }

        /// <summary>
        /// 初始化扩展枚举值
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="name">名称</param>
        protected Enumeration(int id, string name) {
            Id = id;
            Name = name;
        }

        #region 重写
        /// <summary>
        /// 重写ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            return Name;
        }

        /// <summary>
        /// 扩展枚举值相等
        /// </summary>
        /// <param name="obj">比较对象</param>
        /// <returns>是否相等扩展枚举值</returns>
        public override bool Equals(object obj) {
            var otherValue = (Enumeration)obj;
            if (otherValue == null) return false;

            var typeMatches = this.GetType().Equals(obj.GetType());
            var valueMatches = Id.Equals(otherValue.Id);

            return typeMatches && valueMatches;
        }

        /// <summary>
        /// 重写GetHashCode
        /// </summary>
        /// <returns>获得HashCode</returns>
        public override int GetHashCode() {
            return Id.GetHashCode();
        }
        #endregion

        /// <summary>
        /// 获得指定类型扩展枚举值集合
        /// </summary>
        /// <typeparam name="T">扩展枚举值类型</typeparam>
        /// <returns>指定类型扩展枚举值集合</returns>
        public static IEnumerable<T> GetAll<T>() where T : Enumeration {
            var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);
            return fields.Select(f => f.GetValue(null)).Cast<T>();
        }

        /// <summary>
        /// 扩展枚举值绝对差
        /// </summary>
        /// <param name="firstValue">第一个扩展枚举值</param>
        /// <param name="secondValue">第二个扩展枚举值</param>
        /// <returns>两个扩展枚举值绝对差</returns>
        public static int AbsoluteDifference(Enumeration firstValue, Enumeration secondValue) {
            var absoluteDifference = Math.Abs(firstValue.Id - secondValue.Id);
            return absoluteDifference;
        }

        /// <summary>
        /// 由值返回扩展枚举值
        /// </summary>
        /// <typeparam name="T">扩展枚举值类型</typeparam>
        /// <param name="value">值</param>
        /// <returns>扩展枚举值</returns>
        public static T FromValue<T>(int value) where T : Enumeration {
            var matchingItem = Parse<T, int>(value, "value", item => item.Id == value);
            return matchingItem;
        }

        /// <summary>
        /// 由显示名返回扩展枚举值
        /// </summary>
        /// <typeparam name="T">扩展枚举值类型</typeparam>
        /// <param name="displayName">显示名</param>
        /// <returns>扩展枚举值</returns>
        public static T FromDisplayName<T>(string displayName) where T : Enumeration {
            var matchingItem = Parse<T, string>(displayName, "display name", item => item.Name == displayName);
            return matchingItem;
        }

        /// <summary>
        /// 查找扩展枚举值
        /// </summary>
        /// <typeparam name="T">扩展枚举值类型</typeparam>
        /// <typeparam name="K">值类型</typeparam>
        /// <param name="value">值</param>
        /// <param name="description">描述</param>
        /// <param name="predicate">条件</param>
        /// <returns>扩展枚举值</returns>
        private static T Parse<T, K>(K value, string description, Func<T, bool> predicate) where T : Enumeration {
            var matchingItem = GetAll<T>().FirstOrDefault(predicate);
            if (matchingItem == null) {
                throw new InvalidOperationException($"'{value}' is not valid {description} in {typeof(T)}");
            }

            return matchingItem;
        }

        #region IComparable
        /// <summary>
        /// 扩展枚举值比较
        /// </summary>
        /// <param name="obj">被比较的扩展枚举值</param>
        /// <returns>比较结果</returns>
        public int CompareTo(object obj) {
            return Id.CompareTo(((Enumeration)obj).Id);
        }
        #endregion

    }
}
