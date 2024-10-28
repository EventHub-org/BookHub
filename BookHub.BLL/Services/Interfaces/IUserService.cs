using BookHub.BLL.Utils;
using BookHub.DAL.DTO;

namespace BookHub.BLL.Services.Interfaces
{
    public interface IUserService
    {
        Task<ServiceResultType<PageDto<UserDto>>> GetPaginatedUsersAsync(Pageable pageable);
        Task<ServiceResultType<UserDto>> GetUserByIdAsync(int userId);
    }
}
