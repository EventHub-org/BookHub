using AutoMapper;
using BookHub.BLL.Services.Interfaces;
using BookHub.BLL.Utils;
using BookHub.DAL.DTO;
using BookHub.DAL.Entities;
using BookHub.DAL.Repositories.Interfaces;
using Microsoft.VisualBasic.ApplicationServices;
using Serilog;
using System.Net;

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

            if (!bookEntity.Success)
            {
                return ServiceResultType<BookDto>.ErrorResult(bookEntity.ErrorMessage);
            }

            var bookDto = _mapper.Map<BookDto>(bookEntity.Data);

            Log.Information($"Ініціалізовано додавання книги з Id: {id} о {DateTime.UtcNow}.");

            return ServiceResultType<BookDto>.SuccessResult(bookDto);
        }

        public async Task<ServiceResultType<PageDto<BookDto>>> GetPaginatedBooksAsync(Pageable pageable) 
        {
            var validationResult = PageUtils.ValidatePage<BookDto>(pageable);

            if (!validationResult.Success)
            {
                return ServiceResultType<PageDto<BookDto>>.ErrorResult(validationResult.ErrorMessage);
            }

            var (bookEntities, totalElements) = await _bookRepository.GetPagedAsync(pageable);

            var bookDtos = _mapper.Map<List<BookDto>>(bookEntities);

            var totalPages = (int)Math.Ceiling((double)totalElements / pageable.Size);

            Log.Information($"Ініціалізовано отримання всіх книг з пагінацією о {DateTime.UtcNow}.");

            return ServiceResultType<PageDto<BookDto>>.SuccessResult(new PageDto<BookDto>
            {
                Items = bookDtos,
                TotalElements = totalElements,
                CurrentPage = pageable.Page,
                TotalPages = totalPages
            });
        }
        public async Task<ServiceResultType> DeleteBookAsync(int id)
        {
            var bookEntityResult = await GetBookEntityAsync(id);

            if (!bookEntityResult.Success)
            {
                return ServiceResultType.ErrorResult(bookEntityResult.ErrorMessage);
            }

            await _repository.DeleteAsync(bookEntityResult.Data);

            Log.Information($"Ініціалізовано видалення книги з Id: {id} о {DateTime.UtcNow}.");

            return ServiceResultType.SuccessResult();
        }

        private async Task<ServiceResultType<BookEntity>> GetBookEntityAsync(int id)
        {
            var bookEntity = await _repository.GetByIdAsync(b => b.Id == id);

            if (bookEntity == null)
            {
                return ServiceResultType<BookEntity>.ErrorResult($"Book with ID {id} not found.");
            }
            Log.Information($"Ініціалізовано отримання книги за Id з Id: {id} о {DateTime.UtcNow}.");
            return ServiceResultType<BookEntity>.SuccessResult(bookEntity);
        }
    }
}
