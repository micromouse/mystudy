using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DXQ.Study.IdentityServer4.AuthorizationServer.Quickstart.Account {
    /// <summary>
    /// 应用程序用户
    /// </summary>
    public class ApplicationUser {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Status { get; set; }
        public int RegisterDate { get; set; }
        public int? ValidityDate { get; set; }
        public string MacAddress { get; set; }
        public int IsOnline { get; set; }
        public string Token { get; set; }
        public int? LastOperateTime { get; set; }
    }
}
