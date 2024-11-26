using BookHub.BLL.Services.Interfaces;
using BookHub.WPF.ViewModels;
using BookHub.DAL.DTO;
using System.Windows;
using BookHub.BLL.Services.Implementations;
using BookHub.WPF.State.Accounts;
using Microsoft.VisualBasic.ApplicationServices;

namespace BookHub.WPF.Views
{
    /// <summary>
    /// Interaction logic for UserProfileView.xaml
    /// </summary>
    public partial class UserProfileView : Window
    {
        private readonly ICollectionService _collectionService;
        private readonly IBookService _bookService;
        private readonly IAccountStore _accountStore;
        public UserProfileView(IUserService userService, IBookService bookService, IAccountStore accountStore, UserDto selectedUserDto)
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            // Initialize the ViewModel and set it as DataContext
            var userProfileViewModel = new UserProfileViewModel(userService, selectedUserDto);
            DataContext = userProfileViewModel;
            _bookService = bookService;
            _accountStore = accountStore;
        }

        private void CollectionsButton_Click(object sender, RoutedEventArgs e)
        {
            if (_accountStore.IsUserAuthenticated())
            {
                int? userId = _accountStore.CurrentUserId;

                if (userId.HasValue)
                {
                    var collectionsViewModel = new CollectionsViewModel(_collectionService, userId.Value);
                    var collectionsView = new CollectionsView(collectionsViewModel, _bookService, _collectionService, _accountStore);
                    collectionsView.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("User is not authenticated.");
                }
            }
            else
            {
                MessageBox.Show("No user is logged in.");
            }
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            // Отримуємо збережений інстанс BooksView з App
            var app = (App)Application.Current; // Отримуємо екземпляр App
            var booksView = app.BooksView;

            booksView.Show(); // Показуємо BooksView
            this.Close(); // Закриваємо поточне вікно UserProfileView
        }

    }
}
