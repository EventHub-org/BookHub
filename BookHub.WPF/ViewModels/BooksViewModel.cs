using BookHub.BLL.Services.Interfaces;
using BookHub.DAL.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows;
using Serilog;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BookHub.WPF.Views;
using AutoMapper;


namespace BookHub.WPF.ViewModels
{
    public class BooksViewModel : INotifyPropertyChanged
    {

        private readonly IBookService _bookService;
        private readonly IUserService _userService;
        private readonly IReviewService _reviewService;
        private readonly IMapper _mapper;
        public ICommand OpenBookDetailsCommand { get; }
        private ObservableCollection<BookDto> _books;
        private BookDto _selectedBook;
        private int _currentPage;
        private const int _pageSize = 3;
        private int _totalPages;


        public BooksViewModel(IBookService bookService, IUserService userService, IReviewService reviewService, IMapper mapper)
        {
            _bookService = bookService;
            _userService = userService;
            _mapper = mapper;
            CurrentPage = 1;
            LoadBooksAsync().ConfigureAwait(false);
            PreviousPageCommand = new RelayCommand(PreviousPage, CanGoToPreviousPage);
            NextPageCommand = new RelayCommand(NextPage, CanGoToNextPage);
            OpenBookDetailsCommand = new RelayCommand<int>(async (bookId) => await OpenBookDetailsAsync(bookId));

            _reviewService = reviewService;
        }

        // Method to get user by ID
        public async Task<UserDto> GetUserByIdAsync(int userId)
        {
            var result = await _userService.GetUserByIdAsync(userId);
            return result.Success ? result.Data : null;
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
                TotalPages = result.Data.TotalPages; // Assuming your service returns total pages
            }
        }

        private async Task OpenBookDetailsAsync(int bookId)
        {
            var bookDetailsViewModel = new BookDetailsViewModel(_bookService, _reviewService); // Передаємо обидві залежності
            await bookDetailsViewModel.LoadBookAsync(bookId);

            var bookDetailsView = new BookDetailsView
            {
                DataContext = bookDetailsViewModel
            };

            NavigateToPage(bookDetailsView);
        }

        private void NavigateToPage(Page page)
        {
            var booksView = Application.Current.MainWindow as BooksView;
            booksView?.NavigateToPage(page);
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


        //// Open Journal Command
        //public ICommand OpenJournalCommand => new RelayCommand(OpenJournal);

        //private void OpenJournal()
        //{
        //    // Now using _mapper for mapping
        //    var journalViewModel = new JournalViewModel(_bookService, _mapper);
        //    var journalView = new JournalView(journalViewModel);
        //    journalView.Show();
        //}

    }
}
