using BookHub.BLL.Services.Interfaces;
using BookHub.WPF.ViewModels;
using BookHub.WPF.Views;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using AutoMapper;
using System.Threading.Tasks;

namespace BookHub.WPF.Views
{
    /// <summary>
    /// Interaction logic for BooksView.xaml
    /// </summary>
    public partial class BooksView : Window
    {
        private readonly ICollectionService _collectionService;
        private readonly IUserService _userService;
        
        private readonly IReadingProgressService _readingProgressService;
        private readonly IBookService _bookService;


        public BooksView(BooksViewModel viewModel, ICollectionService collectionService, IUserService userService, IReadingProgressService readingProgressService, IBookService bookService)
        {
            InitializeComponent();
            DataContext = viewModel;
            _collectionService = collectionService;
            _readingProgressService = readingProgressService;
            _bookService = bookService;
            _userService = userService;
        }


        // Button to navigate to Collections view
        private void CollectionsButton_Click(object sender, RoutedEventArgs e)
        {
            var collectionsViewModel = new CollectionsViewModel(_collectionService);
            var collectionsView = new CollectionsView(collectionsViewModel);
            collectionsView.Show();
            this.Close();
        }

        // Button to navigate to User Profile view
        private async void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            int userId = 1; // Ideally, fetch this from session/context or login

            // Fetch user data asynchronously using the IUserService
            var user = await _userService.GetUserByIdAsync(userId);

            if (user != null)
            {
                // Create UserProfileViewModel only if user exists
                var userProfileViewModel = new UserProfileViewModel(_userService, user.Data);
                var userProfileView = new UserProfileView(_userService, user.Data)
                {
                    DataContext = userProfileViewModel
                };
                userProfileView.ShowDialog();
            }
            else
            {
                MessageBox.Show("User not found.");
            }
        }

        private async void Journal_Click(object sender, RoutedEventArgs e)
        {
            int userId = 1; // Replace with actual logic to get the user ID
            var user = await _userService.GetUserByIdAsync(userId);

            if (user != null)
            {
                // Initialize the JournalViewModel directly with dependencies
                var journalViewModel = new JournalViewModel(
                    user.Data,
                    _readingProgressService,
                    _bookService
                );

                // Create and show the JournalView window with the view model
                var journalView = new JournalView(journalViewModel);
                journalView.Show();
            }
            else
            {
                MessageBox.Show("User not found.");
            }
        }


        public void NavigateToPage(Page page)
        {
            var frame = new Frame();
            Content = frame;
            frame.Navigate(page);
        }

    }
}
