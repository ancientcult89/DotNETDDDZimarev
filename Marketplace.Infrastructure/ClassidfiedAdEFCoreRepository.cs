using Marketplace.Domain;

namespace Marketplace.Infrastructure
{
    public class ClassidfiedAdEFCoreRepository : IClassifiedAdRepository
    {
        private readonly ClassifiedAdDbContext _dbContext;
        public ClassidfiedAdEFCoreRepository(ClassifiedAdDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task Add(ClassifiedAd entity) => _dbContext.ClassifiedAds.Add(entity);


        public async Task<bool> Exists(ClassifiedAdId id) => await _dbContext.ClassifiedAds.FindAsync(id.Value) != null;

        public async Task<ClassifiedAd> Load(ClassifiedAdId id) => await _dbContext.ClassifiedAds.FindAsync(id.Value);
        private static string EntityId(ClassifiedAdId id) => $"CassifiedAd/{id}";
    }
}
