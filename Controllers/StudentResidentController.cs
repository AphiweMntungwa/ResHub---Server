using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResHub.Data;
using ResHub.Models;
using ResHub.ModelViews;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ResHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentResidentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<StudentResident> _userManager;
        private readonly SignInManager<StudentResident> _signInManager;

        public StudentResidentController(ApplicationDbContext context, UserManager<StudentResident> userManager, SignInManager<StudentResident> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
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

       // POST: api/StudentResidentController/Register
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new StudentResident(model.StudentNumber, model.FirstName, model.LastName, model.Email, model.ResidenceId, model.RoomNumber);

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return Ok(user);
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return BadRequest(ModelState);
        }

        // POST: api/StudentResidentController/Login
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return Ok();
            }

            if (result.IsLockedOut)
            {
                return Forbid();
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return BadRequest(ModelState);
        }

        // POST: api/StudentResidentController/Logout
        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        // GET: api/StudentResidentController/Me
        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        private bool StudentResidentExists(string id)
        {
            return _context.Residents.Any(e => e.Id == id);
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
