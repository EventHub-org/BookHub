using System.ComponentModel.DataAnnotations;

namespace BookHub.DAL.DTO
{
    public class ReviewDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int BookId { get; set; }

        [Range(0, 5, ErrorMessage = "Rating must be between 0 and 5.")]
        public double Rating { get; set; }

        [MaxLength(1000, ErrorMessage = "Comment cannot exceed 1000 characters.")]
        public string Comment { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
