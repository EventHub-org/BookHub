using BookHub.DAL.DTO;
using System.Threading.Tasks;

namespace BookHub.BLL.Services.Interfaces
{
    public interface IUserService
    {
        Task<PageDto<UserDto>> GetPaginatedUsersAsync(int pageNumber, int pageSize);
        Task<UserDto> GetUserByIdAsync(int userId);
    }
}
