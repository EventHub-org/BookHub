using AutoMapper;
using BookHub.BLL.Services.Interfaces;
using BookHub.DAL.DTO;
using BookHub.DAL.Entities;
using BookHub.DAL.Repositories.Interfaces;
using System.ComponentModel.DataAnnotations;
using BookHub.BLL.Utils;
using BookHub.DAL.Repositories.Implementations;

using Serilog;

namespace BookHub.BLL.Services.Implementations
{
    public class CollectionService : ICollectionService 
    {
        private readonly IRepository<CollectionEntity> _collectionRepository;
        private readonly IRepository<BookEntity> _bookRepository;
        private readonly IMapper _mapper;

        public CollectionService(IRepository<CollectionEntity> collectionRepository, IRepository<BookEntity> bookRepository, IMapper mapper)
        {
            _collectionRepository = collectionRepository;
            _bookRepository = bookRepository;
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

            Log.Information("Створення колекції");

            return ServiceResultType<CollectionDto>.SuccessResult(_mapper.Map<CollectionDto>(collectionEntity)); 
        }
        public async Task<ServiceResultType> AddBookToCollectionAsync(int collectionId, int bookId)
        {
            var collectionEntity = await _collectionRepository.GetByIdAsync(c => c.Id == collectionId);
            if (collectionEntity == null)
            {
                return ServiceResultType.ErrorResult("Collection not found");
            }

            var bookEntity = await _bookRepository.GetByIdAsync(b => b.Id == bookId);
            if (bookEntity == null)
            {
                return ServiceResultType.ErrorResult("Book not found");
            }

            if (collectionEntity.Books.Any(b => b.Id == bookId))
            {
                return ServiceResultType.ErrorResult("Book is already in the collection");
            }

            collectionEntity.Books.Add(bookEntity);

            await _collectionRepository.UpdateAsync(collectionEntity);

            return ServiceResultType.SuccessResult();
        }
    }
}
