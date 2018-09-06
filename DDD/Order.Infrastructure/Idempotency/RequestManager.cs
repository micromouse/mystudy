using Ordering.Domain.Exceptions;
using System;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Idempotency {
    /// <summary>
    /// 请求管理器
    /// </summary>
    public class RequestManager : IRequestManager {
        private readonly OrderingDbContext _context;

        /// <summary>
        /// 初始化请求管理器
        /// </summary>
        /// <param name="context">订单DbContext</param>
        public RequestManager(OrderingDbContext context) {
            _context = context;
        }

        /// <summary>
        /// 请求是否已存在
        /// </summary>
        /// <param name="id">请求Id</param>
        /// <returns>请求是否已存在</returns>
        public async Task CreateRequestForCommandASync<T>(Guid id) {
            var exists = await this.ExistAsync(id);

            var request = exists ?
                throw new OrderingDomainException($"Request with {id} already exists") :
                new ClientRequest {
                    Id = id,
                    Name = typeof(T).Name,
                    Time = DateTime.Now
                };
            _context.Add(request);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// 建立命令请求
        /// </summary>
        /// <typeparam name="T">请求类型</typeparam>
        /// <param name="id">请求Id</param>
        /// <returns>任务</returns>
        public async Task<bool> ExistAsync(Guid id) {
            var request = await _context.FindAsync<ClientRequest>(id);
            return request != null;
        }
    }
}
