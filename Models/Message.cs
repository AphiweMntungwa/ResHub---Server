using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ResHub.Models
{

    public class Message
    {
        [Key]
        public int MessageId { get; set; } // Primary Key
        [Required]
        public string Content { get; set; } // Message content
        public DateTime Timestamp { get; set; } = DateTime.UtcNow; // Timestamp of the message
        public bool IsRead { get; set; } = false; // Indicator if the message has been read




        //foreign keys
        [ForeignKey("Sender")]
        public string SenderId { get; set; } // Foreign Key referencing Users.UserId

        [ForeignKey("Receiver")]
        public string ReceiverId { get; set; } // Foreign Key referencing Users.UserId

        // Navigation properties
        public virtual StudentResident Sender { get; set; }
        public virtual StudentResident Receiver { get; set; }
    }
}
