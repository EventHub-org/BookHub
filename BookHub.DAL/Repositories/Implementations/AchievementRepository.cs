using BookHub.DAL.Entities;
using BookHub.DAL.Repositories.Interfaces;
using BookHub.DAL.DataAccess;

namespace BookHub.DAL.Repositories.Implementations
{
    public class AchievementRepository : IRepository<AchievementEntity>, IAchievementRepository<AchievementEntity>
    {
        private readonly AppDbContext _context;

        public AchievementRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(AchievementEntity entity)
        {
            _context.Achievements.Add(entity);
            _context.SaveChanges();
        }

        public async Task UpdateAsync(AchievementEntity entity)
        {
            _context.Achievements.Update(entity);
            _context.SaveChanges();
        }

        public async Task DeleteAsync(AchievementEntity entity)
        {
            _context.Achievements.Remove(entity);
            _context.SaveChanges();
        }

        public async Task<AchievementEntity> GetByIdAsync(int id)
        {
            return _context.Achievements.Find(id);
        }

        public async Task<IEnumerable<AchievementEntity>> GetAllAsync()
        {
            return _context.Achievements.ToList();
        }
    }
}
