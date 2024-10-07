namespace BookHub.DAL.Entities
{
    public class AchievmentEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<UserEntity> Users { get; set; }
    }
}
