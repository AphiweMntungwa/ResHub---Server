using ResHub.Models;

namespace ResHub.ModelViews
{
    public class StudentResidentViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public int? ResidenceId { get; set; }
        public string RoomNumber { get; set; }

        // Constructor to map from StudentResident
        public StudentResidentViewModel(StudentResident student)
        {
            Id = student.Id;
            FirstName = student.FirstName;
            LastName = student.LastName;
            Email = student.Email;
            UserName = student.UserName;
            ResidenceId = student.ResidenceId;
            RoomNumber = student.RoomNumber;
        }
    }
    public class PollRequestViewModel
    {
        public string CreatedByUserId { get; set; } // User who is creating the poll
        public int ResidenceId { get; set; } // Residence the poll is associated with
        public string Role { get; set; } // Role being voted on
        public DateTime ExpiresAt { get; set; } // Expiry date of the poll
    }

    public class PollResponseViewModel
    {
        public int Id { get; set; }
        public string CreatedByUserId { get; set; }
        public string CreatedByUserName { get; set; } // Optional for readability
        public int ResidenceId { get; set; }
        public string Role { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool IsOpen { get; set; }
        public bool Successful { get; set; }    
        public string Message { get; set; }
        public int VoteCount { get; set; } // Number of votes cast
    }

    public class VoteRequestViewModel
    {
        public int PollId { get; set; } // The poll the vote belongs to
        public string VotingUserId { get; set; } // The user who is voting
        public string VotedForUserId { get; set; } // The user being nominated
    }

    public class VoteResponseViewModel
    {
        public int Id { get; set; }
        public int PollId { get; set; }
        public string VotingUserId { get; set; }
        public string VotingUserName { get; set; } // Optional for readability
        public string VotedForUserId { get; set; }
        public string VotedForUserName { get; set; } // Optional for readability
        public DateTime VotedAt { get; set; }
        public bool Successful { get; set; }
        public string Message { get; set; }
    }

}
