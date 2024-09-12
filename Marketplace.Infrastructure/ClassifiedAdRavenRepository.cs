using Marketplace.Domain.ClassifiedAd;
using Raven.Client.Documents.Session;

namespace Marketplace.Infrastructure
{
    public class ClassifiedAdRavenRepository : IClassifiedAdRepository
    {
        private readonly IAsyncDocumentSession _session;
        public ClassifiedAdRavenRepository(IAsyncDocumentSession session)
        {
            _session = session;
        }
        public Task Add(ClassifiedAd entity) => _session.StoreAsync(entity, EntityId(entity.Id));


        public Task<bool> Exists(ClassifiedAdId id) => _session.Advanced.ExistsAsync(EntityId(id));

        public Task<ClassifiedAd> Load(ClassifiedAdId id) => _session.LoadAsync<ClassifiedAd>(EntityId(id));
        private static string EntityId(ClassifiedAdId id) => $"CassifiedAd/{id}";
    }
}
