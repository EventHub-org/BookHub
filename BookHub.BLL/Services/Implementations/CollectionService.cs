using AutoMapper;
using BookHub.BLL.Services.Interfaces;
using BookHub.DAL.DTO;
using BookHub.DAL.Entities;
using BookHub.DAL.Repositories.Interfaces;
using System.ComponentModel.DataAnnotations;

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

        public async Task<CollectionDto> CreateCollectionAsync(CollectionDto collectionDto)
        {


            if (collectionDto == null)
            {
                throw new ArgumentNullException(nameof(collectionDto));
            }
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(collectionDto);
            bool isValid = Validator.TryValidateObject(collectionDto, validationContext, validationResults, true);

            if (!isValid)
            {
                throw new ValidationException("Validation failed: " + string.Join(", ", validationResults.Select(v => v.ErrorMessage)));
            }
            // Перетворення DTO в сутність
            var collectionEntity = _mapper.Map<CollectionEntity>(collectionDto);
            await _collectionRepository.AddAsync(collectionEntity);
            return _mapper.Map<CollectionDto>(collectionEntity);
        }


    }
}
