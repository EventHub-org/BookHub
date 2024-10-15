using AutoMapper;
using BookHub.BLL.Services.Implementations;
using BookHub.DAL.DTO;
using BookHub.DAL.Entities;
using BookHub.DAL.Repositories.Interfaces;
using Moq;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Xunit;

namespace BookHub.Tests.Services.Impl
{
    public class CollectionServiceImplTest
    {
        private readonly Mock<IRepository<CollectionEntity>> _mockCollectionRepository;
        private readonly IMapper _mapper;
        private readonly CollectionService _collectionService;

        public CollectionServiceImplTest()
        {
            _mockCollectionRepository = new Mock<IRepository<CollectionEntity>>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CollectionEntity, CollectionDto>().ReverseMap();
            });
            _mapper = config.CreateMapper();

            _collectionService = new CollectionService(_mockCollectionRepository.Object, _mapper);
        }

        [Fact]
        public async Task CreateCollectionAsync_ShouldThrowArgumentNullException_WhenCollectionDtoIsNull()
        {
            // Arrange
            CollectionDto collectionDto = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _collectionService.CreateCollectionAsync(collectionDto));
        }

        [Fact]
        public async Task CreateCollectionAsync_ShouldThrowValidationException_WhenNameIsTooShort()
        {
            // Arrange
            var collectionDto = new CollectionDto { Name = "A" }; // Ім'я занадто коротке

            // Act
            var exception = await Assert.ThrowsAsync<ValidationException>(() => _collectionService.CreateCollectionAsync(collectionDto));

            // Assert
            Assert.Equal("Validation failed: Name must be between 2 and 50 characters.", exception.Message);
        }

        [Fact]
        public async Task CreateCollectionAsync_ShouldThrowValidationException_WhenNameIsTooLong()
        {
            // Arrange
            var collectionDto = new CollectionDto { Name = new string('A', 51) }; // Ім'я занадто довге

            // Act
            var exception = await Assert.ThrowsAsync<ValidationException>(() => _collectionService.CreateCollectionAsync(collectionDto));

            // Assert
            Assert.Equal("Validation failed: Name must be between 2 and 50 characters.", exception.Message);
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
            Assert.Equal(collectionDto.Name, result.Name);
        }
    }
}
