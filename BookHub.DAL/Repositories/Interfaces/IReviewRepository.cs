using BookHub.DAL.Entities;

namespace BookHub.DAL.Repositories.Interfaces
{
    public interface IReviewRepository : IRepository<ReviewEntity>
    {
        Task<(List<ReviewEntity> Items, long TotalCount)> GetPagedAsync(int pageSize, int pageNumber);
    }
}
