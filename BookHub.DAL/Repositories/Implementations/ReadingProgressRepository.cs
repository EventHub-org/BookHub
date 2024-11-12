using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BookHub.DAL.Entities;
using BookHub.DAL.Repositories.Interfaces;
using BookHub.DAL.DataAccess;

namespace BookHub.DAL.Repositories.Implementations
{
    public class ReadingProgressRepository : Repository<ReadingProgressEntity>, IReadingProgressRepository
    {
        private readonly AppDbContext _context;

        public ReadingProgressRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<ReadingProgressEntity>> GetByUserIdAsync(int userId)
        {
            return await _context.ReadingProgresses
                                 .Where(rp => rp.UserId == userId)
                                 .ToListAsync();
        }
    }
}
