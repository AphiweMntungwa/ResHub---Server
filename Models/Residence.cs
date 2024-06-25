using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResHub.Models
{
    public class Residence
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ResId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Capacity { get; set; }

        // Nullable reference types
        public string? BusAdmin { get; set; }
        public string? RA { get; set; }
        public string? HouseComm { get; set; }

        //foreign keys
        public ICollection<StudentResident>? StudentResidents { get; set; }
        public ICollection<EventResidence>? EventResidences { get; set; }

        public Residence(string name, string address, string capacity)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Address = address ?? throw new ArgumentNullException(nameof(address));
            Capacity = capacity ?? throw new ArgumentNullException(nameof(capacity));
        }
    }
}
