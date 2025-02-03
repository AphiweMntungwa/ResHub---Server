using ResHub.ModelViews;

namespace ResHub.Services.Interfaces
{
    public interface IRolesService
    {
        Task<StudentResidentViewModel> GetAdminAsync(int residenceId);
        Task<PollResponseViewModel> OpenPollAsync(string userId, PollRequestViewModel request);
        Task<VoteResponseViewModel> CastVoteAsync(string userId, VoteRequestViewModel request);
    }
}
