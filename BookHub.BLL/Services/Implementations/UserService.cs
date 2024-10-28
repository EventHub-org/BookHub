using AutoMapper;
using BookHub.BLL.Services.Interfaces;
using BookHub.BLL.Utils;
using BookHub.DAL.DTO;
using BookHub.DAL.Repositories.Interfaces;
using Serilog;

namespace BookHub.BLL.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResultType<PageDto<UserDto>>> GetPaginatedUsersAsync(Pageable pageable)
        {
            var validationResult = PageUtils.ValidatePage<BookDto>(pageable);

            if (!validationResult.Success)
            {
                return ServiceResultType<PageDto<UserDto>>.ErrorResult(validationResult.ErrorMessage);
            }


            var (userEntities, totalElements) = await _userRepository.GetPagedAsync(pageable);

            if (userEntities == null || !userEntities.Any())
            {
                return ServiceResultType<PageDto<UserDto>>.ErrorResult("No users found.");
            }

            var userDtos = _mapper.Map<List<UserDto>>(userEntities);
            var totalPages = (int)Math.Ceiling((double)totalElements / pageable.Size);

            Log.Information($"Ініціалізовано отримання всіх користувачів з пагінацією о {DateTime.UtcNow}.");

            var pageDto = new PageDto<UserDto>
            {
                Items = userDtos,
                TotalElements = totalElements,
                CurrentPage = pageable.Page,
                TotalPages = totalPages
            };

            return ServiceResultType<PageDto<UserDto>>.SuccessResult(pageDto);
        }
        public async Task<ServiceResultType<UserDto>> GetUserByIdAsync(int userId)
        {
            var userEntity = await _userRepository.GetByIdAsync(u => u.UserId == userId);

            if (userEntity == null)
            {
                return ServiceResultType<UserDto>.ErrorResult("User not found.");
            }
            
            var userDto = _mapper.Map<UserDto>(userEntity);
            Log.Information($"Ініціалізовано отримання користувача за Id з UserId: {userId} о {DateTime.UtcNow}.");
            return ServiceResultType<UserDto>.SuccessResult(userDto);
        }
    }
}
