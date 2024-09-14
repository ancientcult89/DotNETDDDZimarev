namespace Marketplace.Framework
{
    public interface IAggregateStore
    {
        Task<bool> Exists<T, TId>(TId aggrefateId);
        Task Save<T, TId>(T aggregate) where T : AggregateRoot<TId>;
        Task Load<T, TId>(TId aggregateId) where T : AggregateRoot<TId>;
    }
}
