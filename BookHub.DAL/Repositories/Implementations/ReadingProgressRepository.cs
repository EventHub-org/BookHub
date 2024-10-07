using BookHub.DAL.Entities;
using BookHub.DAL.Repositories.Interfaces;
using BookHub.DAL.DataAccess;

namespace BookHub.DAL.Repositories.Implementations
{
    public class ReadingProgressRepository : IReadingProgressRepository<ReadingProgressEntity>
    {
        private readonly AppDbContext _context;

        public ReadingProgressRepository(AppDbContext context)
        {
            _context = context;
        }

        public void AddReadingProgress(ReadingProgressEntity entity)
        {
            _context.ReadingProgresses.Add(entity);
            _context.SaveChanges();
        }

        public void UpdateReadingProgress(ReadingProgressEntity entity)
        {
            _context.ReadingProgresses.Update(entity);
            _context.SaveChanges();
        }

        public void DeleteReadingProgress(ReadingProgressEntity entity)
        {
            _context.ReadingProgresses.Remove(entity);
            _context.SaveChanges();
        }

        public ReadingProgressEntity GetReadingProgressById(int id)
        {
            return _context.ReadingProgresses.Find(id);
        }

        public IEnumerable<ReadingProgressEntity> GetAllReadingProgresses()
        {
            return _context.ReadingProgresses.ToList();
        }
    }
}
