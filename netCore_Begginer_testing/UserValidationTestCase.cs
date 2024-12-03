using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.EntityFrameworkCore;
using netCore_Begginer.Controllers;
using netCore_Begginer.Interfaces;
using netCore_Begginer.Models;
using netCore_Begginer.Services;

namespace netCore_Begginer_testing
{
    public class UserValidationTestCase
    {
        private readonly Mock<IGenerateJwtToken> MockGenerateJwtToken;
        private readonly Mock<ProductDbContext> MockProductDbContext;

        public UserValidationTestCase()
        {
            MockGenerateJwtToken = new Mock<IGenerateJwtToken>();
            MockProductDbContext = new Mock<ProductDbContext>();
        }

        [Fact]
        public void Login_ReturnsOk_WithValidCredentials()
        {
            var testUser = new UserAuthentication
            {
                Email = "valid@test.com",
                Password = "correctpassword",
                Role = "admin"
            };

            MockGenerateJwtToken.Setup(s => s.GenerateToken("admin", "valid@test.com")).Returns("auth-token");

            MockProductDbContext.Setup(c => c.UserAuthentication)
                   .ReturnsDbSet(new List<UserAuthentication>{ testUser});


            var userValidationController = new UserValidationController(MockProductDbContext.Object, MockGenerateJwtToken.Object);

            var userInfo = new Login()
            {
                Email = "valid@test.com",
                Password = "correctpassword"
            };
            var result = userValidationController.Login(userInfo);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<LoginResponse>(okResult.Value);
            Assert.Equal("auth-token", response?.Token);

        }

        [Fact]
        public void Login_ReturnsUnauthorized_WhenUserIsInvalid()
        {
            MockProductDbContext.Setup(c => c.UserAuthentication)
                   .ReturnsDbSet(new List<UserAuthentication>());

            MockGenerateJwtToken.Setup(s => s.GenerateToken("admin", "valid@test.com"))
                .Returns("auth-token");

            var userValidationController = new UserValidationController(MockProductDbContext.Object, MockGenerateJwtToken.Object);

            var userInfo = new Login()
            {
                Email = "valid@test.com",
                Password = "correctpassword"
            };
            var result = userValidationController.Login(userInfo);

            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Invalid email and password", unauthorizedResult.Value);
        }

        [Fact]
        public void Login_ReturnsBadRequest_WhenTokenGenerationFails()
        {

            var testUser = new UserAuthentication
            {
                Email = "valid@test.com",
                Password = "correctpassword",
                Role = "admin"
            };
            MockProductDbContext.Setup(c => c.UserAuthentication)
                   .ReturnsDbSet(new List<UserAuthentication>{ testUser});

            MockGenerateJwtToken.Setup(s => s.GenerateToken("admin", "valid@test.com"))
                .Returns(string.Empty);

            var userValidationController = new UserValidationController(MockProductDbContext.Object, MockGenerateJwtToken.Object);

            var userInfo = new Login()
            {
                Email = "valid@test.com",
                Password = "correctpassword"
            };
            var result = userValidationController.Login(userInfo);

            Assert.IsType<BadRequestResult>(result);
        }
    }
}