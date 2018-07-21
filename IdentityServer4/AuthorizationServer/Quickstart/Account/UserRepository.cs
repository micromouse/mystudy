using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DXQ.Study.IdentityServer4.AuthorizationServer.Quickstart.Account {
    /// <summary>
    /// 用户仓储
    /// </summary>
    public class UserRepository : IUserRepository {
        private ApplicationDbContext dbContext = null;

        /// <summary>
        /// 初始化用户仓储
        /// </summary>
        public UserRepository(ApplicationDbContext context) {
            this.dbContext = context;
        }

        /// <summary>
        /// 验证凭据
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>是否有效</returns>
        public bool ValidateCredentials(string username, string password) {
            var user = FindByUsername(username);
            return user?.Password == this.GetMD5HashData(password);
        }

        /// <summary>
        /// 由用户ID获取用户信息
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>用户信息</returns>
        public Task<ApplicationUser> FindByUseridAsync(Guid userId) {
            return Task.FromResult<ApplicationUser>(dbContext.Set<ApplicationUser>().SingleOrDefault(x => x.Id == userId));
        }

        /// <summary>
        /// 由用户名获得用户信息
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns>用户信息</returns>
        public ApplicationUser FindByUsername(string userName) {
            return dbContext.Set<ApplicationUser>().SingleOrDefault(x => x.UserName == userName);
        }

        /// <summary>
        /// take any string and encrypt it using MD5 then return the encrypted data 
        /// </summary>
        /// <param name="data">input text you will enterd to encrypt it</param>
        /// <returns>return the encrypted text as hexadecimal string</returns>
        private string GetMD5HashData(string data) {
            using (var md5 = MD5.Create()) {
                var result = md5.ComputeHash(Encoding.UTF8.GetBytes(data));
                return BitConverter.ToString(result).Replace("-", "");
            }

            //return string.Join("", MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(data)).Select(s => s.ToString("x2")));
        }
    }
}
