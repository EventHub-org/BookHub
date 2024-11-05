using BookHub.BLL.Services.Interfaces;
using BookHub.DAL.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookHub.WPF.ViewModels
{
    public class BooksViewModel : INotifyPropertyChanged
    {
        private readonly IBookService _bookService;
        private ObservableCollection<BookDto> _books;
        private BookDto _selectedBook;

        public BooksViewModel(IBookService bookService)
        {
            _bookService = bookService;
            LoadBooksAsync().ConfigureAwait(false);
        }

        public ObservableCollection<BookDto> Books
        {
            get => _books;
            set
            {
                _books = value;
                OnPropertyChanged(nameof(Books));
            }
        }

        public BookDto SelectedBook
        {
            get => _selectedBook;
            set
            {
                _selectedBook = value;
                OnPropertyChanged(nameof(SelectedBook));
            }
        }

        private async Task LoadBooksAsync()
        {
            var result = await _bookService.GetPaginatedBooksAsync(new Pageable { Page = 1, Size = 10 });
            if (result.Success)
            {
                Books = new ObservableCollection<BookDto>(result.Data.Items);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
