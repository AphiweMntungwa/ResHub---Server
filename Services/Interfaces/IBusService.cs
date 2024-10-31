using ResHub.Models;
using ResHub.ModelViews;

namespace ResHub.Services.Interfaces
{
    public interface IBusService
    {
        Task<IEnumerable<Bus>> GetBusesByResidenceIdAsync();
        Task AddBusAsync(BusDto busDto);
        Task<bool> UpdateBusAsync(int busId, BusUpdateDto busUpdateDto);
        Task<bool> DeleteBusAsync(int busId);
    }

}
