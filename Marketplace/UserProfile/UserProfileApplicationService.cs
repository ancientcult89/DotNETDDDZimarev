using Marketplace.Domain.Shared;
using Marketplace.Domain.UserProfile;
using Marketplace.Framework;

namespace Marketplace.UserProfile
{
    public class UserProfileApplicationService : IApplicationService
    {
        private readonly IUserProfileRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly CheckTextForProfanity _checkText;

        public UserProfileApplicationService(IUserProfileRepository repository, IUnitOfWork unitOfWork, CheckTextForProfanity checkText)
        {
            _checkText = checkText;
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(object command)
        {
            switch (command)
            {
                case Contracts.V1.RegisterUser cmd:
                    if (await _repository.Exists(cmd.UserId))
                        throw new InvalidOperationException($"Entity with id {cmd.UserId} already exists");

                    var userProfile = new Domain.UserProfile.UserProfile(
                        new UserId(cmd.UserId),
                        FullName.FromString(cmd.FullName),
                        DisplayName.FromString(cmd.DisplayName, _checkText)
                    );

                    await _repository.Add(userProfile);
                    await _unitOfWork.Commit();
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

        private async Task HandleUpdate(Guid userProfileId, Action<Domain.UserProfile.UserProfile> operation)
        {
            var classifiedId = await _repository.Load(userProfileId);
            if (classifiedId == null)
                throw new InvalidOperationException($"Entity with id {userProfileId} cannot be found");

            operation( classifiedId);
            await _unitOfWork.Commit();
        }
    }
}
