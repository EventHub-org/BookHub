using BookHub.DAL.Entities;

namespace BookHub.DAL.Repositories.Interfaces
{
    public interface ICollectionRepository<TEntity> where TEntity : class
    {
        void AddCollection(TEntity entity);
        void UpdateCollection(TEntity entity);
        void DeleteCollection(TEntity entity);
        CollectionEntity GetCollectionById(int id);
        IEnumerable<CollectionEntity> GetAllCollections();
    }
}
