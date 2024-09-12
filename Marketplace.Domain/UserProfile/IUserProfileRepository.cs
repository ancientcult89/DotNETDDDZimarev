using Marketplace.Domain.Shared;

namespace Marketplace.Domain.UserProfile
{
    public interface IUserProfileRepository
    {
        Task<bool> Exists(UserId id);

        Task<UserProfile> Load(UserId id);
        Task Add(UserProfile entity);
    }
}
