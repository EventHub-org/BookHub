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
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            DataContext = viewModel;
            _collectionService = collectionService;
            _readingProgressService = readingProgressService;
            _bookService = bookService;
            _userService = userService;
            _authService = authService;
            _sessionService = sessionService;
            _accountStore = accountStore;
        }


        private void CollectionsButton_Click(object sender, RoutedEventArgs e)
        {
            if (_accountStore.IsUserAuthenticated())
            {
                int? userId = _accountStore.CurrentUserId;

                Log.Information("UserId retrieved from AccountStore: {UserId}", userId);

                if (userId.HasValue)
                {
                    var collectionsViewModel = new CollectionsViewModel(_collectionService, userId.Value);
                    var collectionsView = new CollectionsView(collectionsViewModel);
                    collectionsView.Show();
                    this.Hide();
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

        private async void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            Log.Information("Profile button clicked.");

            if (_accountStore.IsUserAuthenticated())
            {
                int? userId = _accountStore.CurrentUserId;

                Log.Information("UserId retrieved from AccountStore: {UserId}", userId);

                if (userId.HasValue)
                {
                    Log.Information("UserId is valid: {UserId}", userId.Value);

                    var user = await _userService.GetUserByIdAsync(userId.Value);

                    if (user != null)
                    {
                        Log.Information("User data retrieved for UserId: {UserId}", userId.Value);

                        var userProfileViewModel = new UserProfileViewModel(_userService, user.Data);
                        var userProfileView = new UserProfileView(_userService, user.Data)
                        {
                            DataContext = userProfileViewModel
                        };

                        userProfileView.Show();
                        this.Hide();

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
            this.Hide();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var loginViewModel = new LoginViewModel(_authService, _accountStore, _sessionService);
            var loginView = new LoginWindow(_authService, _accountStore, _sessionService)
            {
                DataContext = loginViewModel
            };

            loginView.Show();
            this.Hide();
        }

        private async void Journal_Click(object sender, RoutedEventArgs e)
        {
            if (_accountStore.IsUserAuthenticated())
            {
                int? userId = _accountStore.CurrentUserId;

                if (userId.HasValue)
                {
                    var user = await _userService.GetUserByIdAsync(userId.Value);

                    if (user != null)
                    {
                        var journalViewModel = new JournalViewModel(
                            user.Data,
                            _readingProgressService,
                            _bookService,
                            userId.Value
                        );

                        var journalView = new JournalView(journalViewModel);
                        journalView.Show();
                        this.Hide();
                    }
                    else
                    {
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

        public void NavigateToPage(Page page)
        {
            var frame = new Frame();
            Content = frame;
            frame.Navigate(page);
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            _accountStore.ClearSession();
        }
    }
}
