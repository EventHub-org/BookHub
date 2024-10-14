using BookHub.DAL.DTO;

namespace BookHub.BLL.Services.Interfaces
{
    public interface BookService
    {
        PageDto<BookDto> GetPaginatedBooks(int pageNumber, int pageSize);
    }
}
