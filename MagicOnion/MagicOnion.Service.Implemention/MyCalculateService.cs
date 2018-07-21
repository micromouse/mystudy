using MagicOnion.Server;
using MagicOnion.Service.Definition;
using System.Threading.Tasks;

namespace MagicOnion.Service.Implemention {
    /// <summary>
    /// 计算服务实现
    /// </summary>
    public class MyCalculateService : ServiceBase<ICalculateService>, ICalculateService {
        /// <summary>
        /// 任务方式计算x+y
        /// </summary>
        /// <param name="x">x</param>
        /// <param name="y">y</param>
        /// <returns>x+y</returns>
        public Task<UnaryResult<string>> SumAsync(int x, int y) {
            return Task.FromResult<UnaryResult<string>>(UnaryResult((x + y).ToString()));
        }

        /// <summary>
        /// 计算x+y
        /// </summary>
        /// <param name="x">x</param>
        /// <param name="y">y</param>
        /// <returns>x+y</returns>
        public UnaryResult<string> SumAsync2(int x, int y) {
            return UnaryResult((x + y).ToString());
        }

        /// <summary>
        /// 获得消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <returns>消息</returns>
        public UnaryResult<Message> GetMessage(Message message) {
            message.CreatedBy = "dxq";
            return UnaryResult(message);
        }
    }
}
