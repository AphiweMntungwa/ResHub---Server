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

namespace ResHub.Services.Implementations
{
    public class AccountService: IAccountService
    {

        private readonly ApplicationDbContext _context;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly UserManager<StudentResident> _userManager;
        private readonly SignInManager<StudentResident> _signInManager;

        public AccountService(ApplicationDbContext context, IJwtTokenService jwtTokenService, UserManager<StudentResident> userManager, SignInManager<StudentResident> signInManager)
        {
            _context = context;
            _jwtTokenService = jwtTokenService;
            _userManager = userManager;
            _signInManager = signInManager;
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
                user.ConfirmationCodeExpiration = DateTime.UtcNow.AddHours(1); // Code valid for 1 hour

                // Update user with confirmation code
                await _userManager.UpdateAsync(user);

                // Send email with confirmation code
                //await SendConfirmationEmail(user.Email, confirmationCode); // Implement this method to send email

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
        private string GenerateConfirmationCode()
        {
            // Implement your code generation logic here
            return new Random().Next(10000, 99999).ToString(); // Example: 5-digit code
        }

        private async Task SendConfirmationEmail(string email, string confirmationCode)
        {
            // Implement your email sending logic here
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Reshub", "bongumusaayeza@gmail.com"));
            message.To.Add(new MailboxAddress("", email));
            message.Subject = "Email Confirmation";
            message.Body = new TextPart("plain")
            {
                Text = $"Your confirmation code is {confirmationCode}"
            };

            using (var client = new SmtpClient())
            {
                // Replace the placeholders with your actual SMTP server configuration
                var smtpServer = "smtp.gmail.com";
                var smtpPort = 587;
                var smtpUsername = "bongumusaayeza@gmail.com";
                var smtpPassword = "Bongumusa2004";

                client.Connect(smtpServer, smtpPort, false);
                client.Authenticate(smtpUsername, smtpPassword);
                await client.SendAsync(message);
                client.Disconnect(true);
            }
        }
    }
}
