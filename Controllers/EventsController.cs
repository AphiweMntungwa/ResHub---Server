using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResHub.Data;
using ResHub.Models;
using ResHub.ModelViews;
using ResHub.Services.Interfaces;

namespace ResHub.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : Controller
    {
        private readonly IEventService _eventService;
        private readonly UserManager<StudentResident> _userManager;


        public EventsController(IEventService eventService, UserManager<StudentResident> userManager)
        {
            _eventService = eventService;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Events>>> GetAll()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var residenceId = user.ResidenceId;
          
            // Fetch events for the user's residence
            var events = await _eventService.GetAllEvents(residenceId);

            return Ok(events);

        }

        [HttpGet("{eventId}")]
        public async Task<IActionResult> GetEventDetails(int eventId)
        {
            try
            {
                var eventDetails = await _eventService.GetEventDetailsAsync(eventId);
                return Ok(eventDetails);
            }
            catch (Exception ex)
            {
                // Return a 404 status if the event is not found
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] EventLoad Event)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Create the event
                var createdEvent = await _eventService.CreateEvent(Event);

                return Ok(createdEvent);
            }
            catch (Exception ex)
            {
                // Log the error (you can use any logging framework or method)
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
