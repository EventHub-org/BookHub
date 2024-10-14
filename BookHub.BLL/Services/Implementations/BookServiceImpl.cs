using AutoMapper;
using BookHub.DAL.DTO;
using BookHub.DAL.Entities;
using BookHub.DAL.Repositories.Interfaces;


namespace BookHub.BLL.Services.Implementations
{

    public class BookServiceImpl
    {
        private readonly IBookRepository<BookEntity> _bookRepository;
        private readonly IMapper _mapper;

        public BookServiceImpl(IBookRepository<BookEntity> bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        public PageDto<BookDto> GetPageDto(int size, int page) 
        {
            if (size <= 0)
            {
                throw new ArgumentException("Page size must be greater than zero.", nameof(size));
            }

            if (page <= 0)
            {
                throw new ArgumentException("Page number must be greater than zero.", nameof(page));
            }

            var (bookEntities, totalElements) = _bookRepository.GetPagedAsync(size, page).GetAwaiter().GetResult();

            var bookDtos = _mapper.Map<List<BookDto>>(bookEntities);

            var totalPages = (int)Math.Ceiling((double)totalElements / size);

            return new PageDto<BookDto>
            {
                Items = bookDtos,
                TotalElements = totalElements,
                CurrentPage = page,
                TotalPages = totalPages
            };

        }
    }
}
