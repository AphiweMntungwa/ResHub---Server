using Microsoft.AspNetCore.Identity;
using ResHub.Models;
using System.ComponentModel.DataAnnotations;

namespace ResHub.ModelViews
{
        public abstract class BaseAuth
        {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string UserName { get; set; }

        protected BaseAuth(string email, string firstName, string lastName, string userName)
        {
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            UserName = userName;
        }

        public async Task<bool> UserExists(UserManager<StudentResident> userManager)
        {
            return await userManager.FindByEmailAsync(Email) != null;
        }
    }
}
