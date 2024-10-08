using BookHub.DAL.Entities;

namespace BookHub.DAL.Repositories.Interfaces
{
    public interface IAchievementRepository<TEntity> where TEntity : class
    {
        void AddAchievement(TEntity entity);
        void UpdateAchievement(TEntity entity);
        void DeleteAchievement(TEntity entity);
        AchievementEntity GetAchievementById(int id);
        IEnumerable<AchievementEntity> GetAllAchievements();
    }
}
