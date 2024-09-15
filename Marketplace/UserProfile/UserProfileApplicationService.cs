using Marketplace.Domain.Shared;
using Marketplace.Domain.UserProfile;
using Marketplace.Framework;

namespace Marketplace.UserProfile
{
    public class UserProfileApplicationService : IApplicationService
    {
        private readonly IAggregateStore _aggregateStore;
        private readonly CheckTextForProfanity _checkText;

        public UserProfileApplicationService(IAggregateStore aggregateStore, CheckTextForProfanity checkText)
        {
            _checkText = checkText;
            _aggregateStore = aggregateStore;
        }

        public async Task Handle(object command)
        {
            switch (command)
            {
                case Contracts.V1.RegisterUser cmd:
                    await HandleCreate(cmd);
                    break;

                case Contracts.V1.UpdateUserFullName cmd:
                    await HandleUpdate(cmd.UserId, profile => profile.UpdateFullName(FullName.FromString(cmd.FullName)));
                    break;
                case Contracts.V1.UpdateUserDisplayName cmd:
                    await HandleUpdate(cmd.UserId, profile => profile.UpdateDisplayName(DisplayName.FromString(cmd.DisplayName, _checkText)));
                    break;
                case Contracts.V1.UpdateUserProfilePhoto cmd:
                    await HandleUpdate(cmd.UserId, profile => profile.UpdateProfilePhoto(new Uri(cmd.PhotoUrl)));
                    break;
            }
        }

        private async Task HandleCreate(Contracts.V1.RegisterUser cmd)
        {
            if (await _aggregateStore.Exists<Domain.UserProfile.UserProfile, UserId>(new UserId(cmd.UserId)))
                throw new InvalidOperationException($"Entity with id {cmd.UserId} already exists");

            var userProfile = new Domain.UserProfile.UserProfile(
                new UserId(cmd.UserId),
                FullName.FromString(cmd.FullName),
                DisplayName.FromString(cmd.DisplayName, _checkText)
            );

            await _aggregateStore.Save<Domain.UserProfile.UserProfile, UserId>(userProfile);
        }

        private async Task HandleUpdate(Guid id, Action<Domain.UserProfile.UserProfile> update) =>
            this.HandleUpdate(_aggregateStore, new UserId(id), update);
    }
}