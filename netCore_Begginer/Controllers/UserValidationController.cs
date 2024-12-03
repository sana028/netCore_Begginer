using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using netCore_Begginer.Interfaces;
using netCore_Begginer.Models;
using netCore_Begginer.Services;

namespace netCore_Begginer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserValidationController : ControllerBase
    {
        private readonly ProductDbContext ProductDbContext;
        private readonly IGenerateJwtToken GenerateJwtToken;

        public UserValidationController(ProductDbContext context,IGenerateJwtToken generateJwt)
        {
            ProductDbContext = context;
            GenerateJwtToken = generateJwt;
        }

        [HttpPost("login")]
        public  IActionResult Login([FromBody]Login user)
        {
            var validateUser = ProductDbContext.UserAuthentication
                   ?.Where(userData => userData.Email == user.Email && userData.Password == user.Password).FirstOrDefault();
            if (validateUser == null)
            {
                return Unauthorized("Invalid email and password");
            }
            else
            {
                var token = GenerateJwtToken.GenerateToken(validateUser.Role, validateUser.Email);
                if (!string.IsNullOrEmpty(token))
                {
                    return Ok(new LoginResponse{ Token =  token });
                }
            }
            return BadRequest();
        }
    }
}
