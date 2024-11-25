using AutoMapper;
using BookHub.BLL.Services.Interfaces;
using BookHub.DAL.DTO;
using BookHub.DAL.Entities;
using BookHub.DAL.Repositories.Interfaces;
using System.ComponentModel.DataAnnotations;
using BookHub.BLL.Utils;
using BookHub.DAL.Repositories.Implementations;

using Serilog;
using Microsoft.EntityFrameworkCore;

namespace BookHub.BLL.Services.Implementations
{
    public class CollectionService : ICollectionService 
    {
        private readonly ICollectionRepository _collectionRepository;
        private readonly IRepository<BookEntity> _bookRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public CollectionService(IUserRepository userRepository, ICollectionRepository collectionRepository, IRepository<BookEntity> bookRepository, IMapper mapper)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));

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
            var userExists = await _userRepository.ExistsAsync(collectionDto.UserId);
            if (!userExists)
            {
                Log.Error("User with ID {UserId} does not exist.", collectionDto.UserId);
                return ServiceResultType<CollectionDto>.ErrorResult($"User with ID {collectionDto.UserId} does not exist.");
            }

            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(collectionDto);
            bool isValid = Validator.TryValidateObject(collectionDto, validationContext, validationResults, true);

            if (!isValid)
            {
                return ServiceResultType<CollectionDto>.ErrorResult("Validation failed: " + string.Join(", ", validationResults.Select(v => v.ErrorMessage)));
            }

            var collectionEntity = _mapper.Map<CollectionEntity>(collectionDto);
            await _collectionRepository.AddAsync(collectionEntity);

            Log.Information($"Ініціалізовано створення колекції з Id: {collectionEntity.Id} о {DateTime.UtcNow}.");

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

            
            Log.Information($"Book entity: {bookEntity.Title}");
            

            collectionEntity.Books = collectionEntity.Books ?? new List<BookEntity>();

            if (collectionEntity.Books.Any(b => b.Id == bookId))
            {
                return ServiceResultType.ErrorResult("Book is already in the collection");
            }

            collectionEntity.Books.Add(bookEntity);
            Log.Information($"Collection entity book: {collectionEntity.Books}");

            await _collectionRepository.UpdateAsync(collectionEntity);
            Log.Information($"Ініціалізовано додавання книги з Id: {bookId} до колекції з Id: {collectionId} о {DateTime.UtcNow}.");

            return ServiceResultType.SuccessResult();
        }

        public async Task<ServiceResultType> RemoveBookFromCollectionAsync(int collectionId, int bookId)
        {
            var collectionEntity = await _collectionRepository.GetByIdAsync(c => c.Id == collectionId);
            if (collectionEntity == null)
            {
                return ServiceResultType.ErrorResult("Collection not found");
            }

            var bookEntity = collectionEntity.Books.FirstOrDefault(b => b.Id == bookId);
            if (bookEntity == null)
            {
                return ServiceResultType.ErrorResult("Book not found in the collection");
            }

            collectionEntity.Books.Remove(bookEntity);

            await _collectionRepository.UpdateAsync(collectionEntity);

            Log.Information($"Ініціалізовано видалення книги з Id: {bookId} з колекції з Id: {collectionId} о {DateTime.UtcNow}.");

            return ServiceResultType.SuccessResult();
        }
        public async Task<ServiceResultType<List<CollectionDto>>> GetAllCollectionsAsync(int userId)
        {
            Log.Information($"Початок отримання всіх колекцій для користувача з Id: {userId}...");
            try
            {
                var collectionEntities = await _collectionRepository.GetAllAsync(c => c.User.UserId == userId);
                var collectionDtos = _mapper.Map<List<CollectionDto>>(collectionEntities);

                Log.Information($"Завантажено {collectionDtos.Count} колекцій для користувача з Id: {userId}.");
                return ServiceResultType<List<CollectionDto>>.SuccessResult(collectionDtos);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Виникла помилка під час отримання колекцій для користувача з Id: {userId}.");
                return ServiceResultType<List<CollectionDto>>.ErrorResult("Failed to retrieve collections.");
            }
        }

       



    }
}
