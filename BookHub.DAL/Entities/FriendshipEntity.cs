using Microsoft.VisualBasic.ApplicationServices;
using System.ComponentModel.DataAnnotations;

namespace BookHub.DAL.Entities
{
    public class FriendshipEntity
    {
        public int User1Id { get; set; }
        public UserEntity User1 { get; set; }
        public int User2Id { get; set; }
        public UserEntity User2 { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        [StringLength(20, ErrorMessage = "Email cannot exceed 20 characters.")]
        public string Status { get; set; }
    }
}