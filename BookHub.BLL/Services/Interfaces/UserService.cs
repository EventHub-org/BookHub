using BookHub.DAL.DTO;

namespace BookHub.BLL.Services.Interfaces
{
    public interface UserService
    {
        PageDto<UserDto> GetPaginatedUsers(int pageNumber, int pageSize);
        Task<UserDto> GetUserByIdAsync(int userId);
    }
}
