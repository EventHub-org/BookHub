using BookHub.DAL.DTO;
using BookHub.BLL.Utils;

namespace BookHub.BLL.Services.Interfaces
{
    public interface ICollectionService
    {
        Task<ServiceResultType<CollectionDto>> CreateCollectionAsync(CollectionDto collectionDto);
    }
}
