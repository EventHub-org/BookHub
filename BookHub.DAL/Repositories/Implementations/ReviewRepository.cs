using BookHub.DAL.Entities;
using BookHub.DAL.Repositories.Interfaces;
using BookHub.DAL.DataAccess;
using Microsoft.EntityFrameworkCore;
using BookHub.DAL.DTO;

namespace BookHub.DAL.Repositories.Implementations
{
    public class ReviewRepository : Repository<ReviewEntity>, IReviewRepository
    {
        private AppDbContext _context;

        public ReviewRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }


        public async Task<(List<ReviewEntity> Items, long TotalCount)> GetPagedAsync(Pageable pageable)
        {
            var totalCount = await _context.Reviews.CountAsync();

            var items = await _context.Reviews
                .Skip((pageable.Page - 1) * pageable.Size)
                .Take(pageable.Size)
                .ToListAsync();

            return (items, totalCount);
        }
    }
}
