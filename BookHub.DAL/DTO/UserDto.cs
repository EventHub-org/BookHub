
using System.ComponentModel.DataAnnotations;

namespace BookHub.DAL.DTO
{
    public class UserDto
    {
        public int Id { get; set; }

      
        public string Name { get; set; }

  
        public string Email { get; set; }

        
        public string ProfilePictureUrl { get; set; }

        public UserDto() { }
        public UserDto(int id, string name, string email, string profilePictureUrl)
        {
            Id = id;
            Name = name;
            Email = email;
            ProfilePictureUrl = profilePictureUrl;

        }
    }

}
