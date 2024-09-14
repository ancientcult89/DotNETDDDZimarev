using Marketplace.Domain.ClassifiedAd;

namespace Marketplace.Infrastructure
{
    public class ClassidfiedAdEFCoreRepository : IClassifiedAdRepository
    {
        private readonly MarketPlaceDbContext _dbContext;
        public ClassidfiedAdEFCoreRepository(MarketPlaceDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task Add(ClassifiedAd entity) => _dbContext.ClassifiedAds.Add(entity);


        public async Task<bool> Exists(ClassifiedAdId id) => await _dbContext.ClassifiedAds.FindAsync(id.Value) != null;

        public async Task<ClassifiedAd> Load(ClassifiedAdId id) => await _dbContext.ClassifiedAds.FindAsync(id.Value);
    }
}
