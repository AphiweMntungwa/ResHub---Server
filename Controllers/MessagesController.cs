using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResHub.Data;
using ResHub.Models;
using ResHub.ModelViews;
using ResHub.Services.Interfaces;
using SendGrid.Helpers.Mail;

namespace ResHub.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : Controller
    {
        private readonly UserManager<StudentResident> _userManager;
        private readonly IMessageService _messageService;

        public MessagesController( UserManager<StudentResident> userManager, IMessageService messageService)
        {
            _userManager = userManager;
            _messageService = messageService;
        }

        [HttpGet("mark-as-read/{otherUserId}")]
        public async Task<IActionResult> MarkMessagesAsRead(string otherUserId)
        {
            // Extract the user ID from the JWT token

            if (string.IsNullOrEmpty(otherUserId))
            {
                return BadRequest("User IDs are required");
            }

            var messages = await _messageService.MarkMessagesAsReadAsync(otherUserId);

            if (messages == null || messages.Count == 0)
            {
                return NotFound("No messages found between these users");
            }

            return Ok("Messages marked as read");
        }
    }
}
