using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResHub.Models
{
    public class Bus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BusId { get; set; }
        public string BusNumber { get; set; }


        public string? BusDriver { get; set; }
        public string? BusDriverPhoneNumber { get; set; }

        public DateTime LastUpdated { get; set; }

        public int ResidenceId { get; set; }
        public string LastUpdatedByUserId { get; set; }
        public virtual ICollection<DepartureTime> DepartureTimes { get; set; } = new List<DepartureTime>();

        [ForeignKey("ResidenceId")]
        public virtual Residence Residence { get; set; }
    }
}
