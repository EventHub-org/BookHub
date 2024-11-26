using System.Windows;
using BookHub.WPF.ViewModels;

namespace BookHub.WPF.Views
{
    public partial class ReadingListsView : Window
    {
        private readonly ReadingListsViewModel _viewModel;

        public ReadingListsView(ReadingListsViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;
            _viewModel.CloseWindowAction = this.Close;
            DataContext = _viewModel;

            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
