using BookHub.DAL.DTO;

namespace BookHub.BLL.Services.Interfaces
{
    public interface IBookService
    {
        PageDto<BookDto> GetPaginatedBooks(int pageNumber, int pageSize);
    }
}
