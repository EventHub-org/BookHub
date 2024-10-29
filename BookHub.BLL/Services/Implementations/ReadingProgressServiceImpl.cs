using AutoMapper;
using BookHub.BLL.Services.Interfaces;
using BookHub.DAL.Entities;
using BookHub.DAL.Repositories.Interfaces;
using BookHub.BLL.Utils;
using System.ComponentModel.DataAnnotations;
using Serilog;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BookHub.BLL.Services.Implementations
{
    public class ReadingProgressServiceImpl : IReadingProgressService
    {
        private readonly IReadingProgressRepository _readingProgressRepository;
        private readonly IMapper _mapper;

        public ReadingProgressServiceImpl(
            IReadingProgressRepository readingProgressRepository,
            IMapper mapper)
        {
            _readingProgressRepository = readingProgressRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResultType<ReadingProgressResponseDTO>> CreateReadingProgressAsync(ReadingProgressDTO readingProgressDTO)
        {
            if (readingProgressDTO == null)
                return ServiceResultType<ReadingProgressResponseDTO>.ErrorResult("Reading progress data cannot be null");

            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(readingProgressDTO);
            bool isValid = Validator.TryValidateObject(readingProgressDTO, validationContext, validationResults, true);

            if (!isValid)
            {
                string errors = string.Join(", ", validationResults.Select(v => v.ErrorMessage));
                return ServiceResultType<ReadingProgressResponseDTO>.ErrorResult($"Validation failed: {errors}");
            }

            var entity = _mapper.Map<ReadingProgressEntity>(readingProgressDTO);
            await _readingProgressRepository.AddAsync(entity);

            var responseDto = _mapper.Map<ReadingProgressResponseDTO>(entity);
            Log.Information($"Ініціалізовано створення прогресу читання з Id: {entity.Id} о {DateTime.UtcNow}.");

            return ServiceResultType<ReadingProgressResponseDTO>.SuccessResult(responseDto);
        }


        public async Task<ServiceResultType<ReadingProgressResponseDTO>> UpdateReadingProgressAsync(int id, ReadingProgressDTO readingProgressDTO)
        {
            if (readingProgressDTO == null)
                return ServiceResultType<ReadingProgressResponseDTO>.ErrorResult("Reading progress data cannot be null");

            var existingEntity = await _readingProgressRepository.GetByIdAsync(e => e.Id == id);
            if (existingEntity == null)
                return ServiceResultType<ReadingProgressResponseDTO>.ErrorResult("Reading progress not found");

            _mapper.Map(readingProgressDTO, existingEntity);

            await _readingProgressRepository.UpdateAsync(existingEntity);

            var responseDto = _mapper.Map<ReadingProgressResponseDTO>(existingEntity);
            Log.Information($"Оновлено прогрес читання з Id: {id} о {DateTime.UtcNow}.");

            return ServiceResultType<ReadingProgressResponseDTO>.SuccessResult(responseDto);
        }



        public async Task<ServiceResultType<ReadingProgressResponseDTO>> GetReadingProgressByIdAsync(int id)
        {
            var entity = await _readingProgressRepository.GetByIdAsync(e => e.Id == id);
            if (entity == null)
                return ServiceResultType<ReadingProgressResponseDTO>.ErrorResult("Reading progress not found");

            var responseDto = _mapper.Map<ReadingProgressResponseDTO>(entity);
            Log.Information($"Отримано прогрес читання з Id: {id} о {DateTime.UtcNow}.");

            return ServiceResultType<ReadingProgressResponseDTO>.SuccessResult(responseDto);
        }


        public async Task<ServiceResultType<bool>> DeleteReadingProgressAsync(int id)
        {
            var entity = await _readingProgressRepository.GetByIdAsync(e => e.Id == id);
            if (entity == null)
                return ServiceResultType<bool>.ErrorResult("Reading progress not found");

            await _readingProgressRepository.DeleteAsync(entity);
            Log.Information($"Ініціалізовано видалення прогресу читання з Id: {id} о {DateTime.UtcNow}.");
            return ServiceResultType<bool>.SuccessResult(true);
        }

    }
}
