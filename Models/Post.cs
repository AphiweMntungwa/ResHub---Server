using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResHub.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public string OwnerId { get; set; } // Assuming you have a User model

        [ForeignKey("OwnerId")]
        public StudentResident Owner { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [NotMapped] // We calculate this dynamically
        public int LoveCount => Loves.Count;

        public ICollection<Love> Loves { get; set; } = new List<Love>();
    }
}
