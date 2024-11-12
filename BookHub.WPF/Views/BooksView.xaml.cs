using BookHub.BLL.Services.Interfaces;
using BookHub.WPF.ViewModels;
using BookHub.WPF.Views;
using System;
using System.Windows;
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

        // Constructor with dependency injection
        public BooksView(BooksViewModel viewModel, ICollectionService collectionService, IUserService userService)
        {
            InitializeComponent();
            DataContext = viewModel;
            _collectionService = collectionService;
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
    }
}
