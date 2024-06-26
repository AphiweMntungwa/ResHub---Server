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

        public EventTypes Type { get; set; }
        public DateTime DateOfEvent { get; set; }
        public ICollection<EventResidence>? EventResidences { get; set; }

        // Constructor to initialize fields
        public Events(string eventName, EventTypes type, DateTime dateOfEvent)
        {
            EventName = eventName ?? throw new ArgumentNullException(nameof(eventName));
            Type = type;
            DateOfEvent = dateOfEvent;
        }
    }
}
