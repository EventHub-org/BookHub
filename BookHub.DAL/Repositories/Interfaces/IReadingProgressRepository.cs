using BookHub.DAL.Entities;

namespace BookHub.DAL.Repositories.Interfaces
{
    public interface IReadingProgressRepository : IRepository<ReadingProgressEntity>
    {
        Task UpdateAsync(ReadingProgressEntity obj);
        Task SaveAsync();
    }
}
