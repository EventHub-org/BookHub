using BookHub.BLL.Services.Implementations;
using BookHub.BLL.Services.Interfaces;
using BookHub.WPF.State.Accounts;
using BookHub.WPF.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace BookHub.WPF.Views
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow(IAuthService authService,IAccountStore accountStore, ISessionService sessionService)
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            DataContext = new LoginViewModel(authService, accountStore, sessionService);
        }

        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel vm && sender is PasswordBox passwordBox)
            {
                vm.Password = passwordBox.Password;
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
