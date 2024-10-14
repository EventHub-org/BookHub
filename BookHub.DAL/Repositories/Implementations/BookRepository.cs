using BookHub.DAL.Entities;
using BookHub.DAL.Repositories.Interfaces;
using BookHub.DAL.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace BookHub.DAL.Repositories.Implementations
{
    public class BookRepository : IRepository<BookEntity>, IBookRepository<BookEntity>
    {
        private readonly AppDbContext _context;

        public BookRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(BookEntity entity)
        {
            _context.Books.Add(entity);
            _context.SaveChanges();
        }

        public async Task UpdateAsync(BookEntity entity)
        {
            _context.Books.Update(entity);
            _context.SaveChanges();
        }

        public async Task DeleteAsync(BookEntity entity)
        {
            _context.Books.Remove(entity);
            _context.SaveChanges();
        }

        public async Task<BookEntity> GetByIdAsync(int id)
        {
            return _context.Books.Find(id);
        }

        public async Task<IEnumerable<BookEntity>> GetAllAsync()
        {
            return _context.Books.ToList();
        }

        public async Task<(List<BookEntity> Items, long TotalCount)> GetPagedAsync(int pageSize, int pageNumber)
        {
            var totalCount = await _context.Books.CountAsync();

            var items = await _context.Books
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }
    }
}
