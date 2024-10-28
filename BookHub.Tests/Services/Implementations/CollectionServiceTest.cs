using AutoMapper;
using BookHub.BLL.Services.Implementations;
using BookHub.DAL.DTO;
using BookHub.DAL.Entities;
using BookHub.DAL.Repositories.Interfaces;
using Moq;
using Sprache;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace BookHub.Tests.Services.Impl
{
    public class CollectionServiceTest
    {

        private readonly Mock<IRepository<CollectionEntity>> _mockCollectionRepository;
        private readonly Mock<IRepository<BookEntity>> _mockBookRepository; 
        private readonly IMapper _mapper;
        private readonly CollectionService _collectionService;

        public CollectionServiceTest()
        {
            _mockCollectionRepository = new Mock<IRepository<CollectionEntity>>();
            _mockBookRepository = new Mock<IRepository<BookEntity>>(); 

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CollectionEntity, CollectionDto>().ReverseMap();
            });
            _mapper = config.CreateMapper();

            _collectionService = new CollectionService(_mockCollectionRepository.Object, _mockBookRepository.Object, _mapper);
        }

        [Fact]
        public async Task CreateCollectionAsync_ShouldThrowArgumentNullException_WhenCollectionDtoIsNull()
        {
            // Arrange
            CollectionDto collectionDto = null;

            var stas  = await _collectionService.CreateCollectionAsync(collectionDto);
            // Act & Assert
             Assert.Equal("Collection data cannot be null", stas.ErrorMessage);

        }

        [Fact]
        public async Task CreateCollectionAsync_ShouldThrowValidationException_WhenNameIsTooShort()
        {
            // Arrange
            var collectionDto = new CollectionDto { Name = "A" }; // Ім'я занадто коротке

            // Act
            var test = await _collectionService.CreateCollectionAsync(collectionDto);

            // Assert
            Assert.False(test.Success);
        }

        [Fact]
        public async Task CreateCollectionAsync_ShouldThrowValidationException_WhenNameIsTooLong()
        {
            // Arrange
            var collectionDto = new CollectionDto { Name = new string('A', 51) }; // Ім'я занадто довге

            // Act
            var exception = await  _collectionService.CreateCollectionAsync(collectionDto);

            // Assert
            Assert.False(exception.Success);
        }



        [Fact]
        public async Task CreateCollectionAsync_ShouldReturnCollectionDto_WhenCollectionIsCreated()
        {
            // Arrange
            var collectionDto = new CollectionDto { Name = "My Collection" };
            var collectionEntity = _mapper.Map<CollectionEntity>(collectionDto);

            _mockCollectionRepository
                .Setup(repo => repo.AddAsync(It.IsAny<CollectionEntity>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _collectionService.CreateCollectionAsync(collectionDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(collectionDto.Name, result.Data.Name);
        }

        [Fact]
        public async Task AddBookToCollectionAsync_ShouldReturnError_WhenBookNotFound()
        {
            // Arrange
            int collectionId = 1;
            int bookId = 1;

            var collectionEntity = new CollectionEntity { Id = collectionId };

            _mockCollectionRepository
                .Setup(repo => repo.GetByIdAsync(It.IsAny<Expression<Func<CollectionEntity, bool>>>()))
                .ReturnsAsync(collectionEntity);

            _mockBookRepository
                .Setup(repo => repo.GetByIdAsync(It.IsAny<Expression<Func<BookEntity, bool>>>()))
                .ReturnsAsync((BookEntity)null);

            // Act
            var result = await _collectionService.AddBookToCollectionAsync(collectionId, bookId);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Book not found", result.ErrorMessage);
        }

        [Fact]
        public async Task AddBookToCollectionAsync_ShouldAddBookToCollection_WhenBookIsFound()
        {
            // Arrange
            int collectionId = 1;
            int bookId = 1;

            var collectionEntity = new CollectionEntity
            {
                Id = collectionId,
                Books = new List<BookEntity>() 
            };

            var bookEntity = new BookEntity
            {
                Id = bookId,
                Title = "New Book"
            };

            _mockCollectionRepository
                .Setup(repo => repo.GetByIdAsync(It.IsAny<Expression<Func<CollectionEntity, bool>>>()))
                .ReturnsAsync(collectionEntity);

            _mockBookRepository
                .Setup(repo => repo.GetByIdAsync(It.IsAny<Expression<Func<BookEntity, bool>>>()))
                .ReturnsAsync(bookEntity);

            _mockCollectionRepository
                .Setup(repo => repo.UpdateAsync(It.IsAny<CollectionEntity>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _collectionService.AddBookToCollectionAsync(collectionId, bookId);

            // Assert
            Assert.True(result.Success);  
            Assert.Contains(collectionEntity.Books, b => b.Id == bookId); 
        }
        [Fact]
        public async Task AddBookToCollectionAsync_ShouldReturnError_WhenBookIsAlreadyInCollection()
        {
            // Arrange
            int collectionId = 1;
            int bookId = 1;

            var collectionEntity = new CollectionEntity
            {
                Id = collectionId,
                Books = new List<BookEntity>
        {
            new BookEntity { Id = bookId, Title = "Existing Book" }
        }
            };

            var bookEntity = new BookEntity
            {
                Id = bookId,
                Title = "Existing Book"
            };

            _mockCollectionRepository
                .Setup(repo => repo.GetByIdAsync(It.IsAny<Expression<Func<CollectionEntity, bool>>>()))
                .ReturnsAsync(collectionEntity);

            _mockBookRepository
                .Setup(repo => repo.GetByIdAsync(It.IsAny<Expression<Func<BookEntity, bool>>>()))
                .ReturnsAsync(bookEntity);

            // Act
            var result = await _collectionService.AddBookToCollectionAsync(collectionId, bookId);

            // Assert
            Assert.False(result.Success);  
            Assert.Equal("Book is already in the collection", result.ErrorMessage); 
        }

        [Fact]
        public async Task RemoveBookFromCollectionAsync_ShouldReturnError_WhenCollectionNotFound()
        {
            // Arrange
            int collectionId = 1;
            int bookId = 1;
            _mockCollectionRepository.Setup(repo => repo.GetByIdAsync(c => c.Id == collectionId))
                                     .ReturnsAsync((CollectionEntity)null);

            // Act
            var result = await _collectionService.RemoveBookFromCollectionAsync(collectionId, bookId);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Collection not found", result.ErrorMessage);
        }

        [Fact]
        public async Task RemoveBookFromCollectionAsync_ShouldReturnError_WhenBookNotFoundInCollection()
        {
            // Arrange
            int collectionId = 1;
            int bookId = 1;
            var collectionEntity = new CollectionEntity
            {
                Id = collectionId,
                Books = new List<BookEntity>() // Порожній список книг
            };

            _mockCollectionRepository.Setup(repo => repo.GetByIdAsync(c => c.Id == collectionId))
                                     .ReturnsAsync(collectionEntity);

            // Act
            var result = await _collectionService.RemoveBookFromCollectionAsync(collectionId, bookId);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Book not found in the collection", result.ErrorMessage);
        }

        [Fact]
        public async Task RemoveBookFromCollectionAsync_ShouldReturnSuccess_WhenBookIsRemoved()
        {
            // Arrange
            int collectionId = 1;
            int bookId = 1;
            var bookEntity = new BookEntity { Id = bookId };
            var collectionEntity = new CollectionEntity
            {
                Id = collectionId,
                Books = new List<BookEntity> { bookEntity } // Книга вже є в колекції
            };

            _mockCollectionRepository.Setup(repo => repo.GetByIdAsync(c => c.Id == collectionId))
                                     .ReturnsAsync(collectionEntity);
            _mockCollectionRepository.Setup(repo => repo.UpdateAsync(It.IsAny<CollectionEntity>()))
                                     .Returns(Task.CompletedTask);

            // Act
            var result = await _collectionService.RemoveBookFromCollectionAsync(collectionId, bookId);

            // Assert
            Assert.True(result.Success);
            Assert.Null(result.ErrorMessage);
            _mockCollectionRepository.Verify(repo => repo.UpdateAsync(collectionEntity), Times.Once);
        }


    }
}
