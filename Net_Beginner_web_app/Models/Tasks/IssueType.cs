using Net_Beginner_web_app.Enums;

namespace netCore_Begginer.Models.Tasks
{
    public class IssueType
    {
        public required IssueTypeEnum IssueTypes {  get; set; }

        public string? IconName { get; set; }

    }
}
