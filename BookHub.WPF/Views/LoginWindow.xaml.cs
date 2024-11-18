using BookHub.BLL.Services.Interfaces;
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
        public LoginWindow(IAuthService authService)
        {
            InitializeComponent();
            DataContext = new LoginViewModel(authService);
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
