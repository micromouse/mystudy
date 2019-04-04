using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Ordering.Domain.AggregateModel.BuyerAggregate;
using Ordering.Domain.SeedWork;
using System;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Repositories
{
    /// <summary>
    /// 买家仓储
    /// </summary>
    public class BuyerRepository : IBuyerRepository
    {
        private readonly OrderingDbContext _context;

        /// <summary>
        /// 工作单元
        /// </summary>
        public IUnitOfWork UnitOfWork => _context;

        /// <summary>
        /// 初始化买家仓储
        /// </summary>
        /// <param name="context">订单DbContext</param>
        public BuyerRepository(OrderingDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// 添加买家
        /// </summary>
        /// <param name="buyer">买家</param>
        /// <returns>新添加的买家</returns>
        public Buyer Add(Buyer buyer)
        {
            if (buyer.IsTransient())
            {
                return _context.Buyers
                    .Add(buyer)
                    .Entity;
            }
            else
            {
                return buyer;
            }
        }

        /// <summary>
        /// 更新买家
        /// </summary>
        /// <param name="buyer">买家</param>
        /// <returns>买家</returns>
        public async Task<Buyer> FindAsync(string buyerIdentityGuid)
        {
            var sql = "SELECT * FROM ordering.buyers a WHERE identityguid=@buyerIdentityGuid AND EXISTS(SELECT TOP 1 1 FROM ordering.paymentmethods WHERE buyerid=a.id)";
            var buyer = await _context.Database.GetDbConnection().QuerySingleOrDefaultAsync<Buyer>(sql, new { buyerIdentityGuid }, _context.CurrentTransaction.GetDbTransaction());
            return buyer;
        }

        /// <summary>
        /// 查找买家
        /// </summary>
        /// <param name="buyerIdentityGuid">买家标识</param>
        /// <returns>买家</returns>
        public async Task<Buyer> FindByIdAsync(string id)
        {
            var sql = "SELECT * FROM ordering.buyers a WHERE id=@id AND EXISTS(SELECT TOP 1 1 FROM ordering.paymentmethods WHERE buyerid=a.id)";
            var buyer = await _context.Database.GetDbConnection().QuerySingleOrDefaultAsync<Buyer>(sql, new { id });
            return buyer;
        }

        /// <summary>
        /// 查找买家
        /// </summary>
        /// <param name="id">买家Id</param>
        /// <returns>买家</returns>
        public Buyer Update(Buyer buyer)
        {
            return _context.Buyers
                    .Update(buyer)
                    .Entity;
        }
    }
}
