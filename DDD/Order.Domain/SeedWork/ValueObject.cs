using System.Collections.Generic;
using System.Linq;

namespace Ordering.Domain.SeedWork {
    /// <summary>
    /// 值对象
    /// </summary>
    public abstract class ValueObject {
        /// <summary>
        /// 两个值对象是否相等
        /// </summary>
        /// <param name="left">左边值对象</param>
        /// <param name="right">右边值对象</param>
        /// <returns>是否相等</returns>
        protected static bool EqualOperator(ValueObject left, ValueObject right) {
            //针对整型类型和 bool 预定义了二元 ^ 运算符。 
            //对于整型类型，^ 会计算其操作数的按位异或。 
            //对于 bool 操作数，^ 计算其操作数的逻辑异或；即，当且仅当其一个操作数为 true 时，结果才为 true
            if (ReferenceEquals(left, null) ^ ReferenceEquals(right, null)) {
                return false;
            }

            return ReferenceEquals(left, null) || left.Equals(right);
        }

        /// <summary>
        /// 两个值对象是否不相等
        /// </summary>
        /// <param name="left">左边值对象</param>
        /// <param name="right">右边值对象</param>
        /// <returns></returns>
        protected static bool NotEqualOperator(ValueObject left, ValueObject right) {
            return !EqualOperator(left, right);
        }

        /// <summary>
        /// 获得原子值集合
        /// </summary>
        /// <returns>原子值集合</returns>
        protected abstract IEnumerable<object> GetAtomicValues();

        /// <summary>
        /// 重写Equal
        /// </summary>
        /// <param name="obj">比较对象</param>
        /// <returns>是否相等</returns>
        public override bool Equals(object obj) {
            //为NULL或不是值对象类型
            if (obj == null || obj.GetType() != this.GetType()) {
                return false;
            }

            var thisValues = this.GetAtomicValues().GetEnumerator();
            var otherValues = ((ValueObject)obj).GetAtomicValues().GetEnumerator();
            while (thisValues.MoveNext() && otherValues.MoveNext()) {
                if (ReferenceEquals(thisValues.Current, null) ^ ReferenceEquals(otherValues.Current, null)) {
                    return false;
                }
                if (thisValues.Current != null && !thisValues.Current.Equals(otherValues.Current)) {
                    return false;
                }
            }
            return !thisValues.MoveNext() && !otherValues.MoveNext();
        }

        /// <summary>
        /// 重写GetHasCode
        /// </summary>
        /// <returns>获得HashCode</returns>
        public override int GetHashCode() {
            return this.GetAtomicValues()
                .Select(x => x != null ? x.GetHashCode() : 0)
                .Aggregate((x, y) => x ^ y);
        }

        /// <summary>
        /// 获得当前值对象复制
        /// </summary>
        /// <returns>当前值对象副本</returns>
        public ValueObject GetCopy() {
            return this.MemberwiseClone() as ValueObject;
        }
    }
}
