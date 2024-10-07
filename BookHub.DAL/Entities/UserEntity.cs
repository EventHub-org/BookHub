
namespace BookHub.DAL.Entities
{
    public class UserEntity
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ProfilePicture { get; set; } //url

        public ICollection<CollectionEntity> Collections { get; set; }
        public ICollection<AchievmentEntity> Achievments { get; set; }
    }
}
