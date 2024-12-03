using Net_Beginner_web_app.Models;

namespace Net_Beginner_web_app.Interfaces
{
    public interface IDataServices
    {
        void setDailyTask(DailyTasks task);
        void setAllDailyTasks(List<DailyTasks> tasks);
        List<DailyTasks> getDailyTasks();
    }
}
