using BookHub.BLL.Utils;
using BookHub.DAL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookHub.BLL.Services.Interfaces
{
    public interface IFriendshipService
    {
        
        Task<ServiceResultType<FriendshipDto>> AddFriendRequestAsync(FriendshipDto friendshipDto);
        Task<ServiceResultType<FriendshipDto>> AcceptFriendRequestAsync(int user1Id, int user2Id);
        Task<ServiceResultType<bool>> RemoveFriendshipAsync(int user1Id, int user2Id);
        Task<ServiceResultType<FriendshipDto>> GetFriendshipAsync(int user1Id, int user2Id);


    }
}
