using BookHub.DAL.DTO;

namespace BookHub.BLL.Services.Interfaces
{
    public interface IBookService
    {
        Task<PageDto<BookDto>> GetPaginatedBooksAsync(int pageNumber, int pageSize);
    }
}
