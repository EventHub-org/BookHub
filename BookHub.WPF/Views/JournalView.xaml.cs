using BookHub.WPF.ViewModels;
using System.Windows;

namespace BookHub.WPF.Views
{
    public partial class JournalView : Window
    {
        public JournalView(JournalViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
