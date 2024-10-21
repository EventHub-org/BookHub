using AutoMapper;
using BookHub.BLL.Services.Interfaces;
using BookHub.DAL.DTO;
using BookHub.DAL.Entities;
using BookHub.DAL.Repositories.Implementations;


namespace BookHub.BLL.Services.Implementations
{

    public class BookService : IBookService
    {
        private readonly BookRepository _bookRepository;
        private readonly IMapper _mapper;

        public BookService(BookRepository bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        public async Task<BookDto> GetBookAsync(int id)
        {
            BookEntity bookEntity = await GetBookEntityAsync(id);

            var bookDto = _mapper.Map<BookDto>(bookEntity);

            return bookDto;
        }

        public async Task<PageDto<BookDto>> GetPaginatedBooksAsync(int size, int page) 
        {
            if (size <= 0)
            {
                throw new ArgumentException("Page size must be greater than zero.", nameof(size));
            }

            if (page <= 0)
            {
                throw new ArgumentException("Page number must be greater than zero.", nameof(page));
            }

            var (bookEntities, totalElements) = await _bookRepository.GetPagedAsync(size, page);

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
        public async Task DeleteBookAsync(int id)
        {
            var bookEntity = await GetBookEntityAsync(id);

            await _bookRepository.DeleteAsync(bookEntity);
        }

        private async Task<BookEntity> GetBookEntityAsync(int id)
        {
            var bookEntity = await _bookRepository.GetByIdAsync(id);

            if (bookEntity == null)
            {
                throw new KeyNotFoundException($"Book with ID {id} not found.");
            }

            return bookEntity;
        }
    }
}
