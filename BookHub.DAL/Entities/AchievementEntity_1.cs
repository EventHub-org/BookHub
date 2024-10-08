using System.ComponentModel.DataAnnotations;

namespace BookHub.DAL.Entities
{
    public class AchievementEntity
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 50 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(255, MinimumLength = 20, ErrorMessage = "Description must be between 20 and 255 characters.")]
        public string Description { get; set; }
        public ICollection<UserEntity> Users { get; set; }
    }
}
