using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookHub.DAL.Entities
{
    public class BookEntity
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [MaxLength(255, ErrorMessage = "Title cannot exceed 255 characters.")]
        public string Title { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Number of pages must be greater than 0.")]
        public int NumberOfPages { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Published date is required.")]
        public DateTime PublishedDate { get; set; }

        [Url(ErrorMessage = "Cover image must be a valid URL.")]
        public string CoverImageUrl { get; set; }

        [Range(0, 5, ErrorMessage = "Rating must be between 0 and 5.")]
        public double Rating { get; set; }

        [Required(ErrorMessage = "Author is required.")]
        [MaxLength(255, ErrorMessage = "Author name cannot exceed 255 characters.")]
        public string Author { get; set; }


        [MaxLength(255, ErrorMessage = "Genre cannot exceed 255 characters.")]
        public string Genre { get; set; }

        public ICollection<ReviewEntity> Reviews { get; set; }

        public ICollection<CollectionEntity> Collections { get; set; }
    }
}
