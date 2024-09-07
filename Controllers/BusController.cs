using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ResHub.Models;
using ResHub.ModelViews;
using ResHub.Services.Interfaces;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ResHub.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BusController : ControllerBase
    {
        private readonly IBusService _busService;

        public BusController(IBusService busService)
        {
            _busService = busService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBuses()
        {
            try
            {
                var buses = await _busService.GetBusesByResidenceIdAsync();
                return Ok(buses);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("User is not logged in.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/bus
        [HttpPost]
        public async Task<IActionResult> AddBus([FromBody] BusDto busDto)
        {
            if (busDto == null)
            {
                return BadRequest("Bus is null.");
            }

            try
            {
                await _busService.AddBusAsync(busDto);
                return Ok();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("User is not logged in or authorized.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
