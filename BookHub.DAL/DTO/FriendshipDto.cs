using System.ComponentModel.DataAnnotations;

namespace BookHub.DAL.DTO
{
    public class FriendshipDto
    {
        public int User1Id { get; set; }
        public UserDto User1 { get; set; }
        public int User2Id { get; set; }
        public UserDto User2 { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        [StringLength(20, ErrorMessage = "Status cannot exceed 20 characters.")]
        public string Status { get; set; }
    }
}