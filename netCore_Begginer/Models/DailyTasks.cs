using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace netCore_Begginer.Models
{
    public class DailyTasks : EditDailyTasks
    {
   
        [Key]
        [Required(ErrorMessage = "Task id is manadatory")]
        public required string Task_id { get; set; }

        [Required]
        public string? Email { get; set; }

    }
}
