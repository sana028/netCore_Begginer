using Net_Beginner_web_app.Interfaces;
using Net_Beginner_web_app.Models;

namespace Net_Beginner_web_app.Services
{
    public class StoreTaskDataService :ITaskDataServices
    {
        private List<DailyTasks> _tasks = new();

        public void setDailyTask(DailyTasks task)
        {
            _tasks.Add(task);
        }
        public void setAllDailyTasks(List<DailyTasks> tasks)
        {
            _tasks = tasks;
        }
        public List<DailyTasks> getDailyTasks()
        {
            return _tasks;
        }
    }
}
