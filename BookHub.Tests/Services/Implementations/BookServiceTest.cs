using AutoMapper;
using BookHub.BLL.Services.Implementations;
using BookHub.DAL.Entities;
using BookHub.DAL.Mappers;
using BookHub.DAL.Repositories.Interfaces;
using BookHub.DAL.DTO;
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

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<BookProfile>();
            });
            _mapper = config.CreateMapper();

            _bookService = new BookService(_mockRepository.Object, _mockBookRepository.Object, _mapper);
        }

        [Fact]
        public async Task GetPaginatedBooksAsync_ShouldReturnError_WhenSizeIsZero()
        {
            // Arrange
            var pageable = new Pageable(size: 0, page: 1);

            // Act
            var result = await _bookService.GetPaginatedBooksAsync(pageable);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Page size must be greater than zero.", result.ErrorMessage);
        }

        [Fact]
        public async Task GetPaginatedBooksAsync_ShouldReturnError_WhenPageIsNegative()
        {
            // Arrange
            var pageable = new Pageable(size: 1, page: -1);

            // Act
            var result = await _bookService.GetPaginatedBooksAsync(pageable);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Page number must be greater than or equal to zero.", result.ErrorMessage);
        }

        [Fact]
        public async Task GetPaginatedBooksAsync_ShouldReturnPageDto_WhenDataIsValid()
        {
            // Arrange
            var pageable = new Pageable(size: 2, page: 1);

            var bookEntities = new List<BookEntity>
            {
                new BookEntity { Id = 1 },
                new BookEntity { Id = 2 }
            };

            var totalElements = 5;

            _mockBookRepository
                .Setup(repo => repo.GetPagedAsync(pageable))
                .ReturnsAsync((bookEntities, totalElements));

            // Act
            var result = await _bookService.GetPaginatedBooksAsync(pageable);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(totalElements, result.Data.TotalElements);
            Assert.Equal(pageable.Page, result.Data.CurrentPage);
            Assert.Equal((int)Math.Ceiling((double)totalElements / pageable.Size), result.Data.TotalPages);
            Assert.Equal(bookEntities.Count, result.Data.Items.Count);

            // Additional assertions to check specific items in the result
            Assert.Equal(bookEntities[0].Id, result.Data.Items[0].Id);
            Assert.Equal(bookEntities[1].Id, result.Data.Items[1].Id);
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
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(bookId, result.Data.Id);
        }

        [Fact]
        public async Task GetBookAsync_ShouldReturnError_WhenBookDoesNotExist()
        {
            // Arrange
            int bookId = 1;
            _mockRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Expression<Func<BookEntity, bool>>>())).ReturnsAsync((BookEntity)null);

            // Act
            var result = await _bookService.GetBookAsync(bookId);

            // Assert
            Assert.False(result.Success);
            Assert.Equal($"Book with ID {bookId} not found.", result.ErrorMessage);
        }

        [Fact]
        public async Task DeleteBookAsync_ShouldCallRepositoryDelete_WhenBookExists()
        {
            // Arrange
            int bookId = 1;
            var bookEntity = new BookEntity { Id = bookId };
            _mockRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Expression<Func<BookEntity, bool>>>())).ReturnsAsync(bookEntity);

            // Act
            var result = await _bookService.DeleteBookAsync(bookId);

            // Assert
            Assert.True(result.Success);
            _mockRepository.Verify(repo => repo.DeleteAsync(bookEntity), Times.Once);
        }

        [Fact]
        public async Task DeleteBookAsync_ShouldReturnError_WhenBookDoesNotExist()
        {
            // Arrange
            int bookId = 1;
            _mockRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Expression<Func<BookEntity, bool>>>())).ReturnsAsync((BookEntity)null);

            // Act
            var result = await _bookService.DeleteBookAsync(bookId);

            // Assert
            Assert.False(result.Success);
            Assert.Equal($"Book with ID {bookId} not found.", result.ErrorMessage);
            _mockRepository.Verify(repo => repo.DeleteAsync(It.IsAny<BookEntity>()), Times.Never);
        }
    }
}
