using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickStart.Models {
    /// <summary>
    /// 我的消息集成事件
    /// </summary>
    public class MyMessageIntegrationEvent {
        /// <summary>
        /// 初始化我的消息集成事件
        /// </summary>
        /// <param name="createdBy">用户</param>
        /// <param name="createTime">时间</param>
        public MyMessageIntegrationEvent(string createdBy,DateTime createTime) {
            this.CreatedBy = createdBy;
            this.CreateTime = createTime;
        }

        /// <summary>
        /// 用户
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
