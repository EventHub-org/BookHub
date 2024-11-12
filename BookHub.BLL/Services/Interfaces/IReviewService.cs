using BookHub.DAL.DTO;
using BookHub.BLL.Utils;

namespace BookHub.BLL.Services.Interfaces
{
    public interface IReviewService
    {
        Task<ServiceResultType<ReviewDto>> GetReviewAsync(int id);
        Task<ServiceResultType<PageDto<ReviewDto>>> GetPaginatedReviewsAsync(Pageable pageable);
        Task<ServiceResultType<PageDto<ReviewDto>>> GetPaginatedReviewsByBookIdAsync(int bookId, Pageable pageable);
        Task<ServiceResultType> DeleteReviewAsync(int id);
        Task<ServiceResultType<ReviewDto>> CreateReviewAsync(ReviewDto reviewDto);
    }
}
