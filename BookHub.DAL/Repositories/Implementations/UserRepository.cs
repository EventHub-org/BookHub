using BookHub.DAL.Entities;
using BookHub.DAL.Repositories.Interfaces;
using BookHub.DAL.DataAccess;

namespace BookHub.DAL.Repositories.Implementations
{
    public class UserRepository : IRepository<UserEntity>, IUserRepository<UserEntity>
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(UserEntity entity)
        {
            _context.Users.Add(entity);
            _context.SaveChanges();
        }

        public async Task UpdateAsync(UserEntity entity)
        {
            _context.Users.Update(entity);
            _context.SaveChanges();
        }

        public async Task DeleteAsync(UserEntity entity)
        {
            _context.Users.Remove(entity);
            _context.SaveChanges();
        }

        public async Task<UserEntity> GetByIdAsync(int id)
        {
            return _context.Users.Find(id);
        }

        public async Task<IEnumerable<UserEntity>> GetAllAsync()
        {
            return _context.Users.ToList();
        }
    }
}
