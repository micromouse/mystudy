using MediatR;
using System;
using System.Collections.Generic;

namespace Ordering.Domain.SeedWork {
    /// <summary>
    /// 实体基类
    /// </summary>
    public abstract class Entity {
        private int? hasCode;
        private List<INotification> domainEvents;

        #region 属性
        /// <summary>
        /// Id
        /// </summary>
        public virtual int Id { get; protected set; }

        /// <summary>
        /// 领域事件集合
        /// </summary>
        public IReadOnlyCollection<INotification> DomainEvents => domainEvents?.AsReadOnly();
        #endregion

        #region 领域事件管理
        /// <summary>
        /// 添加领域事件
        /// </summary>
        /// <param name="eventItem">事件项</param>
        public void AddDomainEvent(INotification eventItem) {
            domainEvents = domainEvents ?? new List<INotification>();
            domainEvents.Add(eventItem);
        }

        /// <summary>
        /// 删除领域事件
        /// </summary>
        /// <param name="eventItem">事件项</param>
        public void RemoveDomainEvent(INotification eventItem) {
            domainEvents?.Remove(eventItem);
        }

        /// <summary>
        /// 清空所有领域事件
        /// </summary>
        public void ClearDomainEvents() {
            domainEvents?.Clear();
        }
        #endregion

        /// <summary>
        /// 实体是否未初始化
        /// </summary>
        /// <returns>实体是否未初始化</returns>
        public bool IsTransient() {
            return this.Id == default(Int32);
        }

        #region 重载Equals/GetHasCode
        /// <summary>
        /// 重写Equals
        /// </summary>
        /// <param name="obj">被比较的实体</param>
        /// <returns>实体是否相等</returns>
        public override bool Equals(object obj) {
            if (obj == null || !(obj is Entity)) return false;      //obj为NULL或不是实体
            if (this.GetType() != obj.GetType()) return false;      //obj类型不是实体类型
            if (object.ReferenceEquals(this, obj)) return true;     //obj和当前实体是同一个引用

            var item = (Entity)obj;
            if (item.IsTransient() || this.IsTransient()) {
                return false;
            } else {
                return this.Id == item.Id;
            }
        }

        /// <summary>
        /// 获得HashCode
        /// </summary>
        /// <returns>HashCode</returns>
        public override int GetHashCode() {
            if (!this.IsTransient()) {
                if (hasCode == null) hasCode = this.Id.GetHashCode() ^ 31;
                return hasCode.Value;
            } else {
                return base.GetHashCode();
            }
        }
        #endregion

        #region ==/!=运算符重载
        /// <summary>
        /// 实体等于比较
        /// </summary>
        /// <param name="left">左实体</param>
        /// <param name="right">右实体</param>
        /// <returns>两个实体是否相等</returns>
        public static bool operator ==(Entity left, Entity right) {
            if (object.Equals(left, null)) {
                return object.Equals(right, null) ? true : false;
            } else {
                return left.Equals(right);
            }
        }

        /// <summary>
        /// 实体不等于比较
        /// </summary>
        /// <param name="left">左实体</param>
        /// <param name="right">右实体</param>
        /// <returns>两个是否不相等</returns>
        public static bool operator !=(Entity left, Entity right) {
            return !(left == right);
        }
        #endregion

    }
}
