using AutoMapper;
using BookHub.BLL.Services.Implementations;
using BookHub.DAL.Entities;
using BookHub.DAL.Mappers;
using BookHub.DAL.Repositories.Interfaces;
using Moq;

namespace BookHub.Tests.Services.Impl
{
    public class BookServiceImplTests
    {
        private readonly Mock<IBookRepository<BookEntity>> _mockBookRepository;
        private readonly IMapper _mapper;
        private readonly BookService _bookService;

        public BookServiceImplTests()
        {
            _mockBookRepository = new Mock<IBookRepository<BookEntity>>();

            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<BookProfile>();
            });
            _mapper = config.CreateMapper();

            _bookService = new BookService(_mockBookRepository.Object, _mapper);
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
    }
}
