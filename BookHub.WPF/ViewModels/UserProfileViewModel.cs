using BookHub.BLL.Services.Interfaces;
using BookHub.DAL.DTO;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookHub.WPF.ViewModels
{
    public class UserProfileViewModel : INotifyPropertyChanged
    {
        private readonly IUserService _userService;
        private ObservableCollection<UserDto> _otherUsers;
        private UserDto _selectedUser;
        private int _currentPage;
        private const int _pageSize = 10; // Number of users per page
        private int _totalPages;

        public UserProfileViewModel(IUserService userService, UserDto userDto)
        {
            _userService = userService;
            _selectedUser = userDto;
            CurrentPage = 1;

            // Initialize commands
            PreviousPageCommand = new RelayCommand(PreviousPage, CanGoToPreviousPage);
            NextPageCommand = new RelayCommand(NextPage, CanGoToNextPage);

            // Load other users asynchronously
            LoadUsersAsync().ConfigureAwait(false);
        }

        // Method to load other users asynchronously
        private async Task LoadUsersAsync()
        {
            var result = await _userService.GetPaginatedUsersAsync(new Pageable { Page = CurrentPage, Size = _pageSize });

            if (result.Success)
            {
                OtherUsers = new ObservableCollection<UserDto>(result.Data.Items);
                TotalPages = result.Data.TotalPages; // Assuming the service returns the total pages
            }
        }

        public ObservableCollection<UserDto> OtherUsers
        {
            get => _otherUsers;
            set
            {
                _otherUsers = value;
                OnPropertyChanged(nameof(OtherUsers));
            }
        }

        public UserDto SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged(nameof(SelectedUser));
            }
        }

        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                OnPropertyChanged(nameof(CurrentPage));
                LoadUsersAsync().ConfigureAwait(false);
            }
        }

        public int TotalPages
        {
            get => _totalPages;
            private set
            {
                _totalPages = value;
                OnPropertyChanged(nameof(TotalPages));
            }
        }

        public ICommand PreviousPageCommand { get; }
        public ICommand NextPageCommand { get; }

        private void PreviousPage()
        {
            if (CanGoToPreviousPage())
            {
                CurrentPage--;
            }
        }

        private void NextPage()
        {
            if (CanGoToNextPage())
            {
                CurrentPage++;
            }
        }

        private bool CanGoToPreviousPage() => CurrentPage > 1;

        private bool CanGoToNextPage() => CurrentPage < TotalPages;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
