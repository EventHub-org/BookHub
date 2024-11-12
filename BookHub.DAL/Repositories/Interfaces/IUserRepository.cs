using BookHub.DAL.DTO;
using BookHub.DAL.Entities;

namespace BookHub.DAL.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<UserEntity>
    {
        Task<bool> ExistsAsync(int userId);

        Task<(List<UserEntity> Items, long TotalCount)> GetPagedAsync(Pageable pageable);
    }
}
