using BookHub.DAL.Entities;

namespace BookHub.DAL.Repositories.Interfaces
{
    public interface IBookRepository : IRepository<BookEntity>
    {
        Task<(List<BookEntity> Items, long TotalCount)> GetPagedAsync(int pageSize, int pageNumber);

    }
}
