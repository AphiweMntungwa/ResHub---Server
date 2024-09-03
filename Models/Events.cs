using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Policy;

namespace ResHub.Models
{
    public class Events
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string EventName { get; set; }
        public enum EventTypes
        {
            Sports,
            Recreation,
            Formal,
            Religious
        }
        public string Description { get; set; }
        public EventTypes Type { get; set; }
        public DateTime DateOfEvent { get; set; }

        // Foreign key property
        public int? ResidenceId { get; set; }
        public Residence? Residence { get; set; }


        // Constructor to initialize fields
        public Events(string eventName, EventTypes type, DateTime dateOfEvent, string description)
        {
            EventName = eventName ?? throw new ArgumentNullException(nameof(eventName));
            Type = type;
            DateOfEvent = dateOfEvent;
            Description = description;
        }
    }
}
