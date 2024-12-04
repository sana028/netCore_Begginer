using Net_Beginner_web_app.Models;

namespace Net_Beginner_web_app.Interfaces
{
    public interface IDataServices
    {
        void SetDailyTask(DailyTasks task);
        void SetAllDailyTasks(List<DailyTasks> tasks);
        List<DailyTasks> GetDailyTasks();
    }
}
