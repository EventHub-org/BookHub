using System.ComponentModel.DataAnnotations;

namespace BookHub.DAL.Entities
{
    public class UserEntity
    {
        [Key]
        public int UserId { get; set; }


        [Required(ErrorMessage = "Name is required.")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Name must be between 6 and 20 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StringLength(50, ErrorMessage = "Email cannot exceed 50 characters.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 20 characters.")]
        public string Password { get; set; }

        public string ProfilePicture { get; set; } //url

        public ICollection<CollectionEntity> Collections { get; set; }
        public ICollection<AchievementEntity> Achievments { get; set; }

        // Collection of users who initiated the "friendship"
        public virtual ICollection<FriendshipEntity> Inviters { get; set; }
        // Collection of users who where invited to the "friendship"
        public virtual ICollection<FriendshipEntity> Invitees { get; set; }
    }
}
