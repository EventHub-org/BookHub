using BookHub.BLL.Services.Interfaces;
using BookHub.DAL.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;



namespace BookHub.WPF.ViewModels
{
    public class RegisterViewModel : INotifyPropertyChanged
    {
        private readonly IAuthService _authService;
        private readonly ISessionService _sessionService;

        public RegisterViewModel(IAuthService authService, ISessionService sessionService)
        {
            _authService = authService;
            _sessionService = sessionService;
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

            var result = await _authService.RegisterUserAsync(userDto);
            if (!result.Success)
            {
                ErrorMessage = result.ErrorMessage;
            }
            else
            {
                ErrorMessage = "Registration successful!";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
