
namespace BookHub.DAL.DTO
{
    public class Pageable
    {
        public int Size { get; set; }
        public int Page { get; set; }

        public Pageable(int size, int page)
        {
            Size = size;
            Page = page;
        }

        public Pageable() : this(10, 0)
        {
        }
    }
}
