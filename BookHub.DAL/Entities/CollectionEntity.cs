namespace BookHub.DAL.Entities
{
    public class CollectionEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public UserEntity User { get; set; }
        public int UserId { get; set; }
    }
}
