using AutoMapper;
using BookHub.DAL.DTO;
using BookHub.DAL.Entities;
using BookHub.DAL.Repositories.Interfaces;

namespace BookHub.BLL.Services.Implementations
{
    public class ReviewServiceImpl
    {
        private readonly IReviewRepository<ReviewEntity> _reviewRepository;
        private readonly IMapper _mapper;

        public ReviewServiceImpl(IReviewRepository<ReviewEntity> reviewRepository, IMapper mapper) 
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
        }

        public PageDto<ReviewDto> GetPaginatedReviews(int size, int page)
        {
            if (size <= 0)
            {
                throw new ArgumentException("Page size must be greater than zero.", nameof(size));
            }

            if (page <= 0)
            {
                throw new ArgumentException("Page number must be greater than zero.", nameof(page));
            }

            var (reviewEntities, totalElements) = _reviewRepository.GetPagedAsync(size, page).GetAwaiter().GetResult();

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
