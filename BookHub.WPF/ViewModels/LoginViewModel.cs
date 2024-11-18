using BookHub.BLL.Services.Interfaces;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using BookHub.DAL.DTO;

namespace BookHub.WPF.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly IAuthService _authService;

        public LoginViewModel(IAuthService authService)
        {
            _authService = authService;
            LoginCommand = new RelayCommand(async () => await LoginAsync(), () => CanLogin);
        }

        private string _email;
        private string _password;
        private string _errorMessage;

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

            var result = await _authService.LoginUserAsync(userDto);
            if (!result.Success)
            {
                ErrorMessage = result.ErrorMessage;
            }
            else
            {
                ErrorMessage = "Login successful!";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
