using BookHub.DAL.DTO;

namespace BookHub.BLL.Services.Interfaces
{
    public interface IReviewService
    {
        Task<PageDto<ReviewDto>> GetPaginatedReviewsAsync(int pageNumber, int pageSize);
        Task DeleteReviewAsync(int id);
    }
}
