using BookHub.BLL.Services.Interfaces;
using BookHub.DAL.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookHub.WPF.ViewModels
{
    public class BooksViewModel : INotifyPropertyChanged
    {
        private readonly IBookService _bookService;
        private ObservableCollection<BookDto> _books;
        private BookDto _selectedBook;
        private int _currentPage;
        private const int _pageSize = 3; // Кількість книг на сторінці
        private int _totalPages;

        public BooksViewModel(IBookService bookService)
        {
            _bookService = bookService;
            CurrentPage = 1;
            LoadBooksAsync().ConfigureAwait(false);
            PreviousPageCommand = new RelayCommand(PreviousPage, CanGoToPreviousPage);
            NextPageCommand = new RelayCommand(NextPage, CanGoToNextPage);
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

        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                OnPropertyChanged(nameof(CurrentPage));
                LoadBooksAsync().ConfigureAwait(false);
            }
        }

        public int TotalPages
        {
            get => _totalPages;
            private set
            {
                _totalPages = value;
                OnPropertyChanged(nameof(TotalPages));
            }
        }

        public ICommand PreviousPageCommand { get; }
        public ICommand NextPageCommand { get; }

        private async Task LoadBooksAsync()
        {
            var result = await _bookService.GetPaginatedBooksAsync(new Pageable { Page = CurrentPage, Size = _pageSize });
            if (result.Success)
            {
                Books = new ObservableCollection<BookDto>(result.Data.Items);
                TotalPages = result.Data.TotalPages; // Припустимо, ваш сервіс повертає загальну кількість сторінок
            }
        }

        private void PreviousPage()
        {
            if (CanGoToPreviousPage())
            {
                CurrentPage--;
            }
        }

        private void NextPage()
        {
            if (CanGoToNextPage())
            {
                CurrentPage++;
            }
        }

        private bool CanGoToPreviousPage() => CurrentPage > 1;

        private bool CanGoToNextPage() => CurrentPage < TotalPages;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
