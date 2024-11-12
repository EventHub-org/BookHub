using BookHub.BLL.Services.Interfaces;
using BookHub.WPF.ViewModels;
using BookHub.DAL.DTO;
using System.Windows;
using BookHub.BLL.Services.Implementations;

namespace BookHub.WPF.Views
{
    /// <summary>
    /// Interaction logic for UserProfileView.xaml
    /// </summary>
    public partial class UserProfileView : Window
    {
        private readonly ICollectionService _collectionService;
        // Constructor that initializes UserProfileView
        public UserProfileView(IUserService userService, UserDto selectedUserDto)
        {
            InitializeComponent();

            // Initialize the ViewModel and set it as DataContext
            var userProfileViewModel = new UserProfileViewModel(userService, selectedUserDto);
            DataContext = userProfileViewModel;
        }

        private void CollectionsButton_Click(object sender, RoutedEventArgs e)
        {
            var collectionsViewModel = new CollectionsViewModel(_collectionService);
            var collectionsView = new CollectionsView(collectionsViewModel);
            collectionsView.Show();
            this.Close();
        }

        private void BooksButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to the Books view
        }
    }
}
