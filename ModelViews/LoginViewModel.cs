using System.ComponentModel.DataAnnotations;

namespace ResHub.ModelViews
{
    public class LoginViewModel : BaseAuth
    {
        public LoginViewModel(string email, string password, bool rememberMe)
            : base(email, string.Empty, string.Empty, email)
        {
            Password = password;
            RememberMe = rememberMe;
        }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
