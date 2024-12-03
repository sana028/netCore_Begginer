using System.ComponentModel.DataAnnotations;

namespace netCore_Begginer.Models
{
    public class LoginResponse
    {
        [Required]
        public required string Token { get; set; }
    }
}
