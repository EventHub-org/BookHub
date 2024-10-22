using BookHub.DAL.Entities;
using BookHub.DAL.Repositories.Interfaces;
using BookHub.DAL.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace BookHub.DAL.Repositories.Implementations
{
    public class FriendshipRepository : Repository<FriendshipEntity>, IFriendshipRepository
    {
        private AppDbContext _context;

        public FriendshipRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        

    }
}
