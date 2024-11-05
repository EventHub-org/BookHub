using BookHub.BLL.Services.Interfaces;
using BookHub.DAL.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Serilog;
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
            Log.Information("Початок завантаження книг..."); // Логування початку завантаження
            try
            {
                var result = await _bookService.GetPaginatedBooksAsync(new Pageable { Page = 1, Size = 2 });
                if (result.Success)
                {
                    Books = new ObservableCollection<BookDto>(result.Data.Items);
                    Log.Information($"Завантажено {result.Data.Items.Count} книг."); // Логування кількості завантажених книг
                }
                else
                {
                    Log.Error($"Не вдалося завантажити книги: {result.ErrorMessage}"); // Логування помилки
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Виникла помилка під час завантаження книг."); // Логування виключення
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
