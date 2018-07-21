using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DXQ.Study.IdentityServer4.AuthorizationServer.Quickstart.Account {
    /// <summary>
    /// 用户存储接口
    /// </summary>
    public interface IUserRepository {
        /// <summary>
        /// 验证凭据
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>是否有效</returns>
        bool ValidateCredentials(string username, string password);

        /// <summary>
        /// 由用户ID获取用户信息
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>用户信息</returns>
        Task<ApplicationUser> FindByUseridAsync(Guid userId);

        /// <summary>
        /// 由用户名获得用户信息
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns>用户信息</returns>
        ApplicationUser FindByUsername(string userName);
    }
}
