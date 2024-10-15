using AutoMapper;
using BookHub.BLL.Services.Implementations;
using BookHub.DAL.DTO;
using BookHub.DAL.Entities;
using BookHub.DAL.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using System.Threading.Tasks;

namespace BookHub.Tests.Services.Impl
{
    public class ReadingProgressServiceImplTest
    {
        private readonly Mock<IReadingProgressRepository<ReadingProgressEntity>> _mockReadingProgressRepository;
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<ReadingProgressServiceImpl>> _mockLogger;
        private readonly ReadingProgressServiceImpl _readingProgressService;

        public ReadingProgressServiceImplTest()
        {
            _mockReadingProgressRepository = new Mock<IReadingProgressRepository<ReadingProgressEntity>>();

            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<ReadingProgressProfile>();
            });
            _mapper = config.CreateMapper();

            _readingProgressService = new ReadingProgressServiceImpl(
                _mockReadingProgressRepository.Object,
                _mapper
            );
        }

        [Fact]
        public async Task CreateReadingProgress_ShouldAddReadingProgress_WhenValidDTOIsProvided()
        {
            // Arrange
            var readingProgressDTO = new ReadingProgressDTO
            {
                UserId = 1,
                BookId = 2,
                CurrentPage = 100
            };

            _mockReadingProgressRepository
                .Setup(repo => repo.AddAsync(It.IsAny<ReadingProgressEntity>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _readingProgressService.createReadingProgress(readingProgressDTO);

            // Assert
            _mockReadingProgressRepository.Verify(repo => repo.AddAsync(It.IsAny<ReadingProgressEntity>()), Times.Once);

            Assert.NotNull(result);
            Assert.Equal(readingProgressDTO.UserId, result.UserId);
            Assert.Equal(readingProgressDTO.BookId, result.BookId);
            Assert.Equal(readingProgressDTO.CurrentPage, result.CurrentPage);
        }

        [Fact]
        public async Task CreateReadingProgress_ShouldThrowNullReferenceException_WhenDTOIsNull()
        {
            // Act & Assert
            var exception = await Assert.ThrowsAsync<NullReferenceException>(() => _readingProgressService.createReadingProgress(null));

            Assert.Equal("ReadingProgressDTO cannot be null", exception.Message);
        }

        [Fact]
        public async Task CreateReadingProgress_ShouldThrowArgumentException_WhenCurrentPageIsNegative()
        {
            // Arrange
            var readingProgressDTO = new ReadingProgressDTO
            {
                UserId = 1,
                BookId = 2,
                CurrentPage = -10
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _readingProgressService.createReadingProgress(readingProgressDTO));

            Assert.Equal("Validation failed: Current page cannot be negative.", exception.Message);
        }

        [Fact]
        public async Task CreateReadingProgress_ShouldLogError_WhenRepositoryThrowsException()
        {
            // Arrange
            var readingProgressDTO = new ReadingProgressDTO
            {
                UserId = 1,
                BookId = 2,
                CurrentPage = 100
            };

            _mockReadingProgressRepository
                .Setup(repo => repo.AddAsync(It.IsAny<ReadingProgressEntity>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _readingProgressService.createReadingProgress(readingProgressDTO));

            Assert.Equal("Database error", exception.Message);
        }

        [Fact]
        public async Task CreateReadingProgress_ShouldThrowArgumentException_WhenUserIdIsMissing()
        {
            // Arrange
            var readingProgressDTO = new ReadingProgressDTO
            {
                BookId = 2,
                CurrentPage = 50,
                UserId = null  // Simulate missing UserId
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _readingProgressService.createReadingProgress(readingProgressDTO));

            Assert.Equal("Validation failed: User ID is required.", exception.Message);
        }
    }
}
