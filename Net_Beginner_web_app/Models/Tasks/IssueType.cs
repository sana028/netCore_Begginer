namespace netCore_Begginer.Models.Tasks
{
    public enum IssueTypes
    {
        Bug,
        Feature,
        Task,
        Story,
        Epic
    }

    public class IssueType
    {
        public required IssueTypes IssueTypes {  get; set; }

        public string? IconName { get; set; }

    }
}
