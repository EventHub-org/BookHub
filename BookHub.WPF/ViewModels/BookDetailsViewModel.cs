using BookHub.DAL.DTO;
using BookHub.BLL.Services.Interfaces;
using System.Threading.Tasks;

namespace BookHub.WPF.ViewModels
{
    public class BookDetailsViewModel
    {
        private readonly IBookService _bookService;
        
        public BookDto Book { get; private set; }

        public BookDetailsViewModel(IBookService bookService)
        {
            _bookService = bookService;
        }

        public async Task LoadBookAsync(int bookId)
        {
            var result = await _bookService.GetBookAsync(bookId);
            if (result.Success)
            {
                Book = result.Data;
            }
        }
    }
}
