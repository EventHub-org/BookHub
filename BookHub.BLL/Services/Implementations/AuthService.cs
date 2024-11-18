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

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(userRegistrerDto.Password);
            //var userEntity = _mapper.Map<UserEntity>(userRegistrerDto);
            var userEntity = new UserEntity
            {
                Name = userRegistrerDto.Name,
                Email = userRegistrerDto.Email,
                Password = passwordHash,
                ProfilePicture = "https://www.shutterstock.com/image-vector/user-profile-icon-vector-avatar-600nw-2247726673.jpg"
            };

            await _userRepository.AddAsync(userEntity);
            Log.Information($"Користувач з email {userRegistrerDto.Email} зареєстрований о {DateTime.UtcNow}.");

            var userDto = _mapper.Map<UserDto>(userEntity);

            return ServiceResultType<UserDto>.SuccessResult(userDto);
        }


        public async Task<ServiceResultType<UserDto>> LoginUserAsync(UserLoginDto userLoginDto)
        {
            // Перевірка, чи існує користувач
            var userEntity = await _userRepository.GetByIdAsync(u => u.Email == userLoginDto.Email);
            if (userEntity == null)
            {
                Log.Warning($"Спроба входу з неіснуючим email: {userLoginDto.Email} о {DateTime.UtcNow}.");
                return ServiceResultType<UserDto>.ErrorResult("Invalid email or password.");
            }

            // Перевірка пароля
            var passwordValid = BCrypt.Net.BCrypt.Verify(userLoginDto.Password, userEntity.Password);
            if (!passwordValid)
            {
                Log.Warning($"Невдала спроба входу для email: {userLoginDto.Email} о {DateTime.UtcNow}.");
                return ServiceResultType<UserDto>.ErrorResult("Invalid email or password.");
            }

            // Успішний логін
            Log.Information($"Користувач з email {userLoginDto.Email} успішно увійшов о {DateTime.UtcNow}.");
            var userDto = _mapper.Map<UserDto>(userEntity);

            return ServiceResultType<UserDto>.SuccessResult(userDto);
        }
    }
}
