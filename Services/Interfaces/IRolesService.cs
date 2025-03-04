using ResHub.ModelViews;
using System.Collections.Generic;
using System.Security.Claims;

namespace ResHub.Services.Interfaces
{
    public interface IRolesService
    {
        Task<List<StudentResidentViewModel>> GetAdminsAsync();
        Task<PollResponseViewModel> OpenPollAsync(string userId, PollRequestViewModel request);
        Task<VoteResponseViewModel> CastVoteAsync(string userId, VoteRequestViewModel request);
    }
}
