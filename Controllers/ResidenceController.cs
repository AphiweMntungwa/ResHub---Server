using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResHub.Data;
using ResHub.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ResHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResidenceController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ResidenceController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Residence
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Residence>>> Get()
        {
            var residences = await _context.Residence
                .ToListAsync();
            return Ok(residences);
        }

        // GET api/<ResidenceController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentResident>> Get(int id)
        {
            var studentResident = await _context.Residence
                                               .Include(r => r.StudentResidents)
                                               .FirstOrDefaultAsync(r => r.ResId == id);

            if (studentResident == null)
            {
                return NotFound();
            }

            return Ok(studentResident);
        }

        // POST: api/Residence
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Residence residence)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Residence.Add(residence);
            await _context.SaveChangesAsync();

            return Ok(residence); // Return the created residence
        }

        // PUT api/<ResidenceController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ResidenceController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
