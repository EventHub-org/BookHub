using BookHub.DAL.DTO;
using System.Threading.Tasks;

namespace BookHub.BLL.Services.Interfaces
{
    public interface IUserService
    {
        Task<PageDto<UserDto>> GetPaginatedUsers(int pageNumber, int pageSize);
        Task<UserDto> GetUserByIdAsync(int userId);
    }
}
