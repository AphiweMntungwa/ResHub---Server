using Microsoft.AspNetCore.Identity;
using ResHub.ModelViews;
using System.ComponentModel.DataAnnotations;

namespace ResHub.Models
{
    public class RegisterViewModel : BaseAuth
    {
        public RegisterViewModel(string email, string firstName, string lastName, string studentNumber, string userName, int residenceId, string roomNumber, string password)
            : base(email, password)
        {
            StudentNumber = studentNumber;
            ResidenceId = residenceId;
            RoomNumber = roomNumber;
            FirstName = firstName;
            LastName = lastName;
            UserName = userName;
        }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string StudentNumber { get; set; }

        [Required]
        public int ResidenceId { get; set; }

        [Required]
        public string RoomNumber { get; set; }
        public bool Successful { get; set; }
        public IEnumerable<IdentityError>? Errors { get; set; }
    }
}
