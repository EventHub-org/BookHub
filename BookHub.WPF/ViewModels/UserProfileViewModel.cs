using BookHub.DAL.DTO; // Переконайтеся, що у вас є простір імен для UserDto
using System.ComponentModel;

namespace BookHub.WPF.ViewModels
{
    public class UserProfileViewModel : INotifyPropertyChanged
    {
        private string _name;
        //private string _email;
        private string _profilePicture;

        public UserProfileViewModel(UserDto user)
        {
            Name = user.Name;
            ProfilePictureUrl = user.ProfilePictureUrl;
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        //public string Email
        //{
        //    get => _email;
        //    set
        //    {
        //        _email = value;
        //        OnPropertyChanged(nameof(Email));
        //    }
        //}

        public string ProfilePictureUrl
        {
            get => _profilePicture;
            set
            {
                _profilePicture = value;
                OnPropertyChanged(nameof(ProfilePictureUrl));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
