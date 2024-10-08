using BookHub.DAL.Entities;
using BookHub.DAL.Repositories.Interfaces;
using BookHub.DAL.DataAccess;

namespace BookHub.DAL.Repositories.Implementations
{
    public class BookRepository : IBookRepository<BookEntity>
    {
        private readonly AppDbContext _context;

        public BookRepository(AppDbContext context)
        {
            _context = context;
        }

        public void AddBook(BookEntity entity)
        {
            _context.Books.Add(entity);
            _context.SaveChanges();
        }

        public void UpdateBook(BookEntity entity)
        {
            _context.Books.Update(entity);
            _context.SaveChanges();
        }

        public void DeleteBook(BookEntity entity)
        {
            _context.Books.Remove(entity);
            _context.SaveChanges();
        }

        public BookEntity GetBookById(int id)
        {
            return _context.Books.Find(id);
        }

        public IEnumerable<BookEntity> GetAllBooks()
        {
            return _context.Books.ToList();
        }
    }
}
