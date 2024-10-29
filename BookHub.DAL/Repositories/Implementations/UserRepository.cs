using BookHub.DAL.Entities;
using BookHub.DAL.Repositories.Interfaces;
using BookHub.DAL.DataAccess;
using Microsoft.EntityFrameworkCore;
using BookHub.DAL.DTO;

namespace BookHub.DAL.Repositories.Implementations
{
    public class UserRepository : Repository<UserEntity>, IUserRepository
    {
        private AppDbContext _context;

        public UserRepository(AppDbContext context): base(context)
        {
            _context = context;
        }


        public async Task<(List<UserEntity> Items, long TotalCount)> GetPagedAsync(Pageable pageable)
        {
            var totalCount = await _context.Users.CountAsync();
            var items = await _context.Users
                .Skip((pageable.Page - 1) * pageable.Size)
                .Take(pageable.Size)
                .ToListAsync();
            return (items, totalCount);
        }
    }
}
