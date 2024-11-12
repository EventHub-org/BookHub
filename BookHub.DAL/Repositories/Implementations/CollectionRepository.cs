using BookHub.DAL.Entities;
using BookHub.DAL.Repositories.Interfaces;
using BookHub.DAL.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BookHub.DAL.Repositories.Implementations
{
    public class CollectionRepository : Repository<CollectionEntity>, ICollectionRepository
    {
        private AppDbContext _context;

        public CollectionRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<CollectionEntity>> GetAllAsync(Expression<Func<CollectionEntity, bool>> predicate)
        {
            return await _context.Collections.Where(predicate).ToListAsync();
        }


    }
}
