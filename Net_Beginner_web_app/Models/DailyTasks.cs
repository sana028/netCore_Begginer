using netCore_Begginer.Models.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Net_Beginner_web_app.Models
{
    public class DailyTasks : EditDailyTasks
    {
        [Required]
        public required string Email { get; set; }
    }

}
