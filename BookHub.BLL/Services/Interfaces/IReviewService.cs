using BookHub.BLL.Services.Implementations;
using BookHub.DAL.DTO;

namespace BookHub.BLL.Services.Interfaces
{
    public interface IReviewService
    {
        Task<ServiceResult> GetPaginatedReviewsAsync(int pageNumber, int pageSize);
        Task DeleteReviewAsync(int id);
    }
}
