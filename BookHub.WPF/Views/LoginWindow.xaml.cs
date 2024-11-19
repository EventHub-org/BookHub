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
            DataContext = new LoginViewModel(authService, accountStore, sessionService);
        }

        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel vm && sender is PasswordBox passwordBox)
            {
                vm.Password = passwordBox.Password;
            }
        }
    }
}
