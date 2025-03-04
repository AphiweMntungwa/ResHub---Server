using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResHub.Models
{
    public class Love
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ReactorId { get; set; }

        [ForeignKey("ReactorId")]
        public StudentResident Reactor { get; set; }

        [Required]
        public int PostId { get; set; }
        public Post Post { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
