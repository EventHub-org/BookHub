using AutoMapper;
using BookHub.DAL.DTO;
using BookHub.BLL.Services.Interfaces;
using BookHub.DAL.Entities;
using BookHub.DAL.Repositories.Interfaces;

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

        public async Task<ServiceResult> GetPaginatedReviewsAsync(int size, int page)
        {
            if (size <= 0)
            {
                ServiceResult.ErrorResult("Page size must be greater than zero.");
            }

            if (page <= 0)
            {
                ServiceResult.ErrorResult("Page number must be greater than zero.");
            }

            var (reviewEntities, totalElements) = await _reviewRepository.GetPagedAsync(size, page);

            var reviewDtos = _mapper.Map<List<ReviewDto>>(reviewEntities);

            var totalPages = (int)Math.Ceiling((double)totalElements / size);

            return ServiceResult.SuccessResult(new PageDto<ReviewDto>
            {
                Items = reviewDtos,
                TotalElements = totalElements,
                CurrentPage = page,
                TotalPages = totalPages
            });
        }

        public async Task<ServiceResult> GetReviewAsync(int id)
        {
            var reviewEntity = await GetReviewEntityAsync(id);

            var reviewDto = _mapper.Map<ReviewDto>(reviewEntity);

            return ServiceResult.SuccessResult(reviewDto);
        }

        public async Task DeleteReviewAsync(int id)
        {
            var reviewEntity = await GetReviewEntityAsync(id);

            await _repository.DeleteAsync((ReviewEntity)reviewEntity.Data);
        }

        private async Task<ServiceResult> GetReviewEntityAsync(int id)
        {
            var reviewEntity = await _repository.GetByIdAsync(b => b.Id == id);

            if (reviewEntity == null)
            {
                ServiceResult.ErrorResult($"Book with ID {id} not found.");
            }

            return ServiceResult.SuccessResult(reviewEntity);
        }
    }
}
