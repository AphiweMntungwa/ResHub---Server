using ResHub.Data;
using ResHub.Models;
using ResHub.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using ResHub.ModelViews;

namespace ResHub.Services.Implementations
{
    public class BusService : IBusService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<StudentResident> _userManager; // Assuming your user class is named ApplicationUser
        private readonly IHttpContextAccessor _contextAccessor;

        public BusService(ApplicationDbContext context, UserManager<StudentResident> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _contextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<Bus>> GetBusesByResidenceIdAsync()
        {

            var userId = _contextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedAccessException("User is not logged in.");
            }

            // Get the user and check if they have a ResidenceId
            var user = await _context.Users.FindAsync(userId);
            if (user == null || user.ResidenceId == null)
            {
                throw new Exception("User or Residence ID is not available.");
            }

            // Retrieve the buses for the given residence ID
            var buses = await _context.Bus
                .Where(b => b.ResidenceId == user.ResidenceId.Value)
                 .Include(b => b.DepartureTimes)
                .ToListAsync();

            return buses;
        }

        public async Task AddBusAsync(BusDto busDto)
        {
            // Get the user ID from the HTTP context
            var userId = _contextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            using var transaction = await _context.Database.BeginTransactionAsync(); // Start a transaction

            // Get ResidenceId from UserId
            var user = await _context.Users
                .Include(u => u.Residence) // Assuming User has a Residence navigation property
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null || user.Residence == null)
            {
                throw new Exception("User or Residence not found.");
            }

            var residenceId = user.Residence.ResId;

            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedAccessException("User is not logged in.");
            }
            var bus = new Bus
            {
                BusNumber = busDto.BusNumber,
                BusDriver = busDto.BusDriver,
                BusDriverPhoneNumber = busDto.BusDriverPhoneNumber,
                ResidenceId = residenceId,
                LastUpdated = DateTime.UtcNow,
                LastUpdatedByUserId = userId
            };


            _context.Bus.Add(bus);
            await _context.SaveChangesAsync();

            // Create DepartureTimes for FromResidence
            foreach (var time in busDto.FromTimes)
            {
                var departureTime = new DepartureTime
                {
                    Time = DateTime.Parse(time).TimeOfDay,
                    Direction = DepartureTime.Directions.FromResidence,
                    BusId = bus.BusId
                };

                _context.DepartureTime.Add(departureTime);
            }

            // Create DepartureTimes for ToResidence
            foreach (var time in busDto.ToTimes)
            {
                var departureTime = new DepartureTime
                {
                    Time = DateTime.Parse(time).TimeOfDay,
                    Direction = DepartureTime.Directions.ToResidence,
                    BusId = bus.BusId

                };
                _context.DepartureTime.Add(departureTime);
            }

            // Save changes to DepartureTimes
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return;

        }
    }
}

