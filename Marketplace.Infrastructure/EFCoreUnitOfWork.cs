using Marketplace.Framework;

namespace Marketplace.Infrastructure
{
    public class EFCoreUnitOfWork : IUnitOfWork
    {
        private readonly MarketPlaceDbContext _dbContext;

        public EFCoreUnitOfWork(MarketPlaceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task Commit() => _dbContext.SaveChangesAsync();        
    }
}
