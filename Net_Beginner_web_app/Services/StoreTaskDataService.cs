using Net_Beginner_web_app.Interfaces;
using Net_Beginner_web_app.Models;

namespace Net_Beginner_web_app.Services
{
    public class StoreTaskDataService :IDataServices
    {
        private List<DailyTasks> Tasks = new();

        public void setDailyTask(DailyTasks task)
        {
            Tasks.Add(task);
        }
        public void setAllDailyTasks(List<DailyTasks> tasks)
        {
            Tasks = tasks;
        }
        public List<DailyTasks> getDailyTasks()
        {
            return Tasks;
        }
    }
}
