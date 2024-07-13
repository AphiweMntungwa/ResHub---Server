using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResHub.Models
{
    public class StudentResident : IdentityUser
    {
        public string StudentNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? RoomNumber { get; set; }


        // Foreign key property
        public int? ResidenceId { get; set; }
        public Residence? Residence { get; set; }

        public StudentResident() { }

        // Constructor initializing all non-nullable properties
        public StudentResident(string studentNumber, string firstName, string lastName, string email, string userName, int resId, string roomNo)
        {
            StudentNumber = studentNumber ?? throw new ArgumentNullException(nameof(studentNumber));
            FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
            Email = email;
            UserName = userName;
            ResidenceId = resId;
            RoomNumber = roomNo;
        }
    }
}
