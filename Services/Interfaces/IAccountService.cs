using ResHub.Models;
using ResHub.ModelViews;
using System.Security.Claims;

namespace ResHub.Services.Interfaces
{
    public interface IAccountService
    {
        Task<RegisterViewModel> CreateAccount(RegisterViewModel model);
        Task<LoginViewModel> Login(LoginViewModel model);
        Task<UserInfoDto> GetCurrentUserAsync(ClaimsPrincipal userPrincipal);
    }
}
