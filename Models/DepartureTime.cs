
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResHub.Models
{
    public class DepartureTime
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DepartureTimeId { get; set; }
        public TimeSpan Time { get; set; }

        public enum Directions
        {
            FromResidence = 1,
            ToResidence = 2
        }

        public Directions Direction { get; set; }

        public int BusId { get; set; }

        [ForeignKey("BusId")]
        public virtual Bus Bus { get; set; }
    }

}
