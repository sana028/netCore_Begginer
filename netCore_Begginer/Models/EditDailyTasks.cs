namespace netCore_Begginer.Models
{
    public class EditDailyTasks
    {
        public required string Task_name { get; set; }

        public required string Assignee { get; set; }

        public required string Issue_type { get; set; }

        public required string Status { get; set; }

        public string? Description { get; set; }

    }
}
