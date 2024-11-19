using BookHub.BLL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BookHub.WPF.ViewModels;
using BookHub.WPF.State.Accounts;

namespace BookHub.WPF.Views
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow(IAuthService authService, IAccountStore accountStore,ISessionService sessionService)
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            DataContext = new RegisterViewModel(authService, accountStore, sessionService);
        }

        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is RegisterViewModel vm && sender is PasswordBox passwordBox)
            {
                vm.Password = passwordBox.Password;
            }
        }

        private void OnRepeatPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is RegisterViewModel vm && sender is PasswordBox passwordBox)
            {
                vm.RepeatPassword = passwordBox.Password;
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
