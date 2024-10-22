using AutoMapper;
using BookHub.BLL.Services.Implementations;
using BookHub.BLL.Utils;
using BookHub.DAL.DTO;
using BookHub.DAL.Entities;
using BookHub.DAL.Mappers;
using BookHub.DAL.Repositories.Interfaces;
using Moq;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace BookHub.Tests.Services.Impl
{
    public class FriendshipServiceImplTests
    {
        private readonly Mock<IFriendshipRepository> _mockFriendshipRepository;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly IMapper _mapper;
        private readonly FriendshipService _friendshipService;

        public FriendshipServiceImplTests()
        {
            _mockFriendshipRepository = new Mock<IFriendshipRepository>();
            _mockUserRepository = new Mock<IUserRepository>();

            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<FriendshipProfile>();
                cfg.AddProfile<UserProfile>();
            });
            _mapper = config.CreateMapper();

            _friendshipService = new FriendshipService(_mockFriendshipRepository.Object, _mockUserRepository.Object, _mapper);
        }

        [Fact]
        public async Task AddFriendRequestAsync_ShouldReturnError_WhenFriendshipDtoIsNull()
        {
            // Arrange
            FriendshipDto friendshipDto = null;

            // Act
            var result = await _friendshipService.AddFriendRequestAsync(friendshipDto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Friendship data cannot be null", result.ErrorMessage);
        }


        [Fact]
        public async Task AddFriendRequestAsync_ShouldReturnSuccess_WhenRequestIsAdded()
        {
            // Arrange
            var friendshipDto = new FriendshipDto { User1Id = 1, User2Id = 2, Status = "Pending" };
            var friendshipEntity = new FriendshipEntity { User1Id = 1, User2Id = 2, Status = "Pending" };

            _mockFriendshipRepository.Setup(repo => repo.AddAsync(It.IsAny<FriendshipEntity>()))
                .Callback<FriendshipEntity>(entity => {
                    friendshipEntity.User1Id = entity.User1Id;
                    friendshipEntity.User2Id = entity.User2Id;
                    friendshipEntity.Status = entity.Status;
                });

            // Act
            var result = await _friendshipService.AddFriendRequestAsync(friendshipDto);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(friendshipDto.User1Id, result.Data.User1Id);
            Assert.Equal(friendshipDto.User2Id, result.Data.User2Id);
            Assert.Equal(friendshipDto.Status, result.Data.Status);
        }

        [Fact]
        public async Task AcceptFriendRequestAsync_ShouldReturnError_WhenFriendshipNotFound()
        {
            // Arrange
            var user1Id = 1;
            var user2Id = 2;

            _mockFriendshipRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Expression<Func<FriendshipEntity, bool>>>()))
                .ReturnsAsync((FriendshipEntity)null);

            // Act
            var result = await _friendshipService.AcceptFriendRequestAsync(user1Id, user2Id);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Friendship not found.", result.ErrorMessage);
        }

        [Fact]
        public async Task AcceptFriendRequestAsync_ShouldReturnSuccess_WhenRequestIsAccepted()
        {
            // Arrange
            var user1Id = 1;
            var user2Id = 2;
            var friendshipEntity = new FriendshipEntity { User1Id = user1Id, User2Id = user2Id, Status = "Pending" };

            _mockFriendshipRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Expression<Func<FriendshipEntity, bool>>>()))
                .ReturnsAsync(friendshipEntity);

            // Act
            var result = await _friendshipService.AcceptFriendRequestAsync(user1Id, user2Id);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Accepted", friendshipEntity.Status); // Verify the status is updated
            _mockFriendshipRepository.Verify(repo => repo.UpdateAsync(friendshipEntity), Times.Once);
        }

        [Fact]
        public async Task RemoveFriendshipAsync_ShouldReturnError_WhenFriendshipNotFound()
        {
            // Arrange
            var user1Id = 1;
            var user2Id = 2;

            _mockFriendshipRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Expression<Func<FriendshipEntity, bool>>>()))
                .ReturnsAsync((FriendshipEntity)null);

            // Act
            var result = await _friendshipService.RemoveFriendshipAsync(user1Id, user2Id);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Friendship not found.", result.ErrorMessage);
        }

        [Fact]
        public async Task RemoveFriendshipAsync_ShouldReturnSuccess_WhenFriendshipIsRemoved()
        {
            // Arrange
            var user1Id = 1;
            var user2Id = 2;
            var friendshipEntity = new FriendshipEntity { User1Id = user1Id, User2Id = user2Id };

            _mockFriendshipRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Expression<Func<FriendshipEntity, bool>>>()))
                .ReturnsAsync(friendshipEntity);

            // Act
            var result = await _friendshipService.RemoveFriendshipAsync(user1Id, user2Id);

            // Assert
            Assert.True(result.Success);
            _mockFriendshipRepository.Verify(repo => repo.DeleteAsync(friendshipEntity), Times.Once);
        }

        [Fact]
        public async Task GetFriendshipAsync_ShouldReturnError_WhenFriendshipNotFound()
        {
            // Arrange
            var user1Id = 1;
            var user2Id = 2;

            _mockFriendshipRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Expression<Func<FriendshipEntity, bool>>>()))
                .ReturnsAsync((FriendshipEntity)null);

            // Act
            var result = await _friendshipService.GetFriendshipAsync(user1Id, user2Id);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Friendship not found.", result.ErrorMessage);
        }

        [Fact]
        public async Task GetFriendshipAsync_ShouldReturnSuccess_WhenFriendshipIsFound()
        {
            // Arrange
            var user1Id = 1;
            var user2Id = 2;
            var friendshipEntity = new FriendshipEntity { User1Id = user1Id, User2Id = user2Id, Status = "Accepted" };

            _mockFriendshipRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Expression<Func<FriendshipEntity, bool>>>()))
                .ReturnsAsync(friendshipEntity);

            // Act
            var result = await _friendshipService.GetFriendshipAsync(user1Id, user2Id);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(friendshipEntity.User1Id, result.Data.User1Id);
            Assert.Equal(friendshipEntity.User2Id, result.Data.User2Id);
            Assert.Equal(friendshipEntity.Status, result.Data.Status);
        }
    }
}
