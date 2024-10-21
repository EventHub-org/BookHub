using AutoMapper;
using BookHub.BLL.Services.Implementations;
using BookHub.DAL.Entities;
using BookHub.DAL.Mappers;
using BookHub.DAL.Repositories.Interfaces;
using Moq;
using System.Linq.Expressions;

namespace BookHub.Tests.Services.Impl
{
    public class BookServiceImplTests
    {
        private readonly Mock<IRepository<BookEntity>> _mockRepository;
        private readonly Mock<IBookRepository> _mockBookRepository;
        private readonly IMapper _mapper;
        private readonly BookService _bookService;

        public BookServiceImplTests()
        {
            _mockRepository = new Mock<IRepository<BookEntity>>();
            _mockBookRepository = new Mock<IBookRepository>();

            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<BookProfile>();
            });
            _mapper = config.CreateMapper();

            _bookService = new BookService(_mockRepository.Object, _mockBookRepository.Object, _mapper);
        }

        [Fact]
        public async Task GetPageDto_ShouldThrowArgumentException_WhenSizeIsZero()
        {
            // Arrange
            int size = 0;
            int page = 1;

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _bookService.GetPaginatedBooksAsync(size, page));
            Assert.Equal("Page size must be greater than zero. (Parameter 'size')", exception.Message);
        }

        [Fact]
        public async Task GetPageDto_ShouldThrowArgumentException_WhenPageIsZero()
        {
            // Arrange
            int size = 1;
            int page = 0;

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _bookService.GetPaginatedBooksAsync(size, page));
            Assert.Equal("Page number must be greater than zero. (Parameter 'page')", exception.Message);
        }

        [Fact]
        public async Task GetPageDto_ShouldReturnPageDto_WhenDataIsValid()
        {
            int size = 2;
            int page = 1;

            var bookEntities = new List<BookEntity>
            {
                new BookEntity(),
                new BookEntity()
            };

            var totalElements = 5;
            _mockBookRepository
                .Setup(repo => repo.GetPagedAsync(size, page))
                .ReturnsAsync((bookEntities, totalElements));

            // Act
            var result = await _bookService.GetPaginatedBooksAsync(size, page);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(totalElements, result.TotalElements);
            Assert.Equal(page, result.CurrentPage);
            Assert.Equal((int)Math.Ceiling((double)totalElements / size), result.TotalPages);
            Assert.Equal(bookEntities.Count, result.Items.Count);
        }

        [Fact]
        public async Task GetBookAsync_ShouldReturnBookDto_WhenBookExists()
        {
            // Arrange
            int bookId = 1;
            var bookEntity = new BookEntity { Id = bookId };
            _mockRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Expression<Func<BookEntity, bool>>>())).ReturnsAsync(bookEntity);

            // Act
            var result = await _bookService.GetBookAsync(bookId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(bookId, result.Id);
        }

        [Fact]
        public async Task GetBookAsync_ShouldThrowKeyNotFoundException_WhenBookDoesNotExist()
        {
            // Arrange
            int bookId = 1;
            _mockRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Expression<Func<BookEntity, bool>>>())).ReturnsAsync((BookEntity)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _bookService.GetBookAsync(bookId));
            Assert.Equal($"Book with ID {bookId} not found.", exception.Message);
        }

        [Fact]
        public async Task DeleteBookAsync_ShouldCallRepositoryDelete_WhenBookExists()
        {
            // Arrange
            int bookId = 1;
            var bookEntity = new BookEntity { Id = bookId };
            _mockRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Expression<Func<BookEntity, bool>>>())).ReturnsAsync(bookEntity);

            // Act
            await _bookService.DeleteBookAsync(bookId);

            // Assert
            _mockRepository.Verify(repo => repo.DeleteAsync(bookEntity), Times.Once);
        }
    }
}
