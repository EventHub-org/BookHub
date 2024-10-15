using BookHub.DAL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookHub.BLL.Services.Interfaces
{
    public interface ICollectionService
    {
        Task<CollectionDto> CreateCollectionAsync(CollectionDto collectionDto);

    }
}
