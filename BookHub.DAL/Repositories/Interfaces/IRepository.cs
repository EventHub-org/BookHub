using System.Linq.Expressions;

namespace BookHub.DAL.Repositories.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task AddAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task<TEntity> GetByIdAsync(Expression<Func<TEntity, bool>> filter);
        Task<IEnumerable<TEntity>> GetAllAsync();
    }
}
