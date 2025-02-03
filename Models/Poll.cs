using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ResHub.Models
{
    public class Poll
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string CreatedByUserId { get; set; } // User who opened the poll

        [ForeignKey("CreatedByUserId")]
        public StudentResident CreatedBy { get; set; } // Navigation property

        [Required]
        public int ResidenceId { get; set; } // Poll is tied to a residence

        [ForeignKey("ResidenceId")]
        public Residence Residence { get; set; } // Navigation property

        [Required]
        public string Role { get; set; } // The role being voted on (Admin, BusCoordinator, etc.)

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // When the poll was created

        [Required]
        public DateTime ExpiresAt { get; set; } // Poll expiry

        public bool IsOpen { get; set; } = true; // Whether voting is still ongoing

        // Collection of votes for this poll
        public ICollection<Vote> Votes { get; set; } = new List<Vote>();

        // Check if poll is valid based on vote count
        public bool IsValid => Votes.Count >= 50 && DateTime.UtcNow <= ExpiresAt;
    }
}
