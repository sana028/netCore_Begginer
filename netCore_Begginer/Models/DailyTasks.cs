using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace netCore_Begginer.Models
{
    public class DailyTasks
    {
        [Required (ErrorMessage = "Task Name is required")]
        public required string Task_name {  get; set; }

        [Key]
        [Required (ErrorMessage = "Task id is manadatory")]
        public required string Task_id { get; set; }

        public required string Assignee { get; set; }

        [Required]
        public  string Email { get; set; }

        public required string Issue_type { get; set; }

        public required string Status {  get; set; }

        public string? Description { get; set; }

        public string? File_name { get; set; }

        public byte[]? Attachments { get; set; }
    }

    public class EditDailyTasks
    {
        public required string Task_name { get; set; }

        public required string Assignee { get; set; }

        public required string Issue_type { get; set; }

        public required string Status { get; set; }

        public string? Description { get; set; }

        public string? File_name { get; set; }

        public byte[]? Attachments { get; set; }
    }
}
