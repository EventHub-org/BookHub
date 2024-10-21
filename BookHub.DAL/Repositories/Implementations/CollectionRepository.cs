using BookHub.DAL.Entities;
using BookHub.DAL.Repositories.Interfaces;
using BookHub.DAL.DataAccess;

namespace BookHub.DAL.Repositories.Implementations
{
    public class CollectionRepository 
    {
        private AppDbContext _context;

        public CollectionRepository(AppDbContext context) 
        {
            _context = context;
        }

        
    }
}
