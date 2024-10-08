using BookHub.DAL.Entities;
using BookHub.DAL.Repositories.Interfaces;
using BookHub.DAL.DataAccess;

namespace BookHub.DAL.Repositories.Implementations
{
    public class FriendshipRepository : IFriendshipRepository<FriendshipEntity>
    {
        private readonly AppDbContext _context;

        public FriendshipRepository(AppDbContext context)
        {
            _context = context;
        }

        public void AddFriendship(FriendshipEntity entity)
        {
            _context.Friendships.Add(entity);
            _context.SaveChanges();
        }

        public void UpdateFriendship(FriendshipEntity entity)
        {
            _context.Friendships.Update(entity);
            _context.SaveChanges();
        }

        public void DeleteFriendship(FriendshipEntity entity)
        {
            _context.Friendships.Remove(entity);
            _context.SaveChanges();
        }

        public FriendshipEntity GetFriendshipById(int id)
        {
            return _context.Friendships.Find(id);
        }

        public IEnumerable<FriendshipEntity> GetAllFriendships()
        {
            return _context.Friendships.ToList();
        }
    }
}
