using AutoMapper;
using BookHub.BLL.Services.Implementations;
using BookHub.DAL.DTO;
using BookHub.DAL.Entities;
using BookHub.DAL.Mappers;
using BookHub.DAL.Repositories.Interfaces;
using Moq;


namespace BookHub.Tests.Services.Impl
{
    public class UserServiceImplTests
    {
        private readonly Mock<IUserRepository<UserEntity>> _mockUserRepository;
        private readonly IMapper _mapper;
        private readonly UserServiceImpl _userService;

        public UserServiceImplTests()
        {
            _mockUserRepository = new Mock<IUserRepository<UserEntity>>();

            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<UserProfile>();
            });
            _mapper = config.CreateMapper();

            _userService = new UserServiceImpl(_mockUserRepository.Object, _mapper);
        }

        [Fact]
        public void GetPaginatedUsers_ShouldThrowArgumentException_WhenPageSizeIsZero()
        {
            // Arrange
            int pageSize = 0;
            int pageNumber = 1;

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _userService.GetPaginatedUsers(pageNumber, pageSize));
            Assert.Equal("Page size must be greater than zero. (Parameter 'pageSize')", exception.Message);
        }

        [Fact]
        public void GetPaginatedUsers_ShouldThrowArgumentException_WhenPageNumberIsZero()
        {
            // Arrange
            int pageSize = 1;
            int pageNumber = 0;

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _userService.GetPaginatedUsers(pageNumber, pageSize));
            Assert.Equal("Page number must be greater than zero. (Parameter 'pageNumber')", exception.Message);
        }



        [Fact]
        public void GetPaginatedUsers_ShouldReturnPageDto_WhenDataIsValid()
        {
            int pageSize = 2;
            int pageNumber = 1;

            var userEntities = new List<UserEntity>
            {
                new UserEntity(),
                new UserEntity()
            };

            var totalElements = 5;
            _mockUserRepository
                .Setup(repo => repo.GetPagedAsync(pageSize, pageNumber))
                .ReturnsAsync((userEntities, totalElements));

            // Act
            var result = _userService.GetPaginatedUsers(pageNumber, pageSize);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(totalElements, result.TotalElements);
            Assert.Equal(pageNumber, result.CurrentPage);
            Assert.Equal((int)Math.Ceiling((double)totalElements / pageSize), result.TotalPages);
            Assert.Equal(userEntities.Count, result.Items.Count);
        }

        [Fact]
        public async void GetUserByIdAsync_ShouldThrowException_WhenUserNotFound()
        {
            // Arrange
            int userId = 999;
            _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync((UserEntity)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _userService.GetUserByIdAsync(userId));
            Assert.Equal("User not found.", exception.Message);
        }

        [Fact]
        public async void GetUserByIdAsync_ShouldReturnUserDto_WhenUserExists()
        {
            // Arrange
            int userId = 1;
            var userEntity = new UserEntity
            {
                UserId = userId,
                Name = "John Doe",
                Email = "john.doe@example.com",
                ProfilePicture = "http://example.com/profile.jpg"
            };

            _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(userEntity);

            // Act
            var result = await _userService.GetUserByIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userEntity.UserId, result.Id);
            Assert.Equal(userEntity.Name, result.Name);
            Assert.Equal(userEntity.Email, result.Email);
            Assert.Equal(userEntity.ProfilePicture, result.ProfilePictureUrl);
        }
    }
}
