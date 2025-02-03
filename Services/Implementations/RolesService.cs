using Microsoft.AspNetCore.Identity;
using ResHub.Data;
using ResHub.Models;
using ResHub.ModelViews;
using ResHub.Services.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace ResHub.Services.Implementations
{
    public class RolesService : IRolesService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<StudentResident> _userManager;

        public RolesService(ApplicationDbContext context, UserManager<StudentResident> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<StudentResidentViewModel> GetAdminAsync(int residenceId)
        {
            var admin = await _userManager.Users.FirstOrDefaultAsync(u => u.ResidenceId == residenceId && _context.UserRoles.Any(r => r.UserId == u.Id && r.RoleId == "Admin"));
            return admin != null ? new StudentResidentViewModel(admin) : null;
        }

        public async Task<PollResponseViewModel> OpenPollAsync(string userId, PollRequestViewModel request)
        {
            var usersCount = await _userManager.Users.CountAsync(u => u.ResidenceId == request.ResidenceId);
            if (usersCount < 50)
                return new PollResponseViewModel { Successful = false, Message = "Not enough users to open a poll." };

            var poll = new Poll
            {
                CreatedByUserId = userId,
                ResidenceId = request.ResidenceId,
                Role = request.Role,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(3)
            };

            _context.Polls.Add(poll);
            await _context.SaveChangesAsync();
            return new PollResponseViewModel { Successful = true, Id = poll.Id };
        }

        public async Task<VoteResponseViewModel> CastVoteAsync(string userId, VoteRequestViewModel request)
        {
            var poll = await _context.Polls.Include(p => p.Votes).FirstOrDefaultAsync(p => p.Id == request.PollId && p.IsOpen);
            if (poll == null || poll.ExpiresAt < DateTime.UtcNow)
                return new VoteResponseViewModel { Successful = false, Message = "Poll is not active." };

            if (poll.Votes.Any(v => v.VotingUserId == userId))
                return new VoteResponseViewModel { Successful = false, Message = "User has already voted." };

            var vote = new Vote { PollId = request.PollId, VotingUserId = userId, VotedForUserId = request.VotedForUserId, VotedAt = DateTime.UtcNow };
            _context.Votes.Add(vote);
            await _context.SaveChangesAsync();

            return new VoteResponseViewModel { Successful = true };
        }
    }

}
