using AutoMapper;
using BookHub.BLL.Services.Interfaces;
using BookHub.BLL.Utils;
using BookHub.DAL.DTO;
using BookHub.DAL.Entities;
using BookHub.DAL.Repositories.Interfaces;
using System.ComponentModel.DataAnnotations;


namespace BookHub.BLL.Services.Implementations
{
    public class FriendshipService : IFriendshipService
    {
        private readonly IFriendshipRepository _friendshipRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public FriendshipService(IFriendshipRepository friendshipRepository, IUserRepository userRepository, IMapper mapper)
        {
            _friendshipRepository = friendshipRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResultType<FriendshipDto>> AddFriendRequestAsync(FriendshipDto friendshipDto)
        {
            if (friendshipDto == null)
            {
                return ServiceResultType<FriendshipDto>.ErrorResult("Friendship data cannot be null");
            }

            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(friendshipDto);
            bool isValid = Validator.TryValidateObject(friendshipDto, validationContext, validationResults, true);

            if (!isValid)
            {
                return ServiceResultType<FriendshipDto>.ErrorResult("Validation failed: " + string.Join(", ", validationResults.Select(v => v.ErrorMessage)));
            }

            var friendshipEntity = _mapper.Map<FriendshipEntity>(friendshipDto);

            await _friendshipRepository.AddAsync(friendshipEntity);

            var resultDto = _mapper.Map<FriendshipDto>(friendshipEntity);
            return ServiceResultType<FriendshipDto>.SuccessResult(resultDto);
        }

        public async Task<ServiceResultType<FriendshipDto>> AcceptFriendRequestAsync(int user1Id, int user2Id)
        {
            var friendship = await _friendshipRepository.GetByIdAsync(f => f.User1Id == user1Id && f.User2Id == user2Id);

            if (friendship == null)
            {
                return ServiceResultType<FriendshipDto>.ErrorResult("Friendship not found.");
            }

            friendship.Status = "Accepted";
            await _friendshipRepository.UpdateAsync(friendship);

            var friendshipDto = _mapper.Map<FriendshipDto>(friendship);
            return ServiceResultType<FriendshipDto>.SuccessResult(friendshipDto);
        }

        public async Task<ServiceResultType<bool>> RemoveFriendshipAsync(int user1Id, int user2Id)
        {
            var friendship = await _friendshipRepository.GetByIdAsync(f => f.User1Id == user1Id && f.User2Id == user2Id);

            if (friendship == null)
            {
                return ServiceResultType<bool>.ErrorResult("Friendship not found.");
            }

            await _friendshipRepository.DeleteAsync(friendship);

            return ServiceResultType<bool>.SuccessResult(true); // Повертаємо true для успішного видалення
        }

        public async Task<ServiceResultType<FriendshipDto>> GetFriendshipAsync(int user1Id, int user2Id)
        {
            var friendship = await _friendshipRepository.GetByIdAsync(f => f.User1Id == user1Id && f.User2Id == user2Id);

            if (friendship == null)
            {
                return ServiceResultType<FriendshipDto>.ErrorResult("Friendship not found.");
            }

            var friendshipDto = _mapper.Map<FriendshipDto>(friendship);
            return ServiceResultType<FriendshipDto>.SuccessResult(friendshipDto);
        }
    }
}