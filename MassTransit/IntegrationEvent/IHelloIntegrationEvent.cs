using System;

namespace IntegrationEvent {
    /// <summary>
    /// Hello集成事件
    /// </summary>
    public interface IHelloIntegrationEvent {
        /// <summary>
        /// 消息文本
        /// </summary>
        string Text { get; }
    }
}
