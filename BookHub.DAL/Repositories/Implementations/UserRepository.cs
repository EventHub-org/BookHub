using BookHub.DAL.Entities;
using BookHub.DAL.Repositories.Interfaces;
using BookHub.DAL.DataAccess;

namespace BookHub.DAL.Repositories.Implementations
{
    public class UserRepository : IUserRepository<UserEntity>
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public void AddUser(UserEntity entity)
        {
            _context.Users.Add(entity);
            _context.SaveChanges();
        }

        public void UpdateUser(UserEntity entity)
        {
            _context.Users.Update(entity);
            _context.SaveChanges();
        }

        public void DeleteUser(UserEntity entity)
        {
            _context.Users.Remove(entity);
            _context.SaveChanges();
        }

        public UserEntity GetUserById(int id)
        {
            return _context.Users.Find(id);
        }

        public IEnumerable<UserEntity> GetAllUsers()
        {
            return _context.Users.ToList();
        }
    }
}
