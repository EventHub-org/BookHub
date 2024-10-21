using BookHub.DAL.Entities;
using BookHub.DAL.Repositories.Interfaces;
using BookHub.DAL.DataAccess;

namespace BookHub.DAL.Repositories.Implementations
{
    public class AchievementRepository : Repository<AchievementEntity>, IAchievementRepository
    {
        private AppDbContext _context;

        public AchievementRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        
    }
}
