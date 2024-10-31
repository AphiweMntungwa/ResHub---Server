using ResHub.Data;
using ResHub.Services.Interfaces;
using ResHub.Models;
using Microsoft.EntityFrameworkCore;
using Mysqlx;
using ResHub.ModelViews;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using SendGrid.Helpers.Errors.Model;

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

            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedAccessException("User is not logged in.");
            }

            // Get the user's residenceId
            var user = await _context.Users
                .Include(u => u.Residence) // Assuming User has a Residence navigation property
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null || user.Residence == null)
            {
                throw new UnauthorizedAccessException("User does not have an associated residence.");
            }

            // Fetch the event details from the database using the eventId
            var eventDetails = await _context.Events
                .FirstOrDefaultAsync(e => e.Id == eventId);

            if (eventDetails == null)
            {
                throw new NotFoundException("Event not found.");
            }

            // Check if the event belongs to the user's residence
            if (eventDetails.ResidenceId != user.Residence.ResId)
            {
                throw new UnauthorizedAccessException("User does not have access to this event.");
            }

            if (eventDetails == null)
            {
                throw new Exception("Event not found");
            }

            return eventDetails;
        }

        public async Task<bool> UpdateEvent(int eventId, EventLoad eventUpdate)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedAccessException("User is not logged in.");
            }

            // Get the user's residenceId
            var user = await _context.Users
                .Include(u => u.Residence) // Assuming User has a Residence navigation property
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null || user.Residence == null)
            {
                throw new UnauthorizedAccessException("User does not have an associated residence.");
            }

            var existingEvent = await _context.Events.FindAsync(eventId);

            if (existingEvent == null)
            {
                return false;
            }

            if (existingEvent.ResidenceId != user.Residence.ResId)
            {
                throw new UnauthorizedAccessException("You are not authorized to Update this Event as you do not belong to this residence.");
            }

            existingEvent.EventName = eventUpdate.EventName;
            existingEvent.Description = eventUpdate.Description;
            existingEvent.Type = eventUpdate.Type;
            existingEvent.DateOfEvent = eventUpdate.DateOfEvent;

            _context.Events.Update(existingEvent);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteEvent(int eventId)
        {

            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedAccessException("User is not logged in.");
            }

            // Get the user's residenceId
            var user = await _context.Users
                .Include(u => u.Residence) // Assuming User has a Residence navigation property
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null || user.Residence == null)
            {
                throw new UnauthorizedAccessException("User does not have an associated residence.");
            }
            var eventToDelete = await _context.Events.FindAsync(eventId);

            if (eventToDelete == null)
            {
                return false;
            }

            if (eventToDelete.ResidenceId != user.Residence.ResId)
            {
                throw new UnauthorizedAccessException("You are not authorized to Delete this Event as you do not belong to this residence.");
            }

            _context.Events.Remove(eventToDelete);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
