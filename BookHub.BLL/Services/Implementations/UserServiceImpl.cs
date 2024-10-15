﻿using AutoMapper;
using BookHub.BLL.Services.Interfaces;
using BookHub.DAL.DTO;
using BookHub.DAL.Entities;
using BookHub.DAL.Repositories.Interfaces;


namespace BookHub.BLL.Services.Implementations
{
    public class UserServiceImpl : UserService
    {
        private readonly IUserRepository<UserEntity> _userRepository;
        private readonly IMapper _mapper;

        public UserServiceImpl(IUserRepository<UserEntity> userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public PageDto<UserDto> GetPaginatedUsers(int pageNumber, int pageSize)
        {
            if (pageSize <= 0)
            {
                throw new ArgumentException("Page size must be greater than zero.", nameof(pageSize));
            }

            if (pageNumber <= 0)
            {
                throw new ArgumentException("Page number must be greater than zero.", nameof(pageNumber));
            }

            var (userEntities, totalElements) = _userRepository.GetPagedAsync(pageSize, pageNumber).GetAwaiter().GetResult();

            var userDtos = _mapper.Map<List<UserDto>>(userEntities);

            var totalPages = (int)Math.Ceiling((double)totalElements / pageSize);

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
            var userEntity = await _userRepository.GetByIdAsync(userId);
            if (userEntity == null)
            {
                throw new Exception("User not found.");
            }
            return _mapper.Map<UserDto>(userEntity);
        }
    }
}
