using Marketplace.Domain.Shared;

namespace Marketplace.Domain.UserProfile
{
    public interface IUserProfileRepository
    {
        Task<bool> Exists(Guid id);

        Task<UserProfile> Load(Guid id);
        Task Add(UserProfile entity);
    }
}
