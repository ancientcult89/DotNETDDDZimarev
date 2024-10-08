﻿using Marketplace.Domain.Shared;
using Marketplace.Domain.UserProfile;
using Marketplace.Infrastructure;
using Raven.Client.Documents.Session;

namespace Marketplace.UserProfile
{
    public class UserProfileRavedDbRepository : RavenDbRepository<Domain.UserProfile.UserProfile, UserId>, IUserProfileRepository
    {
        public UserProfileRavedDbRepository(IAsyncDocumentSession session) : base(session, id => $"UserProfile/{id.Value}")
        {
        }

        public Task<bool> Exists(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Domain.UserProfile.UserProfile> Load(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
