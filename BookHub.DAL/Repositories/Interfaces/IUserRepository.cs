using BookHub.DAL.DTO;
using BookHub.DAL.Entities;

namespace BookHub.DAL.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<UserEntity>
    {
        Task<(List<UserEntity> Items, long TotalCount)> GetPagedAsync(Pageable pageable);
    }
}
