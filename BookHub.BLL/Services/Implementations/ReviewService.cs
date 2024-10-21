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

        public async Task<PageDto<ReviewDto>> GetPaginatedReviewsAsync(int size, int page)
        {
            if (size <= 0)
            {
                throw new ArgumentException("Page size must be greater than zero.", nameof(size));
            }

            if (page <= 0)
            {
                throw new ArgumentException("Page number must be greater than zero.", nameof(page));
            }

            var (reviewEntities, totalElements) = await _reviewRepository.GetPagedAsync(size, page);

            var reviewDtos = _mapper.Map<List<ReviewDto>>(reviewEntities);

            var totalPages = (int)Math.Ceiling((double)totalElements / size);

            return new PageDto<ReviewDto>
            {
                Items = reviewDtos,
                TotalElements = totalElements,
                CurrentPage = page,
                TotalPages = totalPages
            };
        }

        public async Task<ReviewDto> GetReviewAsync(int id)
        {
            ReviewEntity reviewEntity = await GetReviewEntityAsync(id);

            var reviewDto = _mapper.Map<ReviewDto>(reviewEntity);

            return reviewDto;
        }

        public async Task DeleteReviewAsync(int id)
        {
            var reviewEntity = await GetReviewEntityAsync(id);

            await _repository.DeleteAsync(reviewEntity);
        }

        private async Task<ReviewEntity> GetReviewEntityAsync(int id)
        {
            var reviewEntity = await _repository.GetByIdAsync(b => b.Id == id);

            if (reviewEntity == null)
            {
                throw new KeyNotFoundException($"Book with ID {id} not found.");
            }

            return reviewEntity;
        }
    }
}
