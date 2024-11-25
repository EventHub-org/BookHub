using AutoMapper;
using BookHub.BLL.Services.Interfaces;
using BookHub.DAL.DTO;
using BookHub.WPF.State.Accounts;
using BookHub.WPF.ViewModels;
using System;
using System.Windows;

namespace BookHub.WPF.Views
{
    /// <summary>
    /// Interaction logic for BooksView.xaml
    /// </summary>
    public partial class BooksFromCollectionView : Window
    {
        private readonly IAccountStore _accountStore;
        private readonly IBookService _bookService;
        private readonly ICollectionService _collectionService;

        public BooksFromCollectionView(BooksFromCollectionViewModel viewModel, IBookService bookService, ICollectionService collectionService, IAccountStore accountStore)
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            DataContext = viewModel;
            _bookService = bookService;
            _collectionService = collectionService;
            _accountStore = accountStore;
        }

        private void CollectionsButton_Click(object sender, RoutedEventArgs e)
        {
            if (_accountStore.IsUserAuthenticated())
            {
                int? userId = _accountStore.CurrentUserId;
                
                if (userId.HasValue)
                {
                    var collectionsViewModel = new CollectionsViewModel(_collectionService, userId.Value);
                    var collectionsView = new CollectionsView(collectionsViewModel, _bookService, _collectionService, _accountStore);
                    collectionsView.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("User authentication failed.");
                }
            }
            else
            {
                MessageBox.Show("No user is logged in.");
            }
        }
    }
}
