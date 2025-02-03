using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ResHub.Models
{
    public class Vote
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int PollId { get; set; } // The poll this vote belongs to

        [ForeignKey("PollId")]
        public Poll Poll { get; set; } // Navigation property

        [Required]
        public string VotingUserId { get; set; } // The user who is voting

        [ForeignKey("VotingUserId")]
        public StudentResident VotingUser { get; set; } // Navigation property

        [Required]
        public string VotedForUserId { get; set; } // The user being nominated

        [ForeignKey("VotedForUserId")]
        public StudentResident VotedForUser { get; set; } // Navigation property

        [Required]
        public DateTime VotedAt { get; set; } = DateTime.UtcNow; // When the vote was cast
    }
}
