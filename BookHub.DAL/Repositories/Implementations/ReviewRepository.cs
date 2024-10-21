using BookHub.DAL.Entities;
using BookHub.DAL.Repositories.Interfaces;
using BookHub.DAL.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace BookHub.DAL.Repositories.Implementations
{
    public class ReviewRepository : Repository<ReviewEntity>, IReviewRepository
    {
        private AppDbContext _context;

        public ReviewRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }


        public async Task<(List<ReviewEntity> Items, long TotalCount)> GetPagedAsync(int pageSize, int pageNumber)
        {
            var totalCount = await _context.Reviews.CountAsync();

            var items = await _context.Reviews
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }
    }
}
