using BookHub.BLL.Services.Interfaces;
using BookHub.DAL.DTO;
using BookHub.WPF.Views;
using BookHub.WPF.State.Accounts;
using Serilog; // Add the Serilog namespace for logging
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookHub.WPF.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly IAuthService _authService;
        private readonly IAccountStore _accountStore;
        private readonly ISessionService _sessionService;

        public LoginViewModel(IAuthService authService, IAccountStore accountStore, ISessionService sessionService)
        {
            _authService = authService;
            _accountStore = accountStore;
            _sessionService = sessionService;
            LoginCommand = new RelayCommand(async () => await LoginAsync(), () => CanLogin);
            IsAuthenticated = _sessionService.IsUserAuthenticated();
        }

        private string _email;
        private string _password;
        private string _errorMessage;
        private bool _isAuthenticated;

        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); UpdateLoginState(); }
        }

        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); UpdateLoginState(); }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(); }
        }

        public bool IsAuthenticated
        {
            get => _isAuthenticated;
            set { _isAuthenticated = value; OnPropertyChanged(); }
        }

        public ICommand LoginCommand { get; }

        public bool CanLogin =>
            !string.IsNullOrWhiteSpace(Email) &&
            !string.IsNullOrWhiteSpace(Password);

        private void UpdateLoginState()
        {
            if (LoginCommand is RelayCommand command)
            {
                command.RaiseCanExecuteChanged();
            }
        }

        private async Task LoginAsync()
        {
            var userDto = new UserLoginDto
            {
                Email = Email,
                Password = Password
            };

            Log.Information("Attempting to log in with email: {Email}", Email);

            var result = await _authService.LoginUserAsync(userDto);

            if (!result.Success)
            {
                Log.Error("Login failed for email: {Email}. Error: {ErrorMessage}", Email, result.ErrorMessage);
                ErrorMessage = result.ErrorMessage;
            }
            else
            {
                Log.Information("Login successful for email: {Email}. ", Email);
                ErrorMessage = "Login successful!";

                _accountStore.SetCurrentUser(result.Data);
                //_sessionService.SetCurrentUser(result.Data);

                //IsAuthenticated = _accountStore.IsUserAuthenticated();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
