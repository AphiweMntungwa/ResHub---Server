using Microsoft.AspNetCore.Identity;
using MimeKit;
using System;
using MailKit.Net.Smtp;
using MailKit.Security;
using ResHub.Data;
using ResHub.Models;
using ResHub.Services.Interfaces;
using ResHub.ModelViews;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace ResHub.Services.Implementations
{
    public class AccountService: IAccountService
    {
        private readonly ApplicationDbContext _context;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly UserManager<StudentResident> _userManager;
        private readonly SignInManager<StudentResident> _signInManager;
        private readonly IEmailService _emailService;

        public AccountService(ApplicationDbContext context, IJwtTokenService jwtTokenService, UserManager<StudentResident> userManager, SignInManager<StudentResident> signInManager, IEmailService emailService)
        {
            _context = context;
            _jwtTokenService = jwtTokenService;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        public async Task<RegisterViewModel> CreateAccount(RegisterViewModel model)
        {
            var user = new StudentResident(model.StudentNumber, model.FirstName, model.LastName, model.Email, model.UserName, model.ResidenceId, model.RoomNumber);

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Generate confirmation code
                var confirmationCode = GenerateConfirmationCode(); // Implement this method to generate a code
                user.ConfirmationCode = confirmationCode;
                user.ConfirmationCodeExpiration = DateTime.UtcNow.AddHours(3); // Code valid for 1 hour

                // Update user with confirmation code
                await _userManager.UpdateAsync(user);

                // Send email with confirmation code
                //await SendConfirmationEmail(user.Email, confirmationCode);

                await _signInManager.SignInAsync(user, isPersistent: false);
                // Generate the JWT token
                var token = _jwtTokenService.GenerateToken(user.Id);
                model.AccessToken = token;
                model.Successful = true;

                // Return the token along with the user data
                return model;
            }
            model.Successful = false;
            model.Errors = result.Errors;
            return model;
        }


        public async Task<LoginViewModel> Login(LoginViewModel model)
        {

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                // Generate the JWT token
                var user = await _userManager.FindByEmailAsync(model.Email);
                var token = _jwtTokenService.GenerateToken(user.Id); // Assuming you have a method like GenerateToken in your JwtTokenService that takes userId
                model.AccessToken = token;
                model.Successful = true;
                return model;
            }

            model.Successful = false;
            model.SignInResult = result;
            return model;
        }

        public async Task<UserInfoDto> GetCurrentUserAsync(ClaimsPrincipal userPrincipal)
        {
            var userId = _userManager.GetUserId(userPrincipal);

            var user = await _userManager.Users
                .Include(u => u.Residence) // Assuming Residence is the navigation property
                .Where(u => u.Id == userId)
                .Select(u => new UserInfoDto
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    FullName = u.FirstName + " " + u.LastName,
                    Email = u.Email,
                    ResidenceId = u.ResidenceId,
                    ResidenceName = u.Residence.Name // Assuming Residence has a Name property
                })
                .FirstOrDefaultAsync();

            return user;
        }








        //HELPER METHODS
        private string GenerateConfirmationCode()
        {
            // Implement your code generation logic here
            return new Random().Next(10000, 99999).ToString(); // Example: 5-digit code
        }

        private async Task SendConfirmationEmail(string email, string confirmationCode)
        {
            // Use your EmailService to send the confirmation email
            var subject = "Email Confirmation for the ResHub System";
            var body = $"Your confirmation code is {confirmationCode}";

            await _emailService.SendEmailAsync(email, subject, body);
        }
    }
}
