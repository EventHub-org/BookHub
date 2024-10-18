using BookHub.DAL.Entities;

namespace BookHub.DAL.Repositories.Interfaces
{
    public interface IBookRepository : IRepository<BookEntity>
    {
        Task UpdateAsync(BookEntity obj);
        Task SaveAsync();
        Task<(List<BookEntity> Items, long TotalCount)> GetPagedAsync(int pageSize, int pageNumber);

    }
}
