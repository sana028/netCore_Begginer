using Microsoft.AspNetCore.Mvc;
using Net_Beginner_web_app.Interfaces;
using Net_Beginner_web_app.Models;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Net_Beginner_web_app.Delegate_Events
{
    public class TaskApiActions
    {
        private readonly HttpClient HttpClient;
        private static string UserEmail = null;
        private readonly  IDataServices TaskServices;
        private readonly ILogger<TaskApiActions> Logger;

        public TaskApiActions(IHttpClientFactory httpFactory,ISessionStore data, IDataServices taskServices, ILogger<TaskApiActions> logger)
        {
            HttpClient = httpFactory.CreateClient("ApiClient");
            UserEmail = data.GetTheUserDataFromSession();
            TaskServices = taskServices;
            Logger = logger;
        }

        public async Task<DailyTasks> GetAsync(string url,string id)
        {
            if(!string.IsNullOrEmpty(id)) { 
                var response = await HttpClient.GetAsync(url+$"{id}");
                if(response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                    var tasks = JsonSerializer.Deserialize<DailyTasks>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true // Allows case-insensitive property matching
                    });
                    Logger.LogInformation($"Fetched the Task details for the task-id {id}");

                    return tasks;
                }
                else
                {
                    Logger.LogError($"Got some error from the api {response}");
                    return null; 
                }
            }
            return null;
        }

        public async Task<List<DailyTasks>> PostAsync(string url,DailyTasks tasks)
        {
            if(!string.IsNullOrEmpty(url)&& tasks!=null) {
                var response = await HttpClient.PostAsJsonAsync(url,tasks);
                if(response.IsSuccessStatusCode)
                {
                    Logger.LogInformation($"Added data successfully to table {response.StatusCode}");
                    TaskServices.SetDailyTask(tasks);
                }
                else
                {
                    Logger.LogError($"{response.StatusCode}");
                }
            }
            return (new List<DailyTasks>()); ;
        }

        public async Task<List<DailyTasks>>PutAsync(string url,EditDailyTasks tasks)
        {
            if(!string.IsNullOrEmpty(url) && tasks != null)
            {
                var response = await HttpClient.PutAsJsonAsync(url+$"{tasks.Task_id}", tasks);
                if(response.IsSuccessStatusCode)
                {
                    Logger.LogInformation($"Updated data to the table for the task-id {tasks.Task_id},with status {response.StatusCode}");
                    var allTasks =  TaskServices.GetDailyTasks();
                    var list = new DailyTasks
                    {
                        Task_id = tasks.Task_id,
                        Task_name = tasks.Task_name,
                        Assignee = tasks.Assignee,
                        Issue_type = tasks.Issue_type,
                        Status = tasks.Status,
                        Email = UserEmail,
                        Description = tasks.Description,
                    };
                    var taskIndex = allTasks.FindIndex(t => t.Task_id == tasks.Task_id);
                    allTasks[taskIndex] = list;
                   TaskServices.SetAllDailyTasks(allTasks);
                }
                else
                {
                    Logger.LogError($"{response}");
                }
            }
            return (new List<DailyTasks>());
        }

        public async Task<List<DailyTasks>> DeleteAsync(string url,string id)
        {
            if (!string.IsNullOrEmpty(url) && !string.IsNullOrEmpty(id))
            {
                var response = await HttpClient.DeleteAsync(url + id);
                if (response.IsSuccessStatusCode)
                {
                    Logger.LogInformation($"Successfully deleted the task with the id {id},{response.StatusCode}");
                    var allTasks = TaskServices.GetDailyTasks();
                    var taskIndex = allTasks.FindIndex(t => t.Task_id == id);
                    allTasks.RemoveAt(taskIndex);
                   TaskServices.SetAllDailyTasks(allTasks);
                }
                else
                {
                    Logger.LogError($"{response}");
                }
            }
            return (new List<DailyTasks>()); ;
        }

        public async Task<List<DailyTasks>> GetallData(string url)
        {
            if(!string.IsNullOrEmpty(url))
            {
                var response = await HttpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    Logger.LogTrace($"Fetched all tasks of this user email {UserEmail}");
                    var json = await response.Content.ReadAsStringAsync();

                    var tasks = JsonSerializer.Deserialize<List<DailyTasks>>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true // Allows case-insensitive property matching
                    });

                    return tasks;
                }
                else
                {
                    Logger.LogError($"{response}");
                }
            }

            return (new List<DailyTasks>());
        }
    }
}
