using AutoMapper;
using BookHub.DAL.DTO;
using BookHub.BLL.Services.Interfaces;
using BookHub.DAL.Entities;
using BookHub.DAL.Repositories.Interfaces;

namespace BookHub.BLL.Services.Implementations
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository<ReviewEntity> _reviewRepository;
        private readonly IMapper _mapper;

        public ReviewService(IReviewRepository<ReviewEntity> reviewRepository, IMapper mapper)
        {
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
    }
}
