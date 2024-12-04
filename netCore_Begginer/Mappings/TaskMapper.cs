using Microsoft.AspNetCore.Mvc;
using netCore_Begginer.Models;

namespace netCore_Begginer.Mappings
{
    public class TaskMapper
    {
        public DailyTasks MapTasks(EditDailyTasks dailyTasks,string id)
        {
            return new DailyTasks
            {
                Task_id = id,
                Task_name = dailyTasks.Task_name,
                Issue_type = dailyTasks.Issue_type,
                Status = dailyTasks.Status,
                Assignee = dailyTasks.Assignee,
                Description = dailyTasks.Description
            };
        }
    }
}
