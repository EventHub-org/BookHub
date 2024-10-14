using BookHub.DAL.Entities;

namespace BookHub.DAL.Repositories.Interfaces
{
    public interface IReviewRepository<TEntity> where TEntity : class
    {
        Task<(List<ReviewEntity> Items, long TotalCount)> GetPagedAsync(int pageSize, int pageNumber);
    }
}
