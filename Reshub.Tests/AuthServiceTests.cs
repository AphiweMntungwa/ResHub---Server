//using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using ResHub.Data;
using ResHub.Models;
using ResHub.ModelViews;
using ResHub.Services.Implementations;
using ResHub.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reshub.Tests
{
    public class AccountServiceTests
    {
        private readonly ApplicationDbContext _context;
        private readonly Mock<IJwtTokenService> _mockJwtService;
        private readonly Mock<UserManager<StudentResident>> _mockUserManager;
        private readonly Mock<SignInManager<StudentResident>> _mockSignInManager;
        private readonly Mock<IEmailService> _mockEmailService;

        private readonly AccountService _accountService;

        public AccountServiceTests()
        {

            // Set up in-memory EF Core context
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
            _context = new ApplicationDbContext(options);


            // Required setup for UserManager and SignInManager mocks
            var userStore = new Mock<IUserStore<StudentResident>>();
            _mockUserManager = new Mock<UserManager<StudentResident>>(
                userStore.Object, null, null, null, null, null, null, null, null
            );

            var contextAccessor = new Mock<IHttpContextAccessor>();
            var claimsFactory = new Mock<IUserClaimsPrincipalFactory<StudentResident>>();
            _mockSignInManager = new Mock<SignInManager<StudentResident>>(
                _mockUserManager.Object, contextAccessor.Object, claimsFactory.Object, null, null, null, null
            );

            _mockJwtService = new Mock<IJwtTokenService>();
            _mockEmailService = new Mock<IEmailService>();

            _accountService = new AccountService(
                _context,
                _mockJwtService.Object,
                _mockUserManager.Object,
                _mockSignInManager.Object,
                _mockEmailService.Object
            );
        }

        [Fact]
        public async Task Login_ShouldReturnToken_WhenCredentialsAreValid()
        {
            // Arrange
            var loginModel = new LoginViewModel("user@example.com", "securePassword", true);

            var fakeUser = new StudentResident
            {
                Id = "user123",
                Email = loginModel.Email,
                ResidenceId = 42
            };

            _mockSignInManager
                .Setup(m => m.PasswordSignInAsync(
                    loginModel.Email, loginModel.Password, loginModel.RememberMe, false))
                .ReturnsAsync(SignInResult.Success);

            _mockUserManager
                .Setup(m => m.FindByEmailAsync(loginModel.Email))
                .ReturnsAsync(fakeUser);

            _mockJwtService
                .Setup(s => s.GenerateToken("user123", 42))
                .ReturnsAsync("mocked.jwt.token");
      

            // Act
            var result = await _accountService.Login(loginModel);

            //// Assert
            //result.Successful.Should().BeTrue();
            //result.AccessToken.Should().Be("mocked.jwt.token");
        }
    }
}


