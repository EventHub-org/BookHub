using System;
using Microsoft.VisualBasic.ApplicationServices;
using System.ComponentModel.DataAnnotations;

namespace BookHub.DAL.Entities
{
    public class FriendshipEntity
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "User ID is required.")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Friend ID is required.")]
        public int FriendId { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public UserEntity User { get; set; }
        public UserEntity Friend { get; set; }


        [Required(ErrorMessage = "Status is required.")]
        [StringLength(20, ErrorMessage = "Email cannot exceed 20 characters.")]
        public string Status { get; set; }
    }
}
