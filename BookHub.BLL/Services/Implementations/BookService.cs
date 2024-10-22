using AutoMapper;
using BookHub.BLL.Services.Interfaces;
using BookHub.BLL.Utils;
using BookHub.DAL.DTO;
using BookHub.DAL.Entities;
using BookHub.DAL.Repositories.Interfaces;
using Sprache;


namespace BookHub.BLL.Services.Implementations
{

    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IRepository<BookEntity> _repository;
        private readonly IMapper _mapper;

        public BookService(IRepository<BookEntity> repository, IBookRepository bookRepository, IMapper mapper)
        {   
            _repository = repository;
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResultType<BookDto>> GetBookAsync(int id)
        {
            var bookEntity = await GetBookEntityAsync(id);

            var bookDto = _mapper.Map<BookDto>(bookEntity.Data);
            
            return ServiceResultType<BookDto>.SuccessResult(bookDto);
        }

        public async Task<ServiceResultType<PageDto<BookDto>>> GetPaginatedBooksAsync(int size, int page) 
        {
            PageUtils.ValidatePage<BookDto>(size, page);

            var (bookEntities, totalElements) = await _bookRepository.GetPagedAsync(size, page);

            var bookDtos = _mapper.Map<List<BookDto>>(bookEntities);

            var totalPages = (int)Math.Ceiling((double)totalElements / size);

            return ServiceResultType<PageDto<BookDto>>.SuccessResult(new PageDto<BookDto>
            {
                Items = bookDtos,
                TotalElements = totalElements,
                CurrentPage = page,
                TotalPages = totalPages
            });
        }
        public async Task<ServiceResultType> DeleteBookAsync(int id)
        {
            var bookEntity = await GetBookEntityAsync(id);

            await _repository.DeleteAsync(bookEntity.Data);

            return ServiceResultType.SuccessResult();
        }

        private async Task<ServiceResultType<BookEntity>> GetBookEntityAsync(int id)
        {
            var bookEntity = await _repository.GetByIdAsync(b => b.Id == id);

            if (bookEntity == null)
            {
                return ServiceResultType<BookEntity>.ErrorResult($"Book with ID {id} not found.");
            }

            return ServiceResultType<BookEntity>.SuccessResult(bookEntity);
        }
    }
}
