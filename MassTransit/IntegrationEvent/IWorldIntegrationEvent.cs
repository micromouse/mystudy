using System;
using System.Collections.Generic;
using System.Text;

namespace IntegrationEvent {
    /// <summary>
    /// World集成事件
    /// </summary>
    public interface IWorldIntegrationEvent {
        /// <summary>
        /// 创建者
        /// </summary>
        string Createby { get; }
    }
}
