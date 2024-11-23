using BookHub.BLL.Services.Interfaces;
using BookHub.DAL.DTO;
using System;

namespace BookHub.BLL.Services.Implementations
{
    public class SessionService : ISessionService
    {
        private UserDto _currentUser;

        public UserDto CurrentUser
        {
            get => _currentUser;
        }

        public void SetCurrentUser(UserDto user)
        {
            _currentUser = user;
        }

        public UserDto GetCurrentUser()
        {
            return _currentUser;
        }

        public bool IsUserAuthenticated()
        {
            return _currentUser != null;
        }

        public void ClearSession()
        {
            _currentUser = null;
        }
    }
}
