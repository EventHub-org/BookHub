using BookHub.DAL.Entities;
using System.Linq.Expressions;

namespace BookHub.DAL.Repositories.Interfaces
{
    public interface ICollectionRepository : IRepository<CollectionEntity>
    {
        Task<IEnumerable<CollectionEntity>> GetAllAsync(Expression<Func<CollectionEntity, bool>> predicate);

    }
}
