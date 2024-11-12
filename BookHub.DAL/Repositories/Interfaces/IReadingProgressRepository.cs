using BookHub.DAL.Entities;
using BookHub.DAL.Repositories.Interfaces;

public interface IReadingProgressRepository : IRepository<ReadingProgressEntity>
{
    Task<List<ReadingProgressEntity>> GetByUserIdAsync(int userId);
}
