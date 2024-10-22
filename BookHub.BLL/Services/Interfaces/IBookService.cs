using BookHub.DAL.DTO;
using BookHub.BLL.Utils;

namespace BookHub.BLL.Services.Interfaces
{
    public interface IBookService
    {
        Task<ServiceResultType<BookDto>> GetBookAsync(int id);
        Task<ServiceResultType<PageDto<BookDto>>> GetPaginatedBooksAsync(int pageNumber, int pageSize);
        Task<ServiceResultType> DeleteBookAsync(int id);
    }
}
