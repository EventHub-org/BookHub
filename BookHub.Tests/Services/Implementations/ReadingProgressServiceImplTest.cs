﻿using AutoMapper;
using BookHub.BLL.Services.Implementations;
using BookHub.DAL.DTO;
using BookHub.DAL.Entities;
using BookHub.DAL.Repositories.Interfaces;
using Moq;
using Xunit;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BookHub.Tests.Services.Impl
{
    public class ReadingProgressServiceImplTest
    {
        private readonly Mock<IReadingProgressRepository> _mockRepository;
        private readonly IMapper _mapper;
        private readonly ReadingProgressServiceImpl _service;

        public ReadingProgressServiceImplTest()
        {
            _mockRepository = new Mock<IReadingProgressRepository>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReadingProgressDTO, ReadingProgressEntity>();
                cfg.CreateMap<ReadingProgressEntity, ReadingProgressResponseDTO>();
            });
            _mapper = config.CreateMapper();

            _service = new ReadingProgressServiceImpl(_mockRepository.Object, _mapper);
        }

        [Fact]
        public async Task CreateReadingProgress_ShouldReturnSuccess_WhenValidDTOIsProvided()
        {
            var dto = new ReadingProgressDTO { UserId = 1, BookId = 2, CurrentPage = 100 };
            _mockRepository.Setup(repo => repo.AddAsync(It.IsAny<ReadingProgressEntity>())).Returns(Task.CompletedTask);

            var result = await _service.CreateReadingProgressAsync(dto);

            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(dto.UserId, result.Data.UserId);
        }

        [Fact]
        public async Task CreateReadingProgress_ShouldReturnError_WhenDTOIsNull()
        {
            var result = await _service.CreateReadingProgressAsync(null);

            Assert.False(result.Success);
            Assert.Equal("Reading progress data cannot be null", result.ErrorMessage);
        }

        [Fact]
        public async Task GetReadingProgressById_ShouldReturnData_WhenEntityExists()
        {
            var entity = new ReadingProgressEntity { Id = 1, UserId = 1, BookId = 2, CurrentPage = 50 };

            _mockRepository
                .Setup(repo => repo.GetByIdAsync(It.IsAny<Expression<Func<ReadingProgressEntity, bool>>>()))
                .ReturnsAsync(entity);

            var result = await _service.GetReadingProgressByIdAsync(1);

            Assert.True(result.Success);
            Assert.Equal(entity.Id, result.Data.Id);
        }

        [Fact]
        public async Task GetReadingProgressById_ShouldReturnError_WhenEntityDoesNotExist()
        {
            _mockRepository
                .Setup(repo => repo.GetByIdAsync(It.IsAny<Expression<Func<ReadingProgressEntity, bool>>>()))
                .ReturnsAsync((ReadingProgressEntity)null);

            var result = await _service.GetReadingProgressByIdAsync(1);

            Assert.False(result.Success);
            Assert.Equal("Reading progress not found", result.ErrorMessage);
        }

        [Fact]
        public async Task DeleteReadingProgress_ShouldReturnSuccess_WhenEntityExists()
        {
            var entity = new ReadingProgressEntity { Id = 1 };

            _mockRepository
                .Setup(repo => repo.GetByIdAsync(It.IsAny<Expression<Func<ReadingProgressEntity, bool>>>()))
                .ReturnsAsync(entity);

            _mockRepository.Setup(repo => repo.DeleteAsync(entity)).Returns(Task.CompletedTask);

            var result = await _service.DeleteReadingProgressAsync(1);

            Assert.True(result.Success);
            Assert.True(result.Data);
        }

        [Fact]
        public async Task DeleteReadingProgress_ShouldReturnError_WhenEntityDoesNotExist()
        {
            _mockRepository
                .Setup(repo => repo.GetByIdAsync(It.IsAny<Expression<Func<ReadingProgressEntity, bool>>>()))
                .ReturnsAsync((ReadingProgressEntity)null);

            var result = await _service.DeleteReadingProgressAsync(1);

            Assert.False(result.Success);
            Assert.Equal("Reading progress not found", result.ErrorMessage);
        }
    }
}
