using ResHub.Models;
using ResHub.ModelViews;

namespace ResHub.Services.Interfaces
{
    public interface IEventService
    {
        Task<IEnumerable<Events>> GetAllEvents(int? resId);
        Task<Events> CreateEvent(EventLoad Event);
        Task<Events> GetEventDetailsAsync(int eventId);
        Task<bool> UpdateEvent(int eventId, EventLoad eventUpdate);
        Task<bool> DeleteEvent(int eventId);
    }
}
