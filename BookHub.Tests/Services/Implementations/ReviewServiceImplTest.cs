using AutoMapper;
using BookHub.BLL.Services.Implementations;
using BookHub.DAL.Entities;
using BookHub.DAL.Mappers;
using BookHub.DAL.Repositories.Interfaces;
using Moq;

namespace BookHub.Tests.Services.Impl
{
    public class ReviewServiceImplTests
    {
        private readonly Mock<IReviewRepository<ReviewEntity>> _mockReviewRepository;
        private readonly IMapper _mapper;
        private readonly ReviewServiceImpl _reviewService;

        public ReviewServiceImplTests()
        {
            _mockReviewRepository = new Mock<IReviewRepository<ReviewEntity>>();

            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<ReviewProfile>();
            });
            _mapper = config.CreateMapper();

            _reviewService = new ReviewServiceImpl(_mockReviewRepository.Object, _mapper);
        }

        [Fact]
        public void GetPageDto_ShouldThrowArgumentException_WhenSizeIsZero()
        {
            // Arrange
            int size = 0;
            int page = 1;

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _reviewService.GetPaginatedReviews(size, page));
            Assert.Equal("Page size must be greater than zero. (Parameter 'size')", exception.Message);
        }

        [Fact]
        public void GetPageDto_ShouldThrowArgumentException_WhenPageIsZero()
        {
            // Arrange
            int size = 1;
            int page = 0;

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _reviewService.GetPaginatedReviews(size, page));
            Assert.Equal("Page number must be greater than zero. (Parameter 'page')", exception.Message);
        }

        [Fact]
        public void GetPageDto_ShouldReturnPageDto_WhenDataIsValid()
        {
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
            var result = _reviewService.GetPaginatedReviews(size, page);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(totalElements, result.TotalElements);
            Assert.Equal(page, result.CurrentPage);
            Assert.Equal((int)Math.Ceiling((double)totalElements / size), result.TotalPages);
            Assert.Equal(reviewEntities.Count, result.Items.Count);
        }
    }
}
