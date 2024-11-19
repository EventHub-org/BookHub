using BookHub.BLL.Services.Interfaces;
using BookHub.DAL.DTO;
using BookHub.WPF.State.Accounts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Serilog;


namespace BookHub.WPF.ViewModels
{
    public class RegisterViewModel : INotifyPropertyChanged
    {
        private readonly IAuthService _authService;
        private readonly ISessionService _sessionService;

        private readonly IAccountStore _accountStore;

        public RegisterViewModel(IAuthService authService, IAccountStore accountStore, ISessionService sessionService)
        {
            _authService = authService;
            _sessionService = sessionService;
            _accountStore = accountStore;
            RegisterCommand = new RelayCommand(async () => await RegisterAsync(), () => CanRegister);
        }

        private string _name;
        private string _email;
        private string _password;
        private string _repeatPassword;
        private string _errorMessage;

        public ICommand RegisterCommand { get; }
        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); UpdateRegisterState(); }
        }

        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); UpdateRegisterState(); }
        }

        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); UpdateRegisterState(); }
        }

        public string RepeatPassword
        {
            get => _repeatPassword;
            set { _repeatPassword = value; OnPropertyChanged(); UpdateRegisterState(); }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(); }
        }


        public bool CanRegister =>
            !string.IsNullOrWhiteSpace(Name) &&
            !string.IsNullOrWhiteSpace(Email) &&
            !string.IsNullOrWhiteSpace(Password) &&
            Password == RepeatPassword;

        private void UpdateRegisterState()
        {
            ErrorMessage = Password != RepeatPassword ? "Passwords do not match." : string.Empty;

            // Викликаємо зміну стану команди
            if (RegisterCommand is RelayCommand command)
            {
                command.RaiseCanExecuteChanged();
            }
        }


        private async Task RegisterAsync()
        {
            var userDto = new UserRegisterDto
            {
                Name = Name,
                Email = Email,
                Password = Password
            };
            Log.Information("Attempting to register with email: {Email}", Email);

            var result = await _authService.RegisterUserAsync(userDto);
            if (!result.Success)
            {
                Log.Error("Registration failed for email: {Email}. Error: {ErrorMessage}", Email, result.ErrorMessage);

                ErrorMessage = result.ErrorMessage;
            }
            else
            {
                Log.Information("Registration successful for email: {Email}. ", Email);
                _accountStore.SetCurrentUser(result.Data);
                //_sessionService.SetCurrentUser(result.Data);

                //IsAuthenticated = _sessionService.IsUserAuthenticated();
                ErrorMessage = "Registration successful!";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
