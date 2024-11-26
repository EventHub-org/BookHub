using BookHub.WPF.State.Accounts;
using BookHub.WPF.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Serilog;

namespace BookHub.WPF.Views
{
    public partial class BookDetailsView : Window
    {
        private readonly IAccountStore _accountStore;
        private readonly BookDetailsViewModel _viewModel;
        private readonly int _bookId;

        public BookDetailsView(IAccountStore accountStore, BookDetailsViewModel viewModel, int bookId)
        {
            InitializeComponent();
            DataContext = viewModel;
            _accountStore = accountStore;
            _viewModel = viewModel;
            _bookId = bookId;
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            var app = (App)Application.Current;
            var booksView = app.BooksView;

            booksView.Show();
            this.Close();
        }

        private async void SaveNewReview_Click(object sender, RoutedEventArgs e)
        {
            var reviewRatingText = ReviewRatingTextBox.Text.Trim();
            var reviewComment = ReviewCommentTextBox.Text.Trim();

            if (double.TryParse(reviewRatingText, out double reviewRating))
            {
                if (!string.IsNullOrWhiteSpace(reviewComment))
                {
                    int? userId = _accountStore.CurrentUserId;

                    await _viewModel.CreateReviewAsync(reviewRating, reviewComment, userId.Value, _bookId);

                    await _viewModel.RefreshReviewsAsync();

                    CreateReviewPanel.Visibility = Visibility.Collapsed;
                    ReviewRatingTextBox.Clear();
                    ReviewCommentTextBox.Clear();
                }
                else
                {
                    MessageBox.Show("Please enter a review comment.");
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid numeric rating.");
            }
        }


        private void CreateReviewPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CreateReviewPanel.Visibility = Visibility.Collapsed;

        }
        private void CreateNewReview_Click(object sender, RoutedEventArgs e)
        {
            if (_accountStore.IsUserAuthenticated())
            {
                int? userId = _accountStore.CurrentUserId;

                if (userId.HasValue)
                {

                    CreateReviewPanel.Visibility = Visibility.Visible;
                }
                else
                {
                    Log.Warning("UserId is null or invalid for the current account.");
                    MessageBox.Show("User is not authenticated.");
                }
            }
            else
            {
                Log.Warning("No user is currently logged in.");
                MessageBox.Show("No user is logged in.");
            }
        }

    }
}
