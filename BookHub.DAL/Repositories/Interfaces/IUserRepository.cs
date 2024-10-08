using BookHub.DAL.Entities;

namespace BookHub.DAL.Repositories.Interfaces
{
    public interface IUserRepository<TEntity> where TEntity : class
    {
        void AddUser(TEntity entity);
        void UpdateUser(TEntity entity);
        void DeleteUser(TEntity entity);
        UserEntity GetUserById(int id);
        IEnumerable<UserEntity> GetAllUsers();
    }
}
