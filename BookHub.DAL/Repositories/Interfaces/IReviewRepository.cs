using BookHub.DAL.Entities;

namespace BookHub.DAL.Repositories.Interfaces
{
    public interface IReviewRepository<TEntity> where TEntity : class
    {
        void AddReview(TEntity entity);
        void UpdateReview(TEntity entity);
        void DeleteReview(TEntity entity);
        ReviewEntity GetReviewById(int id);
        IEnumerable<ReviewEntity> GetAllReviews();
    }
}
