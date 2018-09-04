using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Idempotency {
    /// <summary>
    /// 请求管理器接口
    /// </summary>
    public interface IRequestManager {
        /// <summary>
        /// 请求是否已存在
        /// </summary>
        /// <param name="id">请求Id</param>
        /// <returns>请求是否已存在</returns>
        Task<bool> ExistAsync(Guid id);

        /// <summary>
        /// 建立命令请求
        /// </summary>
        /// <typeparam name="T">请求类型</typeparam>
        /// <param name="id">请求Id</param>
        /// <returns>任务</returns>
        Task CreateRequestForCommandASync<T>(Guid id);
    }
}
