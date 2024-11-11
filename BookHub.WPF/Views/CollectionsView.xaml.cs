using BookHub.DAL.DTO;
using BookHub.WPF.ViewModels;
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

namespace BookHub.WPF.Views
{
    /// <summary>
    /// Interaction logic for CollectionsView.xaml
    /// </summary>
    public partial class CollectionsView : Window
    {
        private readonly CollectionsViewModel _viewModel; // Keep a reference to the ViewModel

        public CollectionsView(CollectionsViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel; // Set the DataContext
            _viewModel = viewModel; // Store the ViewModel for later use
        }

        private void CreateNewCollection_Click(object sender, RoutedEventArgs e)
        {
            // Logic for creating a new collection goes here
            var newCollection = new CollectionDto(); // Create a new CollectionDto
            // Here you can prompt the user for details, for example through a dialog.

            // Add the new collection to the ObservableCollection in your ViewModel
            _viewModel.Collections.Add(newCollection);
            // You can also handle showing a dialog or a form to fill in details for the new collection.

        }
    }

}
