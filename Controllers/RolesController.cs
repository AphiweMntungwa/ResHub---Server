using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResHub.Services.Interfaces;
using ResHub.ModelViews;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ResHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RolesController : ControllerBase
    {
        private readonly IRolesService _rolesService;

        public RolesController(IRolesService rolesService)
        {
            _rolesService = rolesService;
        }

        // Get admin info
        [HttpGet("admin/{residenceId}")]
        public async Task<IActionResult> GetAdmin(int residenceId)
        {
            var admin = await _rolesService.GetAdminAsync(residenceId);
            return admin != null ? Ok(admin) : NotFound("No admin found.");
        }

        // Open a voting poll
        [HttpPost("polls/open")]
        public async Task<IActionResult> OpenPoll([FromBody] PollRequestViewModel request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _rolesService.OpenPollAsync(userId, request);
            return result.Successful ? Ok(result) : BadRequest(result);
        }

        // Vote in an open poll
        [HttpPost("polls/vote")]
        public async Task<IActionResult> Vote([FromBody] VoteRequestViewModel request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _rolesService.CastVoteAsync(userId, request);
            return result.Successful ? Ok(result) : BadRequest(result);
        }
    }
}