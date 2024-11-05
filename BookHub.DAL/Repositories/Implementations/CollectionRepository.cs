using BookHub.DAL.Entities;
using BookHub.DAL.Repositories.Interfaces;
using BookHub.DAL.DataAccess;

namespace BookHub.DAL.Repositories.Implementations
{
    public class CollectionRepository : Repository<CollectionEntity>, ICollectionRepository
    {
        private AppDbContext _context;

        public CollectionRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }


    }
}
