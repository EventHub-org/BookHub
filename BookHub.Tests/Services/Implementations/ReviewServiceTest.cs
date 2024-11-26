using AutoMapper;
using BookHub.BLL.Services.Implementations;
using BookHub.DAL.DTO;
using BookHub.DAL.Entities;
using BookHub.DAL.Mappers;
using BookHub.DAL.Repositories.Interfaces;
using Moq;
using System.Linq.Expressions;
using BookHub.DAL.DTO;

namespace BookHub.Tests.Services.Impl
{
    public class ReviewServiceImplTests
    {
        private readonly Mock<IRepository<ReviewEntity>> _mockRepository;
        private readonly Mock<IReviewRepository> _mockReviewRepository;
        private readonly Mock<IBookRepository> _mockBookRepository; // Added mock for IBookRepository
        private readonly IMapper _mapper;
        private readonly ReviewService _reviewService;

        public ReviewServiceImplTests()
        {
            _mockRepository = new Mock<IRepository<ReviewEntity>>();
            _mockReviewRepository = new Mock<IReviewRepository>();
            _mockBookRepository = new Mock<IBookRepository>(); // Initialize the mock

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ReviewProfile>();
            });
            _mapper = config.CreateMapper();

            _reviewService = new ReviewService(_mockRepository.Object, _mockReviewRepository.Object, _mapper, _mockBookRepository.Object);
        }

        [Fact]
        public async Task GetPaginatedReviewsAsync_ShouldThrowArgumentException_WhenSizeIsZero()
        {
            // Arrange
            var pageable = new Pageable { Size = 0, Page = 1 };

            // Act & Assert
            var exception = await _reviewService.GetPaginatedReviewsAsync(pageable);
            Assert.Equal("Page size must be greater than zero.", exception.ErrorMessage);
        }

        [Fact]
        public async Task GetPaginatedReviewsAsync_ShouldThrowArgumentException_WhenPageIsNegative()
        {
            // Arrange
            var pageable = new Pageable { Size = 1, Page = -1 };

            // Act & Assert
            var exception = await _reviewService.GetPaginatedReviewsAsync(pageable);
            Assert.Equal("Page number must be greater than or equal to zero.", exception.ErrorMessage);
        }

        [Fact]
        public async Task GetPaginatedReviewsAsync_ShouldReturnPageDto_WhenDataIsValid()
        {
            // Arrange
            var pageable = new Pageable { Size = 2, Page = 1 };

            var reviewEntities = new List<ReviewEntity>
            {
                new ReviewEntity(),
                new ReviewEntity()
            };

            var totalElements = 5;
            _mockReviewRepository
                .Setup(repo => repo.GetPagedAsync(pageable))
                .ReturnsAsync((reviewEntities, totalElements));

            // Act
            var result = await _reviewService.GetPaginatedReviewsAsync(pageable);

            // Assert
            Assert.NotNull(result.Data);
            Assert.Equal(totalElements, result.Data.TotalElements);
            Assert.Equal(pageable.Page, result.Data.CurrentPage);
            Assert.Equal((int)Math.Ceiling((double)totalElements / pageable.Size), result.Data.TotalPages);
            Assert.Equal(reviewEntities.Count, result.Data.Items.Count);
        }

        [Fact]
        public async Task GetReviewAsync_ShouldReturnReviewDto_WhenReviewExists()
        {
            // Arrange
            int reviewId = 1;
            var reviewEntity = new ReviewEntity { Id = reviewId };
            _mockRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Expression<Func<ReviewEntity, bool>>>())).ReturnsAsync(reviewEntity);

            // Act
            var result = await _reviewService.GetReviewAsync(reviewId);

            // Assert
            Assert.NotNull(result.Data);
            Assert.Equal(reviewId, result.Data.Id);
        }

        [Fact]
        public async Task GetReviewAsync_ShouldThrowKeyNotFoundException_WhenReviewDoesNotExist()
        {
            // Arrange
            int reviewId = 1;
            _mockRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Expression<Func<ReviewEntity, bool>>>())).ReturnsAsync((ReviewEntity)null);

            // Act & Assert
            var exception = await _reviewService.GetReviewAsync(reviewId);
            Assert.Equal($"Review with ID {reviewId} not found.", exception.ErrorMessage);
        }

        [Fact]
        public async Task DeleteReviewAsync_ShouldCallRepositoryDelete_WhenReviewExists()
        {
            // Arrange
            int reviewId = 1;
            var reviewEntity = new ReviewEntity { Id = reviewId };
            _mockRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Expression<Func<ReviewEntity, bool>>>())).ReturnsAsync(reviewEntity);

            // Act
            await _reviewService.DeleteReviewAsync(reviewId);

            // Assert
            _mockRepository.Verify(repo => repo.DeleteAsync(reviewEntity), Times.Once);
        }


        [Fact]
        public async Task CreateReviewAsync_ShouldThrowArgumentNullException_WhenReviewDtoIsNull()
        {
            // Arrange
            ReviewDto reviewDto = null;

            var test = await _reviewService.CreateReviewAsync(reviewDto);
            // Act & Assert
            Assert.Equal("Review data cannot be null", test.ErrorMessage);

        }

        [Fact]
        public async Task CreateReviewAsync_ShouldThrowValidationException_WhenRatingIsTooShort()
        {
            // Arrange
            var reviewDto = new ReviewDto { Rating = 6, Comment = "Well" }; // Максимальний рейтинг 5

            // Act
            var test = await _reviewService.CreateReviewAsync(reviewDto);

            // Assert
            Assert.False(test.Success);
        }

        [Fact]
        public async Task CreateReviewAsync_ShouldReturnReviewDto_WhenReviewIsCreated()
        {
            // Arrange
            var reviewDto = new ReviewDto { Rating = 4, Comment = "Well" };
            var reviewEntity = _mapper.Map<ReviewEntity>(reviewDto);

            _mockReviewRepository
                .Setup(repo => repo.AddAsync(It.IsAny<ReviewEntity>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _reviewService.CreateReviewAsync(reviewDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(reviewDto.Rating, result.Data.Rating);
        }
    }
}
