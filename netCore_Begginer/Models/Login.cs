using System.ComponentModel.DataAnnotations;

namespace netCore_Begginer.Models
{
    public class Login
    {
        [Key]
        [Required]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }
    }
}
