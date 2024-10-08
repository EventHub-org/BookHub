using BookHub.DAL.Entities;

namespace BookHub.DAL.Repositories.Interfaces
{
    public interface IFriendshipRepository<TEntity> where TEntity : class
    {
        void AddFriendship(TEntity entity);
        void UpdateFriendship(TEntity entity);
        void DeleteFriendship(TEntity entity);
        FriendshipEntity GetFriendshipById(int id);
        IEnumerable<FriendshipEntity> GetAllFriendships();
    }
}
