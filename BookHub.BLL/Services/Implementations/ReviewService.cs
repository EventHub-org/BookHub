using AutoMapper;
using BookHub.DAL.DTO;
using BookHub.BLL.Services.Interfaces;
using BookHub.DAL.Entities;
using BookHub.DAL.Repositories.Interfaces;
using BookHub.BLL.Utils;

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
            PageUtils.ValidatePage<ReviewDto>(size, page);

            var (reviewEntities, totalElements) = await _reviewRepository.GetPagedAsync(size, page);

            var reviewDtos = _mapper.Map<List<ReviewDto>>(reviewEntities);

            var totalPages = (int)Math.Ceiling((double)totalElements / size);

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
            var reviewEntity = await GetReviewEntityAsync(id);

            var reviewDto = _mapper.Map<ReviewDto>(reviewEntity);

            return ServiceResultType<ReviewDto>.SuccessResult(reviewDto);
        }

        public async Task<ServiceResultType> DeleteReviewAsync(int id)
        {
            var reviewEntity = await GetReviewEntityAsync(id);

            await _repository.DeleteAsync(reviewEntity.Data);

            return ServiceResultType.SuccessResult();
        }

        private async Task<ServiceResultType<ReviewEntity>> GetReviewEntityAsync(int id)
        {
            var reviewEntity = await _repository.GetByIdAsync(b => b.Id == id);

            if (reviewEntity == null)
            {
                ServiceResultType<PageDto<ReviewDto>>.ErrorResult($"Review with ID {id} not found.");
            }

            return ServiceResultType<ReviewEntity>.SuccessResult(reviewEntity);
        }
    }
}
