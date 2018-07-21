using System;
using System.Threading.Tasks;

namespace MagicOnion.Service.Definition {
    /// <summary>
    /// MagicOnion计算服务
    /// </summary>
    public interface ICalculateService : IService<ICalculateService> {
        /// <summary>
        /// 任务方式计算x+y
        /// </summary>
        /// <param name="x">x</param>
        /// <param name="y">y</param>
        /// <returns>x+y</returns>
        Task<UnaryResult<string>> SumAsync(int x, int y);

        /// <summary>
        /// 计算x+y
        /// </summary>
        /// <param name="x">x</param>
        /// <param name="y">y</param>
        /// <returns>x+y</returns>
        UnaryResult<string> SumAsync2(int x, int y);

        /// <summary>
        /// 获得消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <returns>消息</returns>
        UnaryResult<Message> GetMessage(Message message);

        
    }
}
