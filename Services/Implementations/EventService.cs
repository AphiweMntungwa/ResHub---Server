using ResHub.Data;
using ResHub.Services.Interfaces;
using ResHub.Models;
using Microsoft.EntityFrameworkCore;
using Mysqlx;

namespace ResHub.Services.Implementations
{
    public class EventService: IEventService
    {
        private readonly ApplicationDbContext _context;

        public EventService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Events>> GetAllEvents()
        {
            return await _context.Events
                .Include(r => r.EventResidences)
                .ToListAsync();
        }

        public async Task<Events> CreateEvent(Events newEvent, List<int> Ids)
        {
            _context.Events.Add(newEvent);

            if (Ids != null && Ids.Any())
            {
                foreach (var resId in Ids)
                {
                    var eventResidence = new EventResidence
                    {
                        EventId = newEvent.Id,
                        ResidenceId = resId
                    };
                    _context.EventResidents.Add(eventResidence);
                }
            }

            await _context.SaveChangesAsync();
            return newEvent;
        }
    }
}
