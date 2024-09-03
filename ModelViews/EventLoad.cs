using ResHub.Models;

namespace ResHub.ModelViews
{
    public class EventLoad
    {
        public string? EventName { get; set; }
        public Events.EventTypes Type { get; set; }
        public DateTime DateOfEvent { get; set; }
        public int ResidenceId { get; set; }
        public string? Description { get; set;}
    }
}
