using BookHub.DAL.Entities;

namespace BookHub.DAL.Repositories.Interfaces
{
    public interface IAchievementRepository : IRepository<AchievementEntity>
    {
        Task UpdateAsync(AchievementEntity obj);
        Task SaveAsync();
    }
}
