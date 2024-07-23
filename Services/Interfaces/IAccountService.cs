using ResHub.Models;
using ResHub.ModelViews;

namespace ResHub.Services.Interfaces
{
    public interface IAccountService
    {
        Task<RegisterViewModel> CreateAccount(RegisterViewModel model);
        Task<LoginViewModel> Login(LoginViewModel model);
    }
}
