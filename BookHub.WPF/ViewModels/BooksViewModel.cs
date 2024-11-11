using BookHub.BLL.Services.Interfaces;
using BookHub.DAL.DTO;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using BookHub.WPF.Views;
using AutoMapper;  // Import AutoMapper

namespace BookHub.WPF.ViewModels
{
    public class BooksViewModel : INotifyPropertyChanged
    {
        private readonly IBookService _bookService;
        private readonly IUserService _userService; // Inject IUserService
        private readonly IMapper _mapper;  // Inject IMapper
        private ObservableCollection<BookDto> _books;
        private BookDto _selectedBook;
        private int _currentPage;
        private const int _pageSize = 3; // Number of books per page
        private int _totalPages;

        public BooksViewModel(IBookService bookService, IUserService userService, IMapper mapper)
        {
            _bookService = bookService;
            _userService = userService; // Initialize IUserService
            _mapper = mapper; // Initialize IMapper
            CurrentPage = 1;
            LoadBooksAsync().ConfigureAwait(false);
            PreviousPageCommand = new RelayCommand(PreviousPage, CanGoToPreviousPage);
            NextPageCommand = new RelayCommand(NextPage, CanGoToNextPage);
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


        // Open Journal Command
        public ICommand OpenJournalCommand => new RelayCommand(OpenJournal);

        private void OpenJournal()
        {
            // Now using _mapper for mapping
            var journalViewModel = new JournalViewModel(_bookService, _mapper);
            var journalView = new JournalView(journalViewModel);
            journalView.Show();
        }

    }
}
