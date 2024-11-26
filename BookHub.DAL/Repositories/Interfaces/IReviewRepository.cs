using BookHub.DAL.Entities;
using BookHub.DAL.DTO;
using System.Linq.Expressions;

namespace BookHub.DAL.Repositories.Interfaces
{
    public interface IReviewRepository : IRepository<ReviewEntity>
    {
        Task<(List<ReviewEntity> Items, long TotalCount)> GetPagedAsync(Pageable pageable);
        Task<IEnumerable<ReviewEntity>> GetAllAsync(Expression<Func<ReviewEntity, bool>> predicate);
    }
}
