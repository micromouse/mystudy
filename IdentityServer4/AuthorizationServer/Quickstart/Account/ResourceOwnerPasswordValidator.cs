using IdentityModel;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DXQ.Study.IdentityServer4.AuthorizationServer.Quickstart.Account {
    /// <summary>
    /// Handles validation of resource owner password credentials
    /// 自定义处理资源所有者密码凭证验证
    /// </summary>
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator {
        private ISystemClock clock = null;
        private IUserRepository userRepository = null;

        /// <summary>
        /// 初始化自定义处理资源所有者密码凭证验证
        /// </summary>
        /// <param name="userRepository">用户仓储</param>
        public ResourceOwnerPasswordValidator(IUserRepository userRepository, ISystemClock clock) {
            this.userRepository = userRepository;
            this.clock = clock;
        }

        /// <summary>
        /// Validates the resource owner password credential
        /// </summary>
        /// <param name="context"> Class describing the resource owner password validation context(描述资源所有者密码验证上下文的类)</param>
        /// <returns>开启一个任务</returns>
        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context) {
            if (userRepository.ValidateCredentials(context.UserName, context.Password)) {
                var user = userRepository.FindByUsername(context.UserName);
                context.Result = new GrantValidationResult(
                    subject: user.Id.ToString(),
                    authenticationMethod: OidcConstants.AuthenticationMethods.Password,
                    authTime: clock.UtcNow.UtcDateTime);
            }

            return Task.FromResult(0);
        }
    }
}
