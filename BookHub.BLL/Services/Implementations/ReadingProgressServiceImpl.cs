using AutoMapper;
using BookHub.BLL.Services.Interfaces;
using BookHub.DAL.Entities;
using BookHub.DAL.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Serilog;

namespace BookHub.BLL.Services.Implementations
{
    public class ReadingProgressServiceImpl : IReadingProgressService
    {
        private readonly IReadingProgressRepository _readingProgressRepository;
        private readonly IMapper _readingProgressMapper;
        private readonly ILogger<ReadingProgressServiceImpl> _logger;

        public ReadingProgressServiceImpl(
            IReadingProgressRepository readingProgressRepository,
            IMapper readingProgressMapper
            )
        {
            _readingProgressRepository = readingProgressRepository;
            _readingProgressMapper = readingProgressMapper;
        }


        public async Task<ReadingProgressEntity> createReadingProgress(ReadingProgressDTO readingProgressDTO)
        {
            if (readingProgressDTO == null)
            {
                throw new NullReferenceException("ReadingProgressDTO cannot be null");
            }

            var validationContext = new ValidationContext(readingProgressDTO, null, null);
            var validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(readingProgressDTO, validationContext, validationResults, true);

            if (!isValid)
            {
                var validationErrors = string.Join("; ", validationResults.Select(r => r.ErrorMessage));
                throw new ArgumentException($"Validation failed: {validationErrors}");
            }

            var readingProgressEntity = _readingProgressMapper.Map<ReadingProgressEntity>(readingProgressDTO);

            await _readingProgressRepository.AddAsync(readingProgressEntity);

            Log.Information("Створення прогресу читання");

            return readingProgressEntity;
        }
    }
}
