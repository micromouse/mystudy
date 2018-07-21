using Microsoft.Extensions.DependencyInjection;
using System;

namespace NCache.Custom {
    /// <summary>
    /// Redis分布式缓存
    /// </summary>
    public static class RedisCache {
        /// <summary>
        /// 添加Redis分布式缓存
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        public static IServiceCollection AddDistributedCacheRedis(this IServiceCollection services) {
            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = "127.0.0.1,password=redis";
                options.InstanceName = "master";
            });
            return services;
        }
    }
}
