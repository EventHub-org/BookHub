using BookHub.DAL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookHub.BLL.Services.Interfaces
{
    public interface ISessionService
    {
        UserDto CurrentUser { get; }
        // Зберегти поточного користувача
        void SetCurrentUser(UserDto user);

        // Отримати поточного користувача
        UserDto GetCurrentUser();

        // Перевірити, чи є авторизований користувач
        bool IsUserAuthenticated();

        // Очистити сесію (вийти з системи)
        void ClearSession();
    }
}

