using AutoMapper;
using BookHub.BLL.Services.Implementations;
using BookHub.DAL.DTO;
using BookHub.DAL.Entities;
using BookHub.DAL.Mappers;
using BookHub.DAL.Repositories.Interfaces;
using Moq;
using System.Linq.Expressions;

namespace BookHub.Tests.Services.Impl
{
    public class AuthServiceImplTest
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly IMapper _mapper;
        private readonly AuthService _authService;

        public AuthServiceImplTest()
        {
            _mockUserRepository = new Mock<IUserRepository>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<UserRegisterProfile>();

            });
            _mapper = config.CreateMapper();

            _authService = new AuthService(_mockUserRepository.Object, _mapper);
        }

        [Fact]
        public async Task RegisterUserAsync_ShouldReturnError_WhenUserWithEmailExists()
        {
            // Arrange
            var userRegisterDto = new UserRegisterDto
            {
                Name = "Jane Doe",
                Email = "jane@example.com",
                Password = "securepassword"
            };

            var existingUser = new UserEntity { UserId = 1, Email = "jane@example.com" };

            _mockUserRepository
                .Setup(repo => repo.GetByIdAsync(It.IsAny<Expression<Func<UserEntity, bool>>>()))
                .ReturnsAsync(existingUser);

            // Act
            var result = await _authService.RegisterUserAsync(userRegisterDto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("User with this email already exists.", result.ErrorMessage);
        }

        [Fact]
        public async Task RegisterUserAsync_ShouldRegisterUser_WhenUserWithEmailDoesNotExist()
        {
            // Arrange
            var userRegisterDto = new UserRegisterDto
            {
                Name = "John Doe",
                Email = "john@example.com",
                Password = "securepassword"
            };

            _mockUserRepository
                .Setup(repo => repo.GetByIdAsync(It.IsAny<Expression<Func<UserEntity, bool>>>()))
                .ReturnsAsync((UserEntity)null); // Користувач з таким email не існує

            _mockUserRepository
                .Setup(repo => repo.AddAsync(It.IsAny<UserEntity>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _authService.RegisterUserAsync(userRegisterDto);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(userRegisterDto.Name, result.Data.Name);

        }
    }
}
