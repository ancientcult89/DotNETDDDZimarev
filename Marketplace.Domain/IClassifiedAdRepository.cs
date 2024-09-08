namespace Marketplace.Domain
{
    public interface IClassifiedAdRepository
    {
        Task<bool> Exists(string id);

        Task<ClassifiedAd> Load(string id);

        Task Save(ClassifiedAd entity);
    }
}
