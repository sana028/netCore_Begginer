using System.ComponentModel.DataAnnotations;

namespace netCore_Begginer.Models
{
    public class UserAuthentication
    {
        public string? UserName { get; set; }

        [Key]
        [Required]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }

        public string? Role { get; set; }
    }

    public class Login
    {
        [Key]
        [Required]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }
    }

    public class LoginResponse
    {
        [Required]
        public  required string Token { get; set; }
    }

}
