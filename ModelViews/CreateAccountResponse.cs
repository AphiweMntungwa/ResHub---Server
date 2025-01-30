using Microsoft.AspNetCore.Identity;
using ResHub.Models;

namespace ResHub.ModelViews
{
    public class CreateAccountResponse
    {
        public bool Successful { get; set; }
        public StudentResident User { get; set; }
        public string AccessToken { get; set; }
        public IEnumerable<IdentityError> Errors { get; set; }
    }
}
