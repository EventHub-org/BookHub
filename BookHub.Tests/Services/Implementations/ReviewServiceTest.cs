using AutoMapper;
using BookHub.BLL.Services.Implementations;
using BookHub.DAL.Entities;
using BookHub.DAL.Mappers;
using BookHub.DAL.Repositories.Interfaces;
using Moq;
using System.Linq.Expressions;

namespace BookHub.Tests.Services.Impl
{
    public class ReviewServiceImplTests
    {
        private readonly Mock<IRepository<ReviewEntity>> _mockRepository;
        private readonly Mock<IReviewRepository> _mockReviewRepository;
        private readonly IMapper _mapper;
        private readonly ReviewService _reviewService;

        public ReviewServiceImplTests()
        {
            _mockRepository = new Mock<IRepository<ReviewEntity>>();
            _mockReviewRepository = new Mock<IReviewRepository>();

            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<ReviewProfile>();
            });
            _mapper = config.CreateMapper();

            _reviewService = new ReviewService(_mockRepository.Object, _mockReviewRepository.Object, _mapper);
        }

        [Fact]
        public async Task GetPaginatedReviewsAsync_ShouldThrowArgumentException_WhenSizeIsZero()
        {
            // Arrange
            int size = 0;
            int page = 1;

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _reviewService.GetPaginatedReviewsAsync(size, page));
            Assert.Equal("Page size must be greater than zero. (Parameter 'size')", exception.Message);
        }

        [Fact]
        public async Task GetPaginatedReviewsAsync_ShouldThrowArgumentException_WhenPageIsZero()
        {
            // Arrange
            int size = 1;
            int page = 0;

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _reviewService.GetPaginatedReviewsAsync(size, page));
            Assert.Equal("Page number must be greater than zero. (Parameter 'page')", exception.Message);
        }

        [Fact]
        public async Task GetPaginatedReviewsAsync_ShouldReturnPageDto_WhenDataIsValid()
        {
            // Arrange
            int size = 2;
            int page = 1;

            var reviewEntities = new List<ReviewEntity>
            {
                new ReviewEntity(),
                new ReviewEntity()
            };

            var totalElements = 5;
            _mockReviewRepository
                .Setup(repo => repo.GetPagedAsync(size, page))
                .ReturnsAsync((reviewEntities, totalElements));

            // Act
            var result = await _reviewService.GetPaginatedReviewsAsync(size, page);

            // Assert
            Assert.NotNull(result.Data);
            Assert.Equal(totalElements, result.Data.TotalElements);
            Assert.Equal(page, result.Data.CurrentPage);
            Assert.Equal((int)Math.Ceiling((double)totalElements / size), result.Data.TotalPages);
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
            Assert.NotNull(result);
            Assert.Equal(reviewId, result.Data.Id);
        }

        [Fact]
        public async Task GetReviewAsync_ShouldThrowKeyNotFoundException_WhenReviewDoesNotExist()
        {
            // Arrange
            int reviewId = 1;
            _mockRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Expression<Func<ReviewEntity, bool>>>())).ReturnsAsync((ReviewEntity)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _reviewService.GetReviewAsync(reviewId));
            Assert.Equal($"Book with ID {reviewId} not found.", exception.Message);
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
    }
}
