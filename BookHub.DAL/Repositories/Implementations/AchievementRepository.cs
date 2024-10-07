using BookHub.DAL.Entities;
using BookHub.DAL.Repositories.Interfaces;
using BookHub.DAL.DataAccess;

namespace BookHub.DAL.Repositories.Implementations
{
    public class AchievementRepository : IAchievementRepository<AchievementEntity>
    {
        private readonly AppDbContext _context;

        public AchievementRepository(AppDbContext context)
        {
            _context = context;
        }

        public void AddAchievement(AchievementEntity entity)
        {
            _context.Achievements.Add(entity);
            _context.SaveChanges();
        }

        public void UpdateAchievement(AchievementEntity entity)
        {
            _context.Achievements.Update(entity);
            _context.SaveChanges();
        }

        public void DeleteAchievement(AchievementEntity entity)
        {
            _context.Achievements.Remove(entity);
            _context.SaveChanges();
        }

        public AchievementEntity GetAchievementById(int id)
        {
            return _context.Achievements.Find(id);
        }

        public IEnumerable<AchievementEntity> GetAllAchievements()
        {
            return _context.Achievements.ToList();
        }
    }
}
