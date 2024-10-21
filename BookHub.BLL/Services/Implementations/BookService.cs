using AutoMapper;
using BookHub.BLL.Services.Interfaces;
using BookHub.BLL.Utils;
using BookHub.DAL.DTO;
using BookHub.DAL.Entities;
using BookHub.DAL.Repositories.Interfaces;


namespace BookHub.BLL.Services.Implementations
{

    public class BookService : IBookService
    {
        private readonly IBookRepository<BookEntity> _bookRepository;
        private readonly IRepository<BookEntity> _repository;
        private readonly IMapper _mapper;

        public BookService(IRepository<BookEntity> repository, IBookRepository<BookEntity> bookRepository, IMapper mapper)
        {   
            _repository = repository;
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
            PageUtils.ValidatePage(size, page);

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

            await _repository.DeleteAsync(bookEntity);
        }

        private async Task<BookEntity> GetBookEntityAsync(int id)
        {
            var bookEntity = await _repository.GetByIdAsync(id);

            if (bookEntity == null)
            {
                throw new KeyNotFoundException($"Book with ID {id} not found.");
            }

            return bookEntity;
        }
    }
}
