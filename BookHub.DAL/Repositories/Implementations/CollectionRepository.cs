using BookHub.DAL.Entities;
using BookHub.DAL.Repositories.Interfaces;
using BookHub.DAL.DataAccess;

namespace BookHub.DAL.Repositories.Implementations
{
    public class CollectionRepository : ICollectionRepository<CollectionEntity>
    {
        private readonly AppDbContext _context;

        public CollectionRepository(AppDbContext context)
        {
            _context = context;
        }

        public void AddCollection(CollectionEntity entity)
        {
            _context.Collections.Add(entity);
            _context.SaveChanges();
        }

        public void UpdateCollection(CollectionEntity entity)
        {
            _context.Collections.Update(entity);
            _context.SaveChanges();
        }

        public void DeleteCollection(CollectionEntity entity)
        {
            _context.Collections.Remove(entity);
            _context.SaveChanges();
        }

        public CollectionEntity GetCollectionById(int id)
        {
            return _context.Collections.Find(id);
        }

        public IEnumerable<CollectionEntity> GetAllCollections()
        {
            return _context.Collections.ToList();
        }
    }
}
