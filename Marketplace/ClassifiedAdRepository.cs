using Marketplace.Domain;

namespace Marketplace
{
    public class ClassifiedAdRepository
        : IClassifiedAdRepository, IDisposable
    {

        public ClassifiedAdRepository()
        {

        }

        public Task<bool> Exists(string id)
            => throw new NotImplementedException();

        public Task<ClassifiedAd> Load(string id)
            => throw new NotImplementedException();

        public async Task Save(ClassifiedAd entity)
        {
            throw new NotImplementedException();
        }

        public void Dispose() => throw new NotImplementedException();

        private static string EntityId(ClassifiedAdId id)
            => $"ClassifiedAd/{id}";
    }

    internal interface IAsyncDocumentSession
    {
    }
}
