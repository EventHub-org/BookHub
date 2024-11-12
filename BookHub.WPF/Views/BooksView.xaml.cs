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
            int userId = 1; // Replace with actual logic to retrieve user ID
            var user = await ((BooksViewModel)DataContext).GetUserByIdAsync(userId);

            if (user != null)
            {
                var userProfileViewModel = new UserProfileViewModel(user);
                var userProfileView = new UserProfileView
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

        public void NavigateToPage(Page page)
        {
            var frame = new Frame();
            Content = frame;
            frame.Navigate(page);
        }
    }
}
