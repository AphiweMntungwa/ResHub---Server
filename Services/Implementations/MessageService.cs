using Microsoft.AspNetCore.Identity;
using ResHub.Data;
using ResHub.Models;
using ResHub.Services.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace ResHub.Services.Implementations
{
    public class MessageService: IMessageService
    {
        private readonly ApplicationDbContext _context;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly UserManager<StudentResident> _userManager;
        private readonly SignInManager<StudentResident> _signInManager;
        private readonly IEmailService _emailService;
        private readonly IHttpContextAccessor _contextAccessor;

        public MessageService(ApplicationDbContext context, UserManager<StudentResident> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _contextAccessor = httpContextAccessor;
        }

        public async Task<List<Message>> MarkMessagesAsReadAsync(string otherUserId)
        {
            // Get the logged-in user
            var user = await _userManager.GetUserAsync(_contextAccessor?.HttpContext?.User);
            var currentUserId = user.Id;

            var messages = await _context.Messages
         .Where(m =>
             m.ReceiverId == currentUserId &&
             m.SenderId == otherUserId &&
             !m.IsRead // Only consider unread messages
         )
         .ToListAsync();

            if (messages != null && messages.Count > 0)
            {
                messages.ForEach(m => m.IsRead = true);
                await _context.SaveChangesAsync();
            }

            return messages;
        }
    }
}
