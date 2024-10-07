using BookHub.DAL.Entities;

namespace BookHub.DAL.Repositories.Interfaces
{
    public interface IBookRepository<TEntity> where TEntity : class
    {
        void AddBook(TEntity entity);
        void UpdateBook(TEntity entity);
        void DeleteBook(TEntity entity);
        BookEntity GetBookById(int id);
        IEnumerable<BookEntity> GetAllBooks();
    }
}
