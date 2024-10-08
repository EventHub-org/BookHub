using BookHub.DAL.Entities;
using BookHub.DAL.Repositories.Interfaces;
using BookHub.DAL.DataAccess;

namespace BookHub.DAL.Repositories.Implementations
{
    public class ReadingProgressRepository : IRepository<ReadingProgressEntity>, IReadingProgressRepository<ReadingProgressEntity>
    {
        private readonly AppDbContext _context;

        public ReadingProgressRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ReadingProgressEntity entity)
        {
            _context.ReadingProgresses.Add(entity);
            _context.SaveChanges();
        }

        public async Task UpdateAsync(ReadingProgressEntity entity)
        {
            _context.ReadingProgresses.Update(entity);
            _context.SaveChanges();
        }

        public async Task DeleteAsync(ReadingProgressEntity entity)
        {
            _context.ReadingProgresses.Remove(entity);
            _context.SaveChanges();
        }

        public async Task<ReadingProgressEntity> GetByIdAsync(int id)
        {
            return _context.ReadingProgresses.Find(id);
        }

        public async Task<IEnumerable<ReadingProgressEntity>> GetAllAsync()
        {
            return _context.ReadingProgresses.ToList();
        }
    }
}
