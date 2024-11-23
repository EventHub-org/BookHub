using BookHub.BLL.Utils;
using BookHub.DAL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookHub.BLL.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ServiceResultType<UserDto>> RegisterUserAsync(UserRegisterDto userRegistraterDto);
        Task<ServiceResultType<UserDto>> LoginUserAsync(UserLoginDto userLoginDto);
    }
}
