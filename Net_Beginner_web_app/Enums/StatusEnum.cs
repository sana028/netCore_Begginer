using System.ComponentModel;

namespace Net_Beginner_web_app.Enums
{
    public enum StatusEnum
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
