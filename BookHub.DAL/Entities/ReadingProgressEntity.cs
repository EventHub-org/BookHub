using System;
using System.ComponentModel.DataAnnotations;

namespace BookHub.DAL.Entities
{
    public class ReadingProgressEntity
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "User ID is required.")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Book ID is required.")]
        public int BookId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Current page cannot be negative.")]
        public int CurrentPage { get; set; }

        public string Status { get; set; } // Optional: could be enum

        [DataType(DataType.DateTime)]
        public DateTime? DateFinished { get; set; }

        public BookEntity Book { get; set; }
        public UserEntity User { get; set; }
    }
}
