using System.ComponentModel.DataAnnotations;

namespace Net_Beginner_web_app.Models
{
   
        public class EditDailyTasks
        {
            [Required(ErrorMessage = "Task Name is required")]
            public required string Task_name { get; set; }

            [Key]
            [Required(ErrorMessage = "Task id is manadatory")]
            public required string Task_id { get; set; }

            public required string Assignee { get; set; }

            public required string Issue_type { get; set; }

            public required string Status { get; set; }

            public string? Description { get; set; }

        }
    
}
