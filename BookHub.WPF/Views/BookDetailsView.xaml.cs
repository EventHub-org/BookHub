using BookHub.BLL.Services.Implementations;
using BookHub.BLL.Services.Interfaces;
using BookHub.WPF.State.Accounts;
using BookHub.WPF.ViewModels;
using System.Windows;
using System.Windows.Controls;
using Serilog;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BookHub.WPF.Views
{
    public partial class BookDetailsView : Page
    {
        private readonly IAccountStore _accountStore;
        private readonly ICollectionService _collectionService;
        private readonly int _bookId;
        private readonly BookDetailsViewModel _viewModel;
        //private bool _isAddToListEnabled = true;
        public BookDetailsView()
        {
            InitializeComponent();
        }

        public BookDetailsView(IAccountStore accountStore, ICollectionService collectionService, int bookId, BookDetailsViewModel viewModel)
        {
            _accountStore = accountStore;
            _collectionService = collectionService;
            _bookId = bookId;
            InitializeComponent(); 
            _viewModel = viewModel;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Home button click handler
        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void openCollectionsView(object sender, RoutedEventArgs e)
        {
            if (_accountStore.IsUserAuthenticated())
            {
                int? userId = _accountStore.CurrentUserId;

                Log.Information("UserId retrieved from AccountStore: {UserId}", userId);

                if (userId.HasValue)
                {
                    _viewModel.IsAddToListEnabled = false;
                    var readingListsViewModel = new ReadingListsViewModel(_collectionService, userId.Value, _bookId);
                    var readingListsView = new ReadingListsView(readingListsViewModel);
                    readingListsView.Show();
                    
                }
                else
                {
                    Log.Warning("UserId is null or invalid for the current account.");
                    MessageBox.Show("User is not authenticated.");
                }
            }
            else
            {
                Log.Warning("No user is currently logged in.");
                MessageBox.Show("No user is logged in.");
            }
        }

    }
}
