using ResHub.Data;
using ResHub.Services.Interfaces;
using ResHub.Models;
using Microsoft.EntityFrameworkCore;
using Mysqlx;
using ResHub.ModelViews;
using Microsoft.AspNetCore.Identity;

namespace ResHub.Services.Implementations
{
    public class EventService: IEventService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<StudentResident> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EventService(ApplicationDbContext context, UserManager<StudentResident> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IEnumerable<Events>> GetAllEvents(int? resId)
        {
           return await _context.Events
          .Where(e => e.ResidenceId == resId)
          .ToListAsync();
        }

        public async Task<Events> CreateEvent(EventLoad newEvent)
        {
            // Get the logged-in user
            var user = await _userManager.GetUserAsync(_httpContextAccessor?.HttpContext?.User);

            Events thisEvent = new Events
            (
                newEvent.EventName,
                newEvent.Type,
                newEvent.DateOfEvent,
                newEvent.Description
            )
            {
                ResidenceId = user.ResidenceId // Use the logged-in user's residence ID
            };
                        
            _context.Events.Add(thisEvent);
            await _context.SaveChangesAsync();
            return thisEvent;
        }

        public async Task<Events> GetEventDetailsAsync(int eventId)
        {
            // Fetch the event details from the database using the eventId
            var eventDetails = await _context.Events
                .FirstOrDefaultAsync(e => e.Id == eventId);

            if (eventDetails == null)
            {
                throw new Exception("Event not found");
            }

            return eventDetails;
        }
    }
}
