using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DXQ.Study.IdentityServer4.AuthorizationServer.Quickstart.Account {
    /// <summary>
    /// This interface allows IdentityServer to connect to your user and profile store.
    /// 这个接口允许IdentityServer连接到你的用户和概要存储
    /// </summary>
    public class ProfileService : IProfileService {
        private IUserRepository userRepository = null;

        /// <summary>
        /// 初始化IProfileService接口实现
        /// </summary>
        /// <param name="userRepository">用户仓储</param>
        public ProfileService(IUserRepository userRepository) {
            this.userRepository = userRepository;
        }

        /// <summary>
        /// Get user profile data in terms of claims when calling /connect/userinfo
        /// </summary>
        /// <param name="context">user profiledata request context</param>
        /// <returns>开启一个任务</returns>
        public async Task GetProfileDataAsync(ProfileDataRequestContext context) {
            var userId = context.Subject.GetSubjectId();

            var user = await userRepository.FindByUseridAsync(Guid.Parse(userId));
            var claims = new List<Claim> {
                new Claim("Username", user.UserName),
                new Claim("Status", user.Status.ToString()),
                new Claim("Registerdate", user.RegisterDate.ToString()),
                new Claim("Token", user.Token),
                new Claim("Email", "eggeggak@vip.qq.com"),
                new Claim("Role", "superadmin")
            };
            
            //值得注意的是如果我们直接将用户的所有Claim加入 context.IssuedClaims集合，那么用户所有的Claim都将会无差别返回给请求方。
            //比如默认情况下请求用户终结点(http://Identityserver4地址/connect/userinfo)只会返回sub（用户唯一标识）信息，
            //如果我们在此处直接 context.IssuedClaims=User.Claims，那么所有Claim都将被返回，而不会根据请求的Claim来进行筛选，
            //这样做虽然省事，但是损失了我们精确控制的能力，所以不推荐。
            //context.IssuedClaims = claims;

            //调用此方法以后内部会进行过滤，只将用户请求的Claim加入到集合中,这样我们的请求方便能正常获取到所需Claim
            context.AddRequestedClaims(claims);
        }

        /// <summary>
        /// check if user account is active.
        /// </summary>
        /// <param name="context">Context describing the is-active check</param>
        /// <returns>开启一个任务</returns>
        public async Task IsActiveAsync(IsActiveContext context) {
            var userId = context.Subject.GetSubjectId();
            context.IsActive = await userRepository.FindByUseridAsync(Guid.Parse(userId)) != null;
        }
    }
}
