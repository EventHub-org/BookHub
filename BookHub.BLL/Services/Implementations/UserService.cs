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

        public async Task<ServiceResultType<PageDto<UserDto>>> GetPaginatedUsersAsync(int pageNumber, int pageSize)
        {
            if (pageSize <= 0)
            {
                return ServiceResultType<PageDto<UserDto>>.ErrorResult("Page size must be greater than zero.");
            }

            if (pageNumber <= 0)
            {
                return ServiceResultType<PageDto<UserDto>>.ErrorResult("Page number must be greater than zero.");
            }

            var (userEntities, totalElements) = await _userRepository.GetPagedAsync(pageSize, pageNumber);

            if (userEntities == null || !userEntities.Any())
            {
                return ServiceResultType<PageDto<UserDto>>.ErrorResult("No users found.");
            }

            var userDtos = _mapper.Map<List<UserDto>>(userEntities);
            var totalPages = (int)Math.Ceiling((double)totalElements / pageSize);
            
            Log.Information("Отримання всіх користувачів з пагінацією");
            
            var pageDto = new PageDto<UserDto>
            {
                Items = userDtos,
                TotalElements = totalElements,
                CurrentPage = pageNumber,
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
            
            Log.Information("Отримання користувача за id");
            
            var userDto = _mapper.Map<UserDto>(userEntity);
            return ServiceResultType<UserDto>.SuccessResult(userDto);
        }
    }
}
