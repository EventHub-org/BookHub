using BookHub.DAL.DTO;

namespace BookHub.BLL.Services.Interfaces
{
    public interface ReviewService
    {
        PageDto<ReviewDto> GetPaginatedReviews(int pageNumber, int pageSize);
    }
}
