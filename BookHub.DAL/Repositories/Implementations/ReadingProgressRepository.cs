using BookHub.DAL.Entities;
using BookHub.DAL.Repositories.Interfaces;
using BookHub.DAL.DataAccess;

namespace BookHub.DAL.Repositories.Implementations
{
    public class ReadingProgressRepository : Repository<ReadingProgressEntity>, IReadingProgressRepository
    {
        private AppDbContext _context;

        public ReadingProgressRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        
    }
}
