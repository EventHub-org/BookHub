using BookHub.BLL.Utils;
using BookHub.DAL.DTO;
using System.Threading.Tasks;

namespace BookHub.BLL.Services.Interfaces
{
    public interface IUserService
    {
        Task<ServiceResultType<PageDto<UserDto>>> GetPaginatedUsersAsync(int pageNumber, int pageSize);
        Task<ServiceResultType<UserDto>> GetUserByIdAsync(int userId);
    }
}
