using BookHub.DAL.Entities;
using BookHub.DAL.Repositories.Interfaces;
using BookHub.DAL.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace BookHub.DAL.Repositories.Implementations
{
    public class UserRepository : Repository<UserEntity>, IUserRepository
    {
        private AppDbContext _context;

        public UserRepository(AppDbContext context): base(context)
        {
            _context = context;
        }


        public async Task<(List<UserEntity> Items, long TotalCount)> GetPagedAsync(int pageSize, int pageNumber)
        {
            var totalCount = await _context.Users.CountAsync();
            var items = await _context.Users
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return (items, totalCount);
        }
    }
}
