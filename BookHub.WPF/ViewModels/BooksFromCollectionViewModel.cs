using AutoMapper;
using BookHub.BLL.Services.Interfaces;
using BookHub.DAL.DTO;
using BookHub.WPF.State.Accounts;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookHub.WPF.ViewModels
{
    public class BooksFromCollectionViewModel : INotifyPropertyChanged
    {
        private readonly IBookService _bookService;
        private readonly int _collectionId;

        public ICommand OpenBookDetailsCommand { get; }
        private ObservableCollection<BookDto> _books;
        private int _currentPage;
        private const int _pageSize = 3;
        private int _totalPages;

        public BooksFromCollectionViewModel(IBookService bookService, int collectionId)
        {
            _bookService = bookService;
            _collectionId = collectionId;

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
            var pageable = new Pageable { Page = CurrentPage, Size = _pageSize };
            var result = await _bookService.GetPaginatedBooksFromCollectionAsync(_collectionId, pageable);

            if (result.Success)
            {
                Books = new ObservableCollection<BookDto>(result.Data.Items);
                TotalPages = result.Data.TotalPages;
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
