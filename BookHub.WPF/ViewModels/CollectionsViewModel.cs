using BookHub.BLL.Services.Interfaces;
using BookHub.DAL.DTO; // Ensure this includes your CollectionDto class
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using Serilog;
using Microsoft.VisualBasic.ApplicationServices;

namespace BookHub.WPF.ViewModels
{
    public class CollectionsViewModel : INotifyPropertyChanged
    {
        private readonly ICollectionService _collectionService; // Use your collection service interface
        private ObservableCollection<CollectionDto> _collections = new ObservableCollection<CollectionDto>(); // Initialize here
        private CollectionDto _selectedCollection; // The currently selected collection
        private int _userId;

        public CollectionsViewModel(ICollectionService collectionService, int userId)
        {
            _userId = userId;
            _collectionService = collectionService;
            LoadCollectionsAsync().ConfigureAwait(false);
        }

        public ObservableCollection<CollectionDto> Collections
        {
            get => _collections;
            set
            {
                _collections = value;
                OnPropertyChanged(nameof(Collections));
            }
        }

        public CollectionDto SelectedCollection
        {
            get => _selectedCollection;
            set
            {
                _selectedCollection = value;
                OnPropertyChanged(nameof(SelectedCollection));
            }
        }
        
        private async Task LoadCollectionsAsync()
        {
            try
            {
                var result = await _collectionService.GetAllCollectionsAsync(_userId); 
                if (result.Success)
                {
                    Collections = new ObservableCollection<CollectionDto>(result.Data); 
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

        public async Task CreateCollectionAsync(string collectionName)
        {
            Log.Information("Starting to create a new collection...");

            try
            {
                var newCollection = new CollectionDto { Name = collectionName, UserId = _userId };
                var result = await _collectionService.CreateCollectionAsync(newCollection);

                if (result.Success)
                {
                    Collections.Add(newCollection);
                    Log.Information($"New collection '{collectionName}' created successfully.");
                }
                else
                {
                    Log.Error($"Failed to create collection: {result.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while creating the collection.");
            }
        }

        public async Task RefreshCollectionsAsync()
        {
            Log.Information("Refreshing collections...");
            await LoadCollectionsAsync();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
