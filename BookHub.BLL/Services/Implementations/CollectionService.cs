using AutoMapper;
using BookHub.BLL.Services.Interfaces;
using BookHub.DAL.DTO;
using BookHub.DAL.Entities;
using BookHub.DAL.Repositories.Interfaces;
using System.ComponentModel.DataAnnotations;
using BookHub.BLL.Utils;

namespace BookHub.BLL.Services.Implementations
{
    public class CollectionService : ICollectionService 
    {
        private readonly IRepository<CollectionEntity> _collectionRepository;
        private readonly IMapper _mapper;

        public CollectionService(IRepository<CollectionEntity> collectionRepository, IMapper mapper)
        {
            _collectionRepository = collectionRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResultType<CollectionDto>> CreateCollectionAsync(CollectionDto collectionDto)
        {
            if (collectionDto == null)
            {
                return ServiceResultType<CollectionDto>.ErrorResult("Collection data cannot be null");
            }

            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(collectionDto);
            bool isValid = Validator.TryValidateObject(collectionDto, validationContext, validationResults, true);

            if (!isValid)
            {
                return ServiceResultType<CollectionDto>.ErrorResult("Validation failed: " + string.Join(", ", validationResults.Select(v => v.ErrorMessage)));
            }

            // Перетворення DTO в сутність
            var collectionEntity = _mapper.Map<CollectionEntity>(collectionDto);
            await _collectionRepository.AddAsync(collectionEntity);
            return ServiceResultType<CollectionDto>.SuccessResult(_mapper.Map<CollectionDto>(collectionEntity)); 
        }

    }
}
