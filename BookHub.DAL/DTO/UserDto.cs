
using System.ComponentModel.DataAnnotations;

namespace BookHub.DAL.DTO
{
    public class UserDto
    {
        public int UserId { get; set; }
      
        public string Name { get; set; }
   
        public string ProfilePictureUrl { get; set; }
        public IEnumerable<object>? Id { get; set; }
    }

}
