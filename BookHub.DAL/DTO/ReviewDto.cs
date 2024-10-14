using System.ComponentModel.DataAnnotations;

namespace BookHub.DAL.DTO
{
    public class ReviewDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "User ID is required.")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Book ID is required.")]
        public int BookId { get; set; }

        [Range(0, 5, ErrorMessage = "Rating must be between 0 and 5.")]
        public double Rating { get; set; }

        [MaxLength(1000, ErrorMessage = "Comment cannot exceed 1000 characters.")]
        public string Comment { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ReviewDto(int id, int userId, int bookId, double rating, string comment, DateTime createdAt)
        {
            Id = id;
            UserId = userId;
            BookId = bookId;
            Rating = rating;
            Comment = comment;
            CreatedAt = createdAt;
        }
    }
}
