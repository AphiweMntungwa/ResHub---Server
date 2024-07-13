using ResHub.ModelViews;
using System.ComponentModel.DataAnnotations;

namespace ResHub.Models
{
    public class RegisterViewModel : BaseAuth
    {
        public RegisterViewModel(string email, string firstName, string lastName, string studentNumber, string userName, int residenceId, string roomNumber, string password)
            : base(email, firstName, lastName, userName)
        {
            StudentNumber = studentNumber;
            ResidenceId = residenceId;
            RoomNumber = roomNumber;
            Password = password;
        }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string StudentNumber { get; set; }

        [Required]
        public int ResidenceId { get; set; }

        [Required]
        public string RoomNumber { get; set; }
    }
}
