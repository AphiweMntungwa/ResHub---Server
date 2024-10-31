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

        public async Task<bool> DeleteBusAsync(int busId)
        {
            var userId = _contextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

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

            // Find the bus by its ID
            var bus = await _context.Bus.FirstOrDefaultAsync(b => b.BusId == busId);

            if (bus == null)
            {
                throw new Exception("Bus not found.");
            }

            // Check if the bus belongs to the user's residence
            if (bus.ResidenceId != user.Residence.ResId)
            {
                throw new UnauthorizedAccessException("You are not authorized to delete this bus as you do not belong to this residence.");
            }

            // Remove the bus
            _context.Bus.Remove(bus);

            // Save changes to the database
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateBusAsync(int busId, BusUpdateDto busUpdateDto)
        {
            // Retrieve the current user and their residence ID
            var userId = _contextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            using var transaction = await _context.Database.BeginTransactionAsync(); 
            
            var userResidenceId = await _context.Users
                .Where(u => u.Id == userId)
                .Select(u => u.ResidenceId)
                .FirstOrDefaultAsync();

            // Retrieve the bus to be updated
            var bus = await _context.Bus
                .Include(b => b.DepartureTimes)
                .FirstOrDefaultAsync(b => b.BusId == busId);

            if (bus == null)
            {
                return false; // Bus not found
            }

            if (bus.ResidenceId != userResidenceId)
            {
                return false; // User does not belong to the same residence
            }

            // Update bus details
            bus.BusNumber = busUpdateDto.BusNumber;
            bus.BusDriver = busUpdateDto.BusDriver;
            bus.BusDriverPhoneNumber = busUpdateDto.BusDriverPhoneNumber;

            // Add updated departure times
            foreach (var time in busUpdateDto.FromTimes)
            {
                _context.DepartureTime.Add(new DepartureTime
                {
                    Time = DateTime.Parse(time).TimeOfDay,
                    Direction = DepartureTime.Directions.FromResidence,
                    BusId = busId
                });
            }

            foreach (var time in busUpdateDto.ToTimes)
            {
                _context.DepartureTime.Add(new DepartureTime
                {
                    Time = DateTime.Parse(time).TimeOfDay,
                    Direction = DepartureTime.Directions.ToResidence,
                    BusId = busId
                });
            }


            _context.Bus.Update(bus);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
            return true;
        }
    }
}

