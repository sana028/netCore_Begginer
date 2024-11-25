using System.ComponentModel;

namespace netCore_Begginer.Models.Tasks
{
    public enum Status
    {
        //Use an attribute like [Description] from System.ComponentModel to store a display-friendly name
        [Description("To-Do")]
        ToDo,
        [Description("In-Progress")]
        InProgress,
        Pause,
        Block,
        Done
    }
}
