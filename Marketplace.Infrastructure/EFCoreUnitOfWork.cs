using Marketplace.Framework;

namespace Marketplace.Infrastructure
{
    public class EFCoreUnitOfWork : IUnitOfWork
    {
        private readonly ClassifiedAdDbContext _dbContext;

        public EFCoreUnitOfWork(ClassifiedAdDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task Commit() => _dbContext.SaveChangesAsync();        
    }
}
