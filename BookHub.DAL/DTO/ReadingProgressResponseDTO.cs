using System;

public class ReadingProgressResponseDTO
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int BookId { get; set; }
    public int CurrentPage { get; set; }
    public string Status { get; set; }
    public DateTime? DateFinished { get; set; }
}
