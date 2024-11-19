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
using Microsoft.Win32;
using BookHub.WPF.State.Accounts;
using BookHub.BLL.Services.Implementations;
using Serilog; 

namespace BookHub.WPF.Views
{
    /// <summary>
    /// Interaction logic for BooksView.xaml
    /// </summary>
    public partial class BooksView : Window
    {
        private readonly ICollectionService _collectionService;
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        private readonly ISessionService _sessionService;

        private readonly IReadingProgressService _readingProgressService;
        private readonly IBookService _bookService;
        private readonly IAccountStore _accountStore;

        public BooksView(BooksViewModel viewModel, ICollectionService collectionService, IUserService userService,
            IReadingProgressService readingProgressService, IBookService bookService, IAuthService authService, ISessionService sessionService, IAccountStore accountStore)
        {
            InitializeComponent();
            DataContext = viewModel;
            _collectionService = collectionService;
            _readingProgressService = readingProgressService;
            _bookService = bookService;
            _userService = userService;
            _authService = authService;
            _sessionService = sessionService;
            _accountStore = accountStore;
        }


        // Button to navigate to Collections view
        private void CollectionsButton_Click(object sender, RoutedEventArgs e)
        {
            var collectionsViewModel = new CollectionsViewModel(_collectionService);
            var collectionsView = new CollectionsView(collectionsViewModel);
            collectionsView.Show();
            this.Close();
        }


private async void ProfileButton_Click(object sender, RoutedEventArgs e)
    {
        // Log the profile button click
        Log.Information("Profile button clicked.");

        if (_accountStore.IsUserAuthenticated()) // Use AccountStore to get current authenticated account
        {
            int? userId = _accountStore.CurrentUserId; // Assuming Account has UserId

            // Log the current user's ID
            Log.Information("UserId retrieved from AccountStore: {UserId}", userId);

            if (userId.HasValue)
            {
                Log.Information("UserId is valid: {UserId}", userId.Value);

                // Example: Fetching user data based on userId
                var user = await _userService.GetUserByIdAsync(userId.Value);

                if (user != null)
                {
                    Log.Information("User data retrieved for UserId: {UserId}", userId.Value);

                    var userProfileViewModel = new UserProfileViewModel(_userService, user.Data);
                    var userProfileView = new UserProfileView(_userService, user.Data)
                    {
                        DataContext = userProfileViewModel
                    };
                    userProfileView.ShowDialog();

                    Log.Information("User profile view shown for UserId: {UserId}", userId.Value);
                }
                else
                {
                    Log.Warning("No user data found for UserId: {UserId}", userId.Value);
                    MessageBox.Show("User not found.");
                }
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


    private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var registerViewModel = new RegisterViewModel(_authService, _accountStore, _sessionService); 
            var registerView = new RegisterWindow(_authService, _accountStore, _sessionService) 
            {
                DataContext = registerViewModel 
            };
            registerView.Show(); 
            
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var loginViewModel = new LoginViewModel(_authService, _accountStore, _sessionService); 
            var loginView = new LoginWindow(_authService, _accountStore, _sessionService) 
            {
                DataContext = loginViewModel 
            };
            loginView.Show(); 
            
        }

        private async void Journal_Click(object sender, RoutedEventArgs e)
        {
            int userId = 1; 
            var user = await _userService.GetUserByIdAsync(userId);

            if (user != null)
            {
                var journalViewModel = new JournalViewModel(
                    user.Data,
                    _readingProgressService,
                    _bookService
                );

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
