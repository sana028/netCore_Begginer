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
        private readonly ProductDbContext _context;
        private readonly IGenerateJwtToken generateJwtToken;

     

        public UserValidationController(ProductDbContext context,IGenerateJwtToken generateJwt)
        {
            _context = context;
            generateJwtToken = generateJwt;
        }

        public GenerateJwtToken Object { get; }

        [HttpPost("login")]
        public  IActionResult Login([FromBody]Login user)
        {
            var list  = from obj in _context.UserAuthentication
                        where obj.Email == user.Email && obj.Password == user.Password
                        select obj;
            var validateUser = _context.UserAuthentication
                   ?.Where(userData => userData.Email == user.Email && userData.Password == user.Password).FirstOrDefault();
            if (validateUser == null)
            {
                return Unauthorized("Invalid email and password");
            }
            else
            {
                var token = generateJwtToken.GenerateToken(validateUser.Role, validateUser.Email);
                if (!string.IsNullOrEmpty(token))
                {

                    return Ok(new LoginResponse{ Token =  token });
                }
            }
            return BadRequest();
        }
    }
}
