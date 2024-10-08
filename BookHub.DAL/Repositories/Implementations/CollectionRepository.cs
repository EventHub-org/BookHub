using BookHub.DAL.Entities;
using BookHub.DAL.Repositories.Interfaces;
using BookHub.DAL.DataAccess;

namespace BookHub.DAL.Repositories.Implementations
{
    public class CollectionRepository : IRepository<CollectionEntity>, ICollectionRepository<CollectionEntity>
    {
        private readonly AppDbContext _context;

        public CollectionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(CollectionEntity entity)
        {
            _context.Collections.Add(entity);
            _context.SaveChanges();
        }

        public async Task UpdateAsync(CollectionEntity entity)
        {
            _context.Collections.Update(entity);
            _context.SaveChanges();
        }

        public async Task DeleteAsync(CollectionEntity entity)
        {
            _context.Collections.Remove(entity);
            _context.SaveChanges();
        }

        public async Task<CollectionEntity> GetByIdAsync(int id)
        {
            return _context.Collections.Find(id);
        }

        public async Task<IEnumerable<CollectionEntity>> GetAllAsync()
        {
            return _context.Collections.ToList();
        }
    }
}
