using BookHub.DAL.Entities;
using BookHub.DAL.DTO;

namespace BookHub.DAL.Repositories.Interfaces
{
    public interface IReviewRepository : IRepository<ReviewEntity>
    {
        Task<(List<ReviewEntity> Items, long TotalCount)> GetPagedAsync(Pageable pageable);
    }
}
