using BookHub.WPF.ViewModels;
using System.Windows;

namespace BookHub.WPF.Views
{
    public partial class JournalView : Window
    {
        public JournalView(JournalViewModel viewModel)
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            DataContext = viewModel;
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
