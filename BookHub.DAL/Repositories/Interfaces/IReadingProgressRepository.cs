using BookHub.DAL.Entities;

namespace BookHub.DAL.Repositories.Interfaces
{
    public interface IReadingProgressRepository<TEntity> where TEntity : class
    {
        void AddReadingProgress(TEntity entity);
        void UpdateReadingProgress(TEntity entity);
        void DeleteReadingProgress(TEntity entity);
        ReadingProgressEntity GetReadingProgressById(int id);
        IEnumerable<ReadingProgressEntity> GetAllReadingProgresses();
    }
}
