using BookHub.BLL.Services.Implementations;
using BookHub.BLL.Services.Interfaces;
using BookHub.DAL.DTO;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Serilog;

namespace BookHub.WPF.ViewModels
{
    public class BookDetailsViewModel
    {
        private readonly IBookService _bookService;
        private readonly IReviewService _reviewService;
        private int _currentPage;
        private int _totalPages;
        private const int PageSize = 5; // Кількість рев'ю на сторінці

        public BookDto Book { get; private set; }
        public ObservableCollection<ReviewDto> Reviews { get; private set; }

        public ICommand LoadNextReviewsCommand { get; }
        public ICommand LoadPreviousReviewsCommand { get; }

        public BookDetailsViewModel(IBookService bookService, IReviewService reviewService)
        {
            _bookService = bookService;
            _reviewService = reviewService;

            _currentPage = 1;
            Reviews = new ObservableCollection<ReviewDto>();

            LoadNextReviewsCommand = new RelayCommand(LoadNextReviews, CanLoadNextReviews);
            LoadPreviousReviewsCommand = new RelayCommand(LoadPreviousReviews, CanLoadPreviousReviews);
        }

        public async Task LoadBookAsync(int bookId)
        {
            var result = await _bookService.GetBookAsync(bookId);
            if (result.Success)
            {
                Book = result.Data;
                await LoadReviewsAsync();
            }
        }

        private async Task LoadReviewsAsync()
        {
            var result = await _reviewService.GetPaginatedReviewsByBookIdAsync(Book.Id, new Pageable { Page = _currentPage, Size = PageSize });
            if (result.Success)
            {
                Reviews.Clear();
                foreach (var review in result.Data.Items)
                {
                    Reviews.Add(review);
                }

                _totalPages = result.Data.TotalPages;
            }
        }

        public async Task CreateReviewAsync(double reviewRating, string reviewComment, int userId, int bookId)
        {
            Log.Information("Starting to create a new review...");

            try
            {
                var newReview = new ReviewDto { Rating = reviewRating, Comment = reviewComment, UserId = userId, BookId = bookId };
                var result = await _reviewService.CreateReviewAsync(newReview);

                if (result.Success)
                {
                    Reviews.Add(newReview);
                    Log.Information($"New review created successfully.");
                }
                else
                {
                    Log.Error($"Failed to create collection: {result.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while creating the review.");
            }
        }

        public async Task RefreshReviewsAsync()
        {
            Log.Information("Refreshing reviews...");
            await LoadReviewsAsync();
        }

        private void LoadNextReviews()
        {
            _currentPage++;
            LoadReviewsAsync().ConfigureAwait(false);
        }

        private void LoadPreviousReviews()
        {
            _currentPage--;
            LoadReviewsAsync().ConfigureAwait(false);
        }

        private bool CanLoadNextReviews() => _currentPage < _totalPages;
        private bool CanLoadPreviousReviews() => _currentPage > 1;
    }
}
