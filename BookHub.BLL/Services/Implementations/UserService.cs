﻿using AutoMapper;
using BookHub.BLL.Services.Interfaces;
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

        public async Task<PageDto<UserDto>> GetPaginatedUsersAsync(int pageNumber, int pageSize)
        {
            if (pageSize <= 0)
            {
                throw new ArgumentException("Page size must be greater than zero.", nameof(pageSize));
            }

            if (pageNumber <= 0)
            {
                throw new ArgumentException("Page number must be greater than zero.", nameof(pageNumber));
            }

            var (userEntities, totalElements) = await _userRepository.GetPagedAsync(pageSize, pageNumber);

            var userDtos = _mapper.Map<List<UserDto>>(userEntities);

            var totalPages = (int)Math.Ceiling((double)totalElements / pageSize);

            Log.Information("Отримання всіх користувачів з пагінацією");

            return new PageDto<UserDto>
            {
                Items = userDtos,
                TotalElements = totalElements,
                CurrentPage = pageNumber,
                TotalPages = totalPages
            };
        }
        public async Task<UserDto> GetUserByIdAsync(int userId)
        {
            var userEntity = await _userRepository.GetByIdAsync(u => u.UserId == userId);
            if (userEntity == null)
            {
                throw new Exception("User not found.");
            }

            Log.Information("Отримання користувача за id");

            return _mapper.Map<UserDto>(userEntity);
        }
    }
}
