using BookHub.DAL.DTO;

namespace BookHub.BLL.Services.Interfaces
{
    public interface IBookService
    {
        Task<BookDto> GetBookAsync(int id);
        Task<PageDto<BookDto>> GetPaginatedBooksAsync(int pageNumber, int pageSize);
    }
}
