using System.ComponentModel.DataAnnotations;

public class ReadingProgressDTO
{
    [Required(ErrorMessage = "User ID is required.")]
    public int? UserId { get; set; }

    [Required(ErrorMessage = "Book ID is required.")]
    public int BookId { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Current page cannot be negative.")]
    [Required(ErrorMessage = "Current page is required")]
    public int CurrentPage { get; set; }
}