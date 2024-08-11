using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI.Common;
using NuGet.Common;
using ResHub.Data;
using ResHub.Models;
using ResHub.ModelViews;
using ResHub.Services.Interfaces;



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
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IAccountService _accountService;

        public StudentResidentController(ApplicationDbContext context, UserManager<StudentResident> userManager, SignInManager<StudentResident> signInManager, IJwtTokenService jwtTokenService, IAccountService accountService)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtTokenService = jwtTokenService;
            _accountService = accountService;
        }

        // GET: api/<StudentResidentController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentResident>>> Get()
        {
            var residents = await _context.Users
                                        .Include(r => r.Residence)
                                        .ToListAsync();

            return Ok(residents);
        }

        // GET api/<StudentResidentController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentResident>> Get(string id)
        {
            var studentResident = await _context.Users
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

            var newUser = await _accountService.CreateAccount(model);

            if(newUser.Successful)
            {
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true, // Ensures the cookie is accessible only by the server
                    SameSite = SameSiteMode.None, // Allows cross-site requests
                    Secure = true, // Ensures the cookie is sent over HTTPS only
                    Expires = DateTime.Now.AddMinutes(30).ToUniversalTime() // Set expiration time for the cookie
                };

                // Append the cookie to the response
                Response.Cookies.Append("jwt-token", newUser.AccessToken, cookieOptions);

                return Ok(newUser);
            }

            if(newUser.Errors != null)
            {
                foreach (var error in newUser.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
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

            var result = await _accountService.Login(model);
            if(result.Successful)
            {
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true, // Ensures the cookie is accessible only by the server
                    SameSite = SameSiteMode.None, // Allows cross-site requests
                    Secure = true, // Ensures the cookie is sent over HTTPS only
                    Expires = DateTime.Now.AddMinutes(30).ToUniversalTime() // Set expiration time for the cookie
                };

                // Append the cookie to the response
                Response.Cookies.Append("jwt-token", result.AccessToken, cookieOptions);

                return Ok(result);
            }

            if (result.SignInResult.IsLockedOut)
            {
                return Unauthorized("Your Account has been locked out");
            }
            if (result.SignInResult.IsNotAllowed)
            {
                return Unauthorized("Verify Your Email");
            }
            if (!result.SignInResult.Succeeded)
            {
                return Unauthorized("Wrong Email or Password");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return BadRequest(ModelState);
        }

        [HttpPost("verify-email")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return BadRequest("Invalid email.");
            }

            if (user.ConfirmationCode == model.Code && user.ConfirmationCodeExpiration > DateTime.UtcNow)
            {
                user.EmailConfirmed = true;
                user.ConfirmationCode = null; // Clear the code after successful verification
                user.ConfirmationCodeExpiration = null;

                var updateResult = await _userManager.UpdateAsync(user);

                if (updateResult.Succeeded)
                {
                    return Ok("Email successfully verified.");
                }

                return BadRequest("Failed to update user.");
            }

            return BadRequest("Invalid or expired verification code.");
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
            var userDto = await _accountService.GetCurrentUserAsync(User);

            if (userDto == null)
            {
                return NotFound();
            }

            return Ok(userDto);
        }

        private bool StudentResidentExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
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
