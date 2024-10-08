using BookHub.DAL.Entities;
using BookHub.DAL.Repositories.Interfaces;
using BookHub.DAL.DataAccess;

namespace BookHub.DAL.Repositories.Implementations
{
    public class ReviewRepository : IReviewRepository<ReviewEntity>
    {
        private readonly AppDbContext _context;

        public ReviewRepository(AppDbContext context)
        {
            _context = context;
        }

        public void AddReview(ReviewEntity entity)
        {
            _context.Reviews.Add(entity);
            _context.SaveChanges();
        }

        public void UpdateReview(ReviewEntity entity)
        {
            _context.Reviews.Update(entity);
            _context.SaveChanges();
        }

        public void DeleteReview(ReviewEntity entity)
        {
            _context.Reviews.Remove(entity);
            _context.SaveChanges();
        }

        public ReviewEntity GetReviewById(int id)
        {
            return _context.Reviews.Find(id);
        }

        public IEnumerable<ReviewEntity> GetAllReviews()
        {
            return _context.Reviews.ToList();
        }
    }
}
