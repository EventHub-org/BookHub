using BookHub.DAL.Entities;
using BookHub.DAL.Repositories.Interfaces;
using BookHub.DAL.DataAccess;

namespace BookHub.DAL.Repositories.Implementations
{
    public class ReviewRepository : IRepository<ReviewEntity>, IReviewRepository<ReviewEntity>
    {
        private readonly AppDbContext _context;

        public ReviewRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ReviewEntity entity)
        {
            _context.Reviews.Add(entity);
            _context.SaveChanges();
        }

        public async Task UpdateAsync(ReviewEntity entity)
        {
            _context.Reviews.Update(entity);
            _context.SaveChanges();
        }

        public async Task DeleteAsync(ReviewEntity entity)
        {
            _context.Reviews.Remove(entity);
            _context.SaveChanges();
        }

        public async Task<ReviewEntity> GetByIdAsync(int id)
        {
            return _context.Reviews.Find(id);
        }

        public async Task<IEnumerable<ReviewEntity>> GetAllAsync()
        {
            return _context.Reviews.ToList();
        }
    }
}
