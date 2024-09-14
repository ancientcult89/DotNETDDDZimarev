using Marketplace.Domain.UserProfile;
using Marketplace.Infrastructure;

namespace Marketplace.UserProfile
{
    public class UserProfileEfRepository : IUserProfileRepository, IDisposable
    {
        private readonly MarketPlaceDbContext _dbContext;
        public UserProfileEfRepository(MarketPlaceDbContext dbContext) { _dbContext = dbContext; }
        public async Task Add(Domain.UserProfile.UserProfile entity) => _dbContext.UserProfiles.AddAsync(entity);
        public async Task<bool> Exists(Guid id) => await _dbContext.UserProfiles.FindAsync(id) != null;
        public async Task<Domain.UserProfile.UserProfile> Load(Guid id) => await _dbContext.UserProfiles.FindAsync(id);
        public void Dispose() => _dbContext.Dispose();
    }
}
