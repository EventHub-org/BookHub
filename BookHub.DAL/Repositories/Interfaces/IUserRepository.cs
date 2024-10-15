using BookHub.DAL.Entities;

namespace BookHub.DAL.Repositories.Interfaces
{
    public interface IUserRepository<TEntity> where TEntity : class
    {
        Task<UserEntity> GetByIdAsync(int id);
        Task<(List<UserEntity> Items, long TotalCount)> GetPagedAsync(int pageSize, int pageNumber);
    }
}
