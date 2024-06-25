namespace ResHub.Models
{
    public class EventResidence
    {

        public int ResidenceId { get; set; }
        public Residence? Residence { get; set; }

        public int EventId { get; set; }
        public Events? Event { get; set; }

    }
}
