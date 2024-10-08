using BookHub.DAL.Entities;
using BookHub.DAL.Repositories.Interfaces;
using BookHub.DAL.DataAccess;

namespace BookHub.DAL.Repositories.Implementations
{
    public class FriendshipRepository : IRepository<FriendshipEntity>, IFriendshipRepository<FriendshipEntity>
    {
        private readonly AppDbContext _context;

        public FriendshipRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(FriendshipEntity entity)
        {
            _context.Friendships.Add(entity);
            _context.SaveChanges();
        }

        public async Task UpdateAsync(FriendshipEntity entity)
        {
            _context.Friendships.Update(entity);
            _context.SaveChanges();
        }

        public async Task DeleteAsync(FriendshipEntity entity)
        {
            _context.Friendships.Remove(entity);
            _context.SaveChanges();
        }

        public async Task<FriendshipEntity> GetByIdAsync(int id)
        {
            return _context.Friendships.Find(id);
        }

        public async Task<IEnumerable<FriendshipEntity>> GetAllAsync()
        {
            return _context.Friendships.ToList();
        }
    }
}
