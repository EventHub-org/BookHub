using BookHub.BLL.Services.Interfaces;
using BookHub.DAL.DTO;
using Microsoft.Identity.Client.NativeInterop;
using System;

namespace BookHub.WPF.State.Accounts
{
    public class AccountStore : IAccountStore
    {
        private Account _currentAccount;
        private UserDto _currentUser;
        private readonly ISessionService _sessionService;

        // Constructor to inject SessionService
        public AccountStore(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        // Property to store and retrieve the current account
        public Account CurrentAccount
        {
            get => _currentAccount;
            set
            {
                _currentAccount = value;
                StateChanged?.Invoke();
            }
        }

        public UserDto CurrentUser
        {
            get => _currentUser;
            private set
            {
                _currentUser = value;
                _sessionService.SetCurrentUser(value);  
                StateChanged?.Invoke();
            }
        }

        public int? CurrentUserId => _currentUser?.UserId;

        public event Action StateChanged;

        public void SetCurrentUser(UserDto user)
        {
            CurrentUser = user;
        }

        public void ClearSession()
        {
            CurrentUser = null;
            CurrentAccount = null;
        }

        public bool IsUserAuthenticated() => _currentUser != null;
    }
}
