using AutoMapper;
using BookHub.BLL.Services.Interfaces;
using BookHub.BLL.Utils;
using BookHub.DAL.DTO;
using BookHub.DAL.Entities;
using BookHub.DAL.Repositories.Interfaces;
using Serilog;

namespace BookHub.BLL.Services.Implementations
{

    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public AuthService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResultType<UserDto>> RegisterUserAsync(UserRegisterDto userRegistrerDto)
        {

            var existingUser = await _userRepository.GetByIdAsync(u => u.Email == userRegistrerDto.Email);
            if (existingUser != null)
            {
                return ServiceResultType<UserDto>.ErrorResult("User with this email already exists.");
            }

            var userEntity = _mapper.Map<UserEntity>(userRegistrerDto);

            await _userRepository.AddAsync(userEntity);
            Log.Information($"Користувач з email {userRegistrerDto.Email} зареєстрований о {DateTime.UtcNow}.");

            var userDto = _mapper.Map<UserDto>(userEntity);

            return ServiceResultType<UserDto>.SuccessResult(userDto);
        }
    }
}
