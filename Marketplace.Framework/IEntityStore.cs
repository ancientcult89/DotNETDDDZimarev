namespace Marketplace.Framework
{
    public interface IEntityStore
    {
        /// <summary>
        /// loads an entity by id
        /// </summary>
        Task<T> Load<T>(string entityId) where T : Entity;
        /// <summary>
        /// Persists an entity
        /// </summary>
        Task Save<T>(T entity) where T : Entity;
        /// <summary>
        /// Check if entity with a given id already exists
        /// </summary>
        Task<bool> Exists<T>(string entityId);
    }
}
