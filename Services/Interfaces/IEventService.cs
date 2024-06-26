using ResHub.Models;

namespace ResHub.Services.Interfaces
{
    public interface IEventService
    {
        Task<IEnumerable<Events>> GetAllEvents();
        Task<Events> CreateEvent(Events newEvent, List<int> Ids);
    }
}
