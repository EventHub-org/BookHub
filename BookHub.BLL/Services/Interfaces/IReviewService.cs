using BookHub.DAL.DTO;

namespace BookHub.BLL.Services.Interfaces
{
    public interface IReviewService
    {
        PageDto<ReviewDto> GetPaginatedReviews(int pageNumber, int pageSize);
    }
}
