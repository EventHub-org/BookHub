using AutoMapper;
using BookHub.DAL.DTO;
using BookHub.BLL.Services.Interfaces;
using BookHub.DAL.Entities;
using BookHub.DAL.Repositories.Interfaces;
using BookHub.BLL.Utils;
using Serilog;
using System.ComponentModel.DataAnnotations;
using BookHub.DAL.Repositories.Implementations;

namespace BookHub.BLL.Services.Implementations
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IRepository<ReviewEntity> _repository;
        private readonly IMapper _mapper;

        public ReviewService(IRepository<ReviewEntity> repository, IReviewRepository reviewRepository, IMapper mapper, IBookRepository bookRepository)
        {
            _repository = repository;
            _reviewRepository = reviewRepository;
            _mapper = mapper;
            _bookRepository = bookRepository;
        }

        public async Task<ServiceResultType<PageDto<ReviewDto>>> GetPaginatedReviewsAsync(Pageable pageable)
        {
            var validationResult = PageUtils.ValidatePage<BookDto>(pageable);

            if (!validationResult.Success)
            {
                return ServiceResultType<PageDto<ReviewDto>>.ErrorResult(validationResult.ErrorMessage);
            }

            var (reviewEntities, totalElements) = await _reviewRepository.GetPagedAsync(pageable);

            var reviewDtos = _mapper.Map<List<ReviewDto>>(reviewEntities);

            var totalPages = (int)Math.Ceiling((double)totalElements / pageable.Size);

            Log.Information($"Ініціалізовано отримання всіх рецензій з пагінацією о {DateTime.UtcNow}.");

            return ServiceResultType<PageDto<ReviewDto>>.SuccessResult(new PageDto<ReviewDto>
            {
                Items = reviewDtos,
                TotalElements = totalElements,
                CurrentPage = pageable.Page,
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

            var existingReviews = await _reviewRepository.GetAllAsync(r => r.BookId == reviewDto.BookId);
            double averageRating = existingReviews.Any() ? existingReviews.Average(r => r.Rating) : 0;
            var bookEntity = await _bookRepository.GetByIdAsync(b => b.Id == reviewDto.BookId);
            if (bookEntity != null)
            {
                bookEntity.Rating = averageRating;
                await _bookRepository.UpdateAsync(bookEntity);
            }


            Log.Information($"Ініціалізовано створення рецензії з Id: {reviewEntity.Id} о {DateTime.UtcNow}.");

            return ServiceResultType<ReviewDto>.SuccessResult(_mapper.Map<ReviewDto>(reviewEntity));
        }


        public async Task<ServiceResultType<PageDto<ReviewDto>>> GetPaginatedReviewsByBookIdAsync(int bookId, Pageable pageable)
        {
            var allReviews = await _reviewRepository.GetAllAsync();

            var filteredReviews = allReviews.Where(r => r.BookId == bookId);

            var totalItems = filteredReviews.Count();

            var paginatedReviews = filteredReviews
                .Skip((pageable.Page - 1) * pageable.Size)
                .Take(pageable.Size)
                .ToList();

            var reviewDtos = _mapper.Map<List<ReviewDto>>(paginatedReviews);

            var pageDto = new PageDto<ReviewDto>
            {
                Items = reviewDtos,
                TotalElements = totalItems,
                TotalPages = (int)Math.Ceiling((double)totalItems / pageable.Size)
            };

            return ServiceResultType<PageDto<ReviewDto>>.SuccessResult(pageDto);
        }


    }
}
