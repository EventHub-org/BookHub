using BookHub.DAL.DTO;
using BookHub.BLL.Utils;

namespace BookHub.BLL.Services.Interfaces
{
    public interface ICollectionService
    {
        Task<ServiceResultType<CollectionDto>> CreateCollectionAsync(CollectionDto collectionDto);
        Task<ServiceResultType> AddBookToCollectionAsync(int collectionId, int bookId);
        Task<ServiceResultType> RemoveBookFromCollectionAsync(int collectionId, int bookId);
        Task<ServiceResultType<List<CollectionDto>>> GetAllCollectionsAsync(int userId);

    }
}
