using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DXQ.Study.IdentityServer4.AuthorizationServer.Quickstart.Account {
    /// <summary>
    /// 应用程序DbContext
    /// </summary>
    public class ApplicationDbContext : DbContext {
        /// <summary>
        /// 初始化应用程序DbContext
        /// </summary>
        /// <param name="options">选项</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {

        }

        /// <summary>
        /// 模型绑定
        /// </summary>
        /// <param name="builder"></param>
        protected override void OnModelCreating(ModelBuilder builder) {
            builder.Entity<ApplicationUser>().ToTable("User");
            base.OnModelCreating(builder);            
        }
    }
}
