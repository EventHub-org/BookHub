using BookHub.DAL.DTO;
using BookHub.BLL.Utils;

namespace BookHub.BLL.Services.Interfaces
{
    public interface IBookService
    {
        Task<ServiceResultType<BookDto>> GetBookAsync(int id);
        Task<ServiceResultType<PageDto<BookDto>>> GetPaginatedBooksAsync(Pageable pageable);
        Task<ServiceResultType<PageDto<BookDto>>> GetPaginatedBooksFromCollectionAsync(int collectionId,Pageable pageable);
        Task<ServiceResultType<BookDto>> CreateBook(BookCreateDto bookCreateDto);
        Task<ServiceResultType> DeleteBookAsync(int id);
    }
}
