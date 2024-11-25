using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using BookHub.BLL.Services.Interfaces;
using BookHub.DAL.DTO;
using Serilog;

namespace BookHub.WPF.ViewModels
{
    public class ReadingListsViewModel : INotifyPropertyChanged
    {
        private readonly ICollectionService _collectionService;
        private ObservableCollection<CollectionDto> _collections = new ObservableCollection<CollectionDto>();
        private int _userId;
        private int _bookId;

        public ICommand AddToListCommand { get; }
        public Action CloseWindowAction { get; set; } // Action to close the window

        public ReadingListsViewModel(ICollectionService collectionService, int userId, int bookId)
        {
            _userId = userId;
            _bookId = bookId;
            _collectionService = collectionService;

            LoadCollectionsAsync().ConfigureAwait(false);

            AddToListCommand = new RelayCommand<CollectionDto>(AddToList);
        }

        public ObservableCollection<CollectionDto> ReadingLists
        {
            get => _collections;
            set
            {
                _collections = value;
                OnPropertyChanged(nameof(ReadingLists));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private async void AddToList(CollectionDto collection)
        {
            try
            {
                await _collectionService.AddBookToCollectionAsync(collection.Id, _bookId);
                

                CloseWindowAction?.Invoke();

                MessageBox.Show($"Книгу додано до списку '{collection.Name}'!");
            }
            catch (Exception ex)
            {

                Log.Error(ex, $"An error occurred while adding the book to the collection. collId: ${collection.Id} bookId: ${_bookId}" );
                MessageBox.Show("Не вдалося додати книгу до списку. Будь ласка, спробуйте ще раз.");
            }
        }

        private async Task LoadCollectionsAsync()
        {
            try
            {
                var result = await _collectionService.GetAllCollectionsAsync(_userId);
                if (result.Success)
                {
                    ReadingLists = new ObservableCollection<CollectionDto>(result.Data);
                    Log.Information($"Loaded {result.Data.Count} collections.");
                }
                else
                {
                    Log.Error($"Failed to load collections: {result.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while loading collections.");
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
