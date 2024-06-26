using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResHub.Data;
using ResHub.Models;
using ResHub.Services.Interfaces;

namespace ResHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : Controller
    {
        private readonly IEventService _eventService;

        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Events>>> GetAll()
        {
            try
            {
                var events = await _eventService.GetAllEvents();
                return Ok(events);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }


        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Events>>> Get()
        //{
        //    try
        //    {
        //        var events = await _context.Events
        //            .Include(r => r.EventResidences)
        //            .ThenInclude(er => er.Residence.Name)
        //            .ToListAsync();

        //        return Ok(events);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the error (you can use any logging framework or method)
        //        Console.WriteLine($"An error occurred: {ex.Message}");
        //        return StatusCode(500, "Internal server error");
        //    }
        //}

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
                var newEvent = new Events(Event.EventName, Event.Type, Event.DateOfEvent);
                var createdEvent = await _eventService.CreateEvent(newEvent, Event.ResId);

                return Ok(createdEvent);
            }
            catch (Exception ex)
            {
                // Log the error (you can use any logging framework or method)
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        //[HttpPost]
        //public async Task<IActionResult> Post([FromBody] EventLoad Event)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    // Create the event
        //    var newEvent = new Events(Event.EventName, Event.Type, Event.DateOfEvent);
        //    _context.Events.Add(newEvent);
        //    await _context.SaveChangesAsync();

        //    if (Event.ResId != null && Event.ResId.Any())
        //    {
        //        foreach (var resId in Event.ResId)
        //        {
        //            var eventResidence = new EventResidence
        //            {
        //                EventId = newEvent.Id,
        //                ResidenceId = resId
        //            };
        //            _context.EventResidents.Add(eventResidence);
        //        }
        //        await _context.SaveChangesAsync();
        //    }

        //    return Ok(newEvent); // Return the created residence
        //}

        public class EventLoad
        {
            public string EventName { get; set; }
            public Events.EventTypes Type { get; set; }
            public DateTime DateOfEvent { get; set; }
            public List<int>? ResId { get; set; }
        }
    }
}
