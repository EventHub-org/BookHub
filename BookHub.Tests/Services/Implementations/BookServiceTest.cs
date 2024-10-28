using AutoMapper;
using BookHub.BLL.Services.Implementations;
using BookHub.DAL.Entities;
using BookHub.DAL.Repositories.Interfaces;
using BookHub.DAL.DTO;
using Moq;
using System.ComponentModel.DataAnnotations;
using BookHub.DAL.Mappers;
using System.Linq.Expressions;

namespace BookHub.Tests.Services.Impl
{
    public class BookServiceImplTests
    {
        private readonly Mock<IBookRepository> _mockBookRepository;
        private readonly IMapper _mapper;
        private readonly BookService _bookService;

        public BookServiceImplTests()
        {
            _mockBookRepository = new Mock<IBookRepository>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<BookProfile>();
                cfg.AddProfile<BookCreateProfile>();
            });
            _mapper = config.CreateMapper();

            _bookService = new BookService(_mockBookRepository.Object, _mapper);
        }

        [Fact]
        public async Task GetPaginatedBooksAsync_ShouldReturnError_WhenSizeIsZero()
        {
            var pageable = new Pageable(size: 0, page: 1);
            var result = await _bookService.GetPaginatedBooksAsync(pageable);
            Assert.False(result.Success);
            Assert.Equal("Page size must be greater than zero.", result.ErrorMessage);
        }

        [Fact]
        public async Task GetPaginatedBooksAsync_ShouldReturnError_WhenPageIsNegative()
        {
            var pageable = new Pageable(size: 1, page: -1);
            var result = await _bookService.GetPaginatedBooksAsync(pageable);
            Assert.False(result.Success);
            Assert.Equal("Page number must be greater than or equal to zero.", result.ErrorMessage);
        }

        [Fact]
        public async Task GetPaginatedBooksAsync_ShouldReturnPageDto_WhenDataIsValid()
        {
            var pageable = new Pageable(size: 2, page: 1);
            var bookEntities = new List<BookEntity>
            {
                new BookEntity { Id = 1, Title = "Book One", Author = "Author A", PublishedDate = DateTime.Now, NumberOfPages = 200 },
                new BookEntity { Id = 2, Title = "Book Two", Author = "Author B", PublishedDate = DateTime.Now, NumberOfPages = 300 }
            };
            var totalElements = 5;

            _mockBookRepository.Setup(repo => repo.GetPagedAsync(pageable)).ReturnsAsync((bookEntities, totalElements));

            var result = await _bookService.GetPaginatedBooksAsync(pageable);
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(totalElements, result.Data.TotalElements);
            Assert.Equal(pageable.Page, result.Data.CurrentPage);
            Assert.Equal((int)Math.Ceiling((double)totalElements / pageable.Size), result.Data.TotalPages);
            Assert.Equal(bookEntities.Count, result.Data.Items.Count);
            Assert.Equal(bookEntities[0].Id, result.Data.Items[0].Id);
            Assert.Equal(bookEntities[1].Id, result.Data.Items[1].Id);
        }

        [Fact]
        public async Task GetBookAsync_ShouldReturnBookDto_WhenBookExists()
        {
            int bookId = 1;
            var bookEntity = new BookEntity { Id = bookId, Title = "Sample Book", Author = "Author A", PublishedDate = DateTime.Now, NumberOfPages = 150 };
            _mockBookRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Expression<Func<BookEntity, bool>>>())).ReturnsAsync(bookEntity);
            var result = await _bookService.GetBookAsync(bookId);
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(bookId, result.Data.Id);
            Assert.Equal(bookEntity.Title, result.Data.Title);
            Assert.Equal(bookEntity.Author, result.Data.Author);
        }

        [Fact]
        public async Task GetBookAsync_ShouldReturnError_WhenBookDoesNotExist()
        {
            int bookId = 1;
            _mockBookRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Expression<Func<BookEntity, bool>>>())).ReturnsAsync((BookEntity)null);
            var result = await _bookService.GetBookAsync(bookId);
            Assert.False(result.Success);
            Assert.Equal($"Book with ID {bookId} not found.", result.ErrorMessage);
        }

        [Fact]
        public async Task DeleteBookAsync_ShouldCallRepositoryDelete_WhenBookExists()
        {
            int bookId = 1;
            var bookEntity = new BookEntity { Id = bookId, Title = "Sample Book", Author = "Author A", PublishedDate = DateTime.Now, NumberOfPages = 150 };
            _mockBookRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Expression<Func<BookEntity, bool>>>())).ReturnsAsync(bookEntity);
            var result = await _bookService.DeleteBookAsync(bookId);
            Assert.True(result.Success);
            _mockBookRepository.Verify(repo => repo.DeleteAsync(bookEntity), Times.Once);
        }

        [Fact]
        public async Task DeleteBookAsync_ShouldReturnError_WhenBookDoesNotExist()
        {
            int bookId = 1;
            _mockBookRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Expression<Func<BookEntity, bool>>>())).ReturnsAsync((BookEntity)null);
            var result = await _bookService.DeleteBookAsync(bookId);
            Assert.False(result.Success);
            Assert.Equal($"Book with ID {bookId} not found.", result.ErrorMessage);
            _mockBookRepository.Verify(repo => repo.DeleteAsync(It.IsAny<BookEntity>()), Times.Never);
        }

        [Fact]
        public async Task CreateBook_ShouldReturnError_WhenBookCreateDtoIsNull()
        {
            BookCreateDto bookCreateDto = null;

            var result = await _bookService.CreateBook(bookCreateDto);

            Assert.False(result.Success);
            Assert.Equal("Book data cannot be null", result.ErrorMessage);
        }

        [Fact]
        public async Task CreateBook_ShouldReturnError_WhenValidationFails()
        {
            var bookCreateDto = new BookCreateDto();
            var validationResults = new List<ValidationResult>
            {
                new ValidationResult("Title is required.", new[] { "Title" }),
                new ValidationResult("Published date is required.", new[] { "PublishedDate" }),
                new ValidationResult("Author is required.", new[] { "Author" })
            };

            var validationContext = new ValidationContext(bookCreateDto);
            var isValid = Validator.TryValidateObject(bookCreateDto, validationContext, validationResults, true);

            // Act
            var result = await _bookService.CreateBook(bookCreateDto);

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async Task CreateBook_ShouldReturnBookDto_WhenBookIsCreatedSuccessfully()
        {
            var bookCreateDto = new BookCreateDto
            {
                Title = "Test Book",
                Author = "Author A",
                PublishedDate = DateTime.Now,
                NumberOfPages = 250,
                CoverImageUrl = "http://example.com/cover.jpg",
                Rating = 4.5,
                Genre = "Fiction"
            };

            var bookEntity = new BookEntity
            {
                Id = 1,
                Title = bookCreateDto.Title,
                Author = bookCreateDto.Author,
                PublishedDate = bookCreateDto.PublishedDate,
                NumberOfPages = bookCreateDto.NumberOfPages,
                CoverImageUrl = bookCreateDto.CoverImageUrl,
                Rating = bookCreateDto.Rating,
                Genre = bookCreateDto.Genre
            };

            _mockBookRepository.Setup(repo => repo.AddAsync(It.IsAny<BookEntity>()))
                               .Callback<BookEntity>(entity => entity.Id = bookEntity.Id);

            var result = await _bookService.CreateBook(bookCreateDto);

            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(bookEntity.Id, result.Data.Id);
            Assert.Equal(bookCreateDto.Title, result.Data.Title);
            Assert.Equal(bookCreateDto.Author, result.Data.Author);
            Assert.Equal(bookCreateDto.PublishedDate, result.Data.PublishedDate);
            Assert.Equal(bookCreateDto.NumberOfPages, result.Data.NumberOfPages);
            Assert.Equal(bookCreateDto.CoverImageUrl, result.Data.CoverImageUrl);
            Assert.Equal(bookCreateDto.Rating, result.Data.Rating);
            Assert.Equal(bookCreateDto.Genre, result.Data.Genre);
            _mockBookRepository.Verify(repo => repo.AddAsync(It.IsAny<BookEntity>()), Times.Once);
        }
    }
}
