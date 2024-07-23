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
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string? AccessToken { get; set; }

        protected BaseAuth(string email, string password)
        {
            Email = email;
            Password = password;
        }

        public async Task<bool> UserExists(UserManager<StudentResident> userManager)
        {
            return await userManager.FindByEmailAsync(Email) != null;
        }
    }
}
