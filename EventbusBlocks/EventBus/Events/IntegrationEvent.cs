using System;
using System.Collections.Generic;
using System.Text;

namespace DXQ.Study.EventbusBlocks.EventBus.Events {
    /// <summary>
    /// 集成事件
    /// </summary>
    public class IntegrationEvent {
        /// <summary>
        /// 初始化集成事件
        /// </summary>
        public IntegrationEvent() {
            this.Id = Guid.NewGuid();
            this.CreateTime = DateTime.Now;
        }

        /// <summary>
        /// 事件ID,缺省为系统自动生成的GUID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 事件建立时间,缺省为当前时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
