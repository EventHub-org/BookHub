using System.Windows;
using BookHub.BLL.Services.Interfaces;
using BookHub.WPF.ViewModels;

namespace BookHub.WPF.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow(IBookService bookService)
        {
            InitializeComponent();
            DataContext = new MainViewModel(bookService);
        }
    }
}
