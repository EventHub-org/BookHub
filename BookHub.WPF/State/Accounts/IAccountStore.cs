using BookHub.DAL.DTO;
using Microsoft.Identity.Client.NativeInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookHub.WPF.State.Accounts
{
    public interface IAccountStore
    {
        Account CurrentAccount { get; set; }
        event Action StateChanged;
        public void SetCurrentUser(UserDto user);
        public void ClearSession();
        public bool IsUserAuthenticated();
        int? CurrentUserId { get; }

    }
}
