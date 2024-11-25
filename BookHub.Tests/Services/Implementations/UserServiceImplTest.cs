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
    public class UserServiceImplTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly IMapper _mapper;
        private readonly UserService _userService;

        public UserServiceImplTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<UserProfile>();
                

            });
            _mapper = config.CreateMapper();

            _userService = new UserService(_mockUserRepository.Object, _mapper);
        }

        [Fact]
        public async Task GetPaginatedUsers_ShouldReturnError_WhenPageSizeIsZero()
        {
            // Arrange
            var pageable = new Pageable { Size = 0, Page = 1 };

            // Act
            var result = await _userService.GetPaginatedUsersAsync(pageable);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Page size must be greater than zero.", result.ErrorMessage);
        }

        [Fact]
        public async Task GetPaginatedUsers_ShouldReturnError_WhenPageNumberIsZero()
        {
            // Arrange
            var pageable = new Pageable { Size = 1, Page = -1 };

            // Act
            var result = await _userService.GetPaginatedUsersAsync(pageable);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Page number must be greater than or equal to zero.", result.ErrorMessage);
        }

        [Fact]
        public async Task GetPaginatedUsers_ShouldReturnPageDto_WhenDataIsValid()
        {
            // Arrange
            var pageable = new Pageable { Size = 2, Page = 1 };

            var userEntities = new List<UserEntity>
            {
                new UserEntity { UserId = 1, Name = "User 1" },
                new UserEntity { UserId = 2, Name = "User 2" }
            };

            var totalElements = 5;
            _mockUserRepository
                .Setup(repo => repo.GetPagedAsync(pageable))
                .ReturnsAsync((userEntities, totalElements));

            // Act
            var result = await _userService.GetPaginatedUsersAsync(pageable);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(totalElements, result.Data.TotalElements);
            Assert.Equal(pageable.Page, result.Data.CurrentPage);
            Assert.Equal((int)Math.Ceiling((double)totalElements / pageable.Size), result.Data.TotalPages);
            Assert.Equal(userEntities.Count, result.Data.Items.Count);
        }

        [Fact]
        public async Task GetPaginatedUsers_ShouldReturnError_WhenNoUsersFound()
        {
            // Arrange
            var pageable = new Pageable { Size = 2, Page = 1 };

            var userEntities = new List<UserEntity>();
            var totalElements = 0;
            _mockUserRepository
                .Setup(repo => repo.GetPagedAsync(pageable))
                .ReturnsAsync((userEntities, totalElements));

            // Act
            var result = await _userService.GetPaginatedUsersAsync(pageable);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("No users found.", result.ErrorMessage);
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnError_WhenUserNotFound()
        {
            // Arrange
            int userId = 999;
            _mockUserRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Expression<Func<UserEntity, bool>>>())).ReturnsAsync((UserEntity)null);

            // Act
            var result = await _userService.GetUserByIdAsync(userId);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("User not found.", result.ErrorMessage);
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnUserDto_WhenUserExists()
        {
            // Arrange
            int userId = 1;
            var userEntity = new UserEntity
            {
                UserId = userId,
                Name = "John Doe",
                ProfilePicture = "http://example.com/profile.jpg"
            };

            _mockUserRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Expression<Func<UserEntity, bool>>>())).ReturnsAsync(userEntity);

            // Act
            var result = await _userService.GetUserByIdAsync(userId);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            //Assert.Equal(userEntity.UserId, result.Data.Id);
            Assert.Equal(userEntity.Name, result.Data.Name);
            Assert.Equal(userEntity.ProfilePicture, result.Data.ProfilePictureUrl);
        }

        
    }
}
