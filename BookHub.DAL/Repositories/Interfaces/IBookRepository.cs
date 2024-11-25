using Azure;
using BookHub.DAL.Entities;
using BookHub.DAL.DTO;

namespace BookHub.DAL.Repositories.Interfaces
{
    public interface IBookRepository : IRepository<BookEntity>
    {
        Task<(List<BookEntity> Items, long TotalCount)> GetPagedAsync(Pageable pageable);
        Task<(List<BookEntity> Items, long TotalCount)> GetPagedBooksByCollectionAsync(int collectionId, Pageable pageable);

    }
}
