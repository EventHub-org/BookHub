using AutoMapper;
using BookHub.BLL.Services.Interfaces;
using BookHub.BLL.Utils;
using BookHub.DAL.DTO;
using BookHub.DAL.Entities;
using BookHub.DAL.Repositories.Interfaces;
using Serilog;
using System.ComponentModel.DataAnnotations;

namespace BookHub.BLL.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ISessionService _sessionService;

        public AuthService(IUserRepository userRepository, IMapper mapper, ISessionService sessionService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _sessionService = sessionService;
        }

        public async Task<ServiceResultType<UserDto>> RegisterUserAsync(UserRegisterDto userRegisterDto)
        {
            var existingUser = await _userRepository.GetByIdAsync(u => u.Email == userRegisterDto.Email);
            if (existingUser != null)
            {
                return ServiceResultType<UserDto>.ErrorResult("User with this email already exists.");
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(userRegisterDto.Password);
            var userEntity = new UserEntity
            {
                Name = userRegisterDto.Name,
                Email = userRegisterDto.Email,
                Password = passwordHash,
                ProfilePicture = "https://www.shutterstock.com/image-vector/user-profile-icon-vector-avatar-600nw-2247726673.jpg"
            };

            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(userEntity);
            bool isValid = Validator.TryValidateObject(userEntity, validationContext, validationResults, true);

            if (!isValid)
            {
                return ServiceResultType<UserDto>.ErrorResult("Validation failed: " + string.Join(", ", validationResults.Select(v => v.ErrorMessage)));
            }
            await _userRepository.AddAsync(userEntity);
            Log.Information($"User with email {userRegisterDto.Email} registered at {DateTime.UtcNow}.");

            var userDto = _mapper.Map<UserDto>(userEntity);
            return ServiceResultType<UserDto>.SuccessResult(userDto);
        }

        public async Task<ServiceResultType<UserDto>> LoginUserAsync(UserLoginDto userLoginDto)
        {
            // Check if user exists
            var userEntity = await _userRepository.GetByIdAsync(u => u.Email == userLoginDto.Email);
            if (userEntity == null)
            {
                Log.Warning($"Login attempt with non-existent email: {userLoginDto.Email} at {DateTime.UtcNow}.");
                return ServiceResultType<UserDto>.ErrorResult("Invalid email or password.");
            }

            // Validate password
            var passwordValid = BCrypt.Net.BCrypt.Verify(userLoginDto.Password, userEntity.Password);
            if (!passwordValid)
            {
                Log.Warning($"Failed login attempt for email: {userLoginDto.Email} at {DateTime.UtcNow}.");
                return ServiceResultType<UserDto>.ErrorResult("Invalid email or password.");
            }

            // Successful login
            Log.Information($"User with email {userLoginDto.Email} logged in successfully at {DateTime.UtcNow}.");
            var userDto = _mapper.Map<UserDto>(userEntity);

            // Update session
            _sessionService.SetCurrentUser(userDto);

            return ServiceResultType<UserDto>.SuccessResult(userDto);
        }
    }
}
