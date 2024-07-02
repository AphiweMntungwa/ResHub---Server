using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResHub.Data;
using ResHub.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ResHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentResidentController : ControllerBase
    {

        private readonly ApplicationDbContext _context;

        public StudentResidentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/<StudentResidentController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentResident>>> Get()
        {
            var residents = await _context.Residents
                                        .Include(r => r.Residence)
                                        .ToListAsync();

            return Ok(residents);
        }

        // GET api/<StudentResidentController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentResident>> Get(string id)
        {
            var studentResident = await _context.Residents
                                                .Include(r => r.Residence)
                                                .FirstOrDefaultAsync(r => r.Id == id);

            if (studentResident == null)
            {
                return NotFound();
            }

            return Ok(studentResident);
        }

        // POST api/<StudentResidentController>
        [HttpPost]
         public async Task<IActionResult> Post([FromBody] StudentResident studentResident)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Adding the studentResident to the database context
            _context.Residents.Add(studentResident);
            await _context.SaveChangesAsync();

            return Ok(studentResident); // Return the created residence
        }


        // PUT api/<StudentResidentController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<StudentResidentController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
