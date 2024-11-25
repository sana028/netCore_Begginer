using System.ComponentModel.DataAnnotations;

namespace Net_Beginner_web_app.Models
{
    public class Login
    {
        [Key]
        [Required(ErrorMessage = "Email is Mandatory")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Password is Mandatory")] 
        public required string Password { get; set; }
    }
}
