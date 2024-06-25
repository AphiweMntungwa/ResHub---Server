using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResHub.Models
{
    public class StudentResident
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string StudentNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string? RoomNumber { get; set; }


        // Foreign key property
        public int? ResidenceId { get; set; }
        public Residence? Residence { get; set; }


        // Constructor initializing all non-nullable properties
        public StudentResident(string studentNumber, string firstName, string lastName, string userName)
        {
            StudentNumber = studentNumber ?? throw new ArgumentNullException(nameof(studentNumber));
            FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
            UserName = userName ?? throw new ArgumentNullException(nameof(userName));
        }
    }
}
