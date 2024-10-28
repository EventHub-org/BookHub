using AutoMapper;
using BookHub.DAL.DTO;
using BookHub.BLL.Services.Interfaces;
using BookHub.DAL.Entities;
using BookHub.DAL.Repositories.Interfaces;
using BookHub.BLL.Utils;
using Serilog;
using System.ComponentModel.DataAnnotations;

namespace BookHub.BLL.Services.Implementations
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IRepository<ReviewEntity> _repository;
        private readonly IMapper _mapper;

        public ReviewService(IRepository<ReviewEntity> repository, IReviewRepository reviewRepository, IMapper mapper)
        {
            _repository = repository;
            _reviewRepository = reviewRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResultType<PageDto<ReviewDto>>> GetPaginatedReviewsAsync(int size, int page)
        {
            var validationResult = PageUtils.ValidatePage<BookDto>(size, page);

            if (!validationResult.Success)
            {
                return ServiceResultType<PageDto<ReviewDto>>.ErrorResult(validationResult.ErrorMessage);
            }

            var (reviewEntities, totalElements) = await _reviewRepository.GetPagedAsync(size, page);

            var reviewDtos = _mapper.Map<List<ReviewDto>>(reviewEntities);

            var totalPages = (int)Math.Ceiling((double)totalElements / size);

            Log.Information($"Ініціалізовано отримання всіх книг з пагінацією о {DateTime.UtcNow}.");

            return ServiceResultType<PageDto<ReviewDto>>.SuccessResult(new PageDto<ReviewDto>
            {
                Items = reviewDtos,
                TotalElements = totalElements,
                CurrentPage = page,
                TotalPages = totalPages
            });
        }

        public async Task<ServiceResultType<ReviewDto>> GetReviewAsync(int id)
        {
            var reviewEntityResult = await GetReviewEntityAsync(id);

            if (!reviewEntityResult.Success)
            {
                return ServiceResultType<ReviewDto>.ErrorResult(reviewEntityResult.ErrorMessage);
            }

            var reviewDto = _mapper.Map<ReviewDto>(reviewEntityResult.Data);

            Log.Information($"Ініціалізовано отримання рецензії за Id з Id: {id} о {DateTime.UtcNow}.");

            return ServiceResultType<ReviewDto>.SuccessResult(reviewDto);
        }

        public async Task<ServiceResultType> DeleteReviewAsync(int id)
        {
            var reviewEntityResult = await GetReviewEntityAsync(id);

            if (!reviewEntityResult.Success)
            {
                return ServiceResultType.ErrorResult(reviewEntityResult.ErrorMessage);
            }

            await _repository.DeleteAsync(reviewEntityResult.Data);

            Log.Information($"Ініціалізовано видалення рецензії з Id: {id} о {DateTime.UtcNow}.");

            return ServiceResultType.SuccessResult();
        }

        private async Task<ServiceResultType<ReviewEntity>> GetReviewEntityAsync(int id)
        {
            var reviewEntity = await _repository.GetByIdAsync(b => b.Id == id);

            if (reviewEntity == null)
            {
                return ServiceResultType<ReviewEntity>.ErrorResult($"Review with ID {id} not found.");
            }
            Log.Information($"Ініціалізовано отримання рецензії за Id з Id: {id} о {DateTime.UtcNow}.");
            return ServiceResultType<ReviewEntity>.SuccessResult(reviewEntity);
        }

        public async Task<ServiceResultType<ReviewDto>> CreateReviewAsync(ReviewDto reviewDto)
        {
            if (reviewDto == null)
            {
                return ServiceResultType<ReviewDto>.ErrorResult("Review data cannot be null");
            }

            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(reviewDto);
            bool isValid = Validator.TryValidateObject(reviewDto, validationContext, validationResults, true);

            if (!isValid)
            {
                return ServiceResultType<ReviewDto>.ErrorResult("Validation failed: " + string.Join(", ", validationResults.Select(v => v.ErrorMessage)));
            }

            var reviewEntity = _mapper.Map<ReviewEntity>(reviewDto);
            await _reviewRepository.AddAsync(reviewEntity);

            Log.Information($"Ініціалізовано створення рецензії з Id: {reviewEntity.Id} о {DateTime.UtcNow}.");

            return ServiceResultType<ReviewDto>.SuccessResult(_mapper.Map<ReviewDto>(reviewEntity));
        }
    }
}
