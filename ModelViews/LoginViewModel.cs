using System.ComponentModel.DataAnnotations;

namespace ResHub.ModelViews
{
    public class LoginViewModel : BaseAuth
    {
        public LoginViewModel(string email, string password, bool rememberMe)
            : base(email, password)
        {
            Password = password;
            RememberMe = rememberMe;
        }
        public bool RememberMe { get; set; }
    }
}
