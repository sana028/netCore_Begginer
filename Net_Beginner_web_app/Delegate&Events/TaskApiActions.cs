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
        private readonly HttpClient _httpClient;
        private static string getallDataUrl = "/api/DailyTasks/getalltasks/";
        private static string userEmail = null;
        private readonly  ITaskDataServices _taskServices;
        private readonly ILogger<TaskApiActions> _logger;

        public TaskApiActions(IHttpClientFactory httpFactory,IDataStore data, ITaskDataServices taskServices, ILogger<TaskApiActions> logger)
        {
            _httpClient = httpFactory.CreateClient("ApiClient");
            userEmail = data.GetTheUserDataFromSession();
            _taskServices = taskServices;
            _logger = logger;
        }

        public async Task<DailyTasks> GetAsync(string url,string id)
        {
            if(!string.IsNullOrEmpty(id)) { 
                var response = await _httpClient.GetAsync(url+$"{id}");
                if(response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                    var tasks = JsonSerializer.Deserialize<DailyTasks>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true // Allows case-insensitive property matching
                    });
                    _logger.LogInformation($"Fetched the Task details for the task-id {id}");
                    return tasks;

                }
                else
                {
                    _logger.LogError($"Got some error from the api {response}");
                    return null; 
                }
            }
            return null;
        }

        public async Task<List<DailyTasks>> PostAsync(string url,DailyTasks tasks)
        {
            if(!string.IsNullOrEmpty(url)&& tasks!=null) {
                var response = await _httpClient.PostAsJsonAsync(url,tasks);
                if(response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"Added data successfully to table {response.StatusCode}");
                    _taskServices.setDailyTask(tasks);
                }
                else
                {
                    _logger.LogError($"{response.StatusCode}");
                }
            }
            return (new List<DailyTasks>()); ;
        }

        public async Task<List<DailyTasks>>PutAsync(string url,EditTasks tasks)
        {
            if(!string.IsNullOrEmpty(url) && tasks != null)
            {
                var response = await _httpClient.PutAsJsonAsync(url+$"{tasks.Task_id}", tasks);
                if(response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"Updated data to the table for the task-id {tasks.Task_id},with status {response.StatusCode}");
                    var allTasks =  _taskServices.getDailyTasks();
                    var list = new DailyTasks
                    {
                        Task_id = tasks.Task_id,
                        Task_name = tasks.Task_name,
                        Assignee = tasks.Assignee,
                        Issue_type = tasks.Issue_type,
                        Status = tasks.Status,
                        Email = userEmail,
                        Description = tasks.Description,
                    };
                    var taskIndex = allTasks.FindIndex(t => t.Task_id == tasks.Task_id);
                    allTasks[taskIndex] = list;
                   _taskServices.setAllDailyTasks(allTasks);
                }
                else
                {
                    _logger.LogError($"{response}");
                }
            }
            return (new List<DailyTasks>());
        }

        public async Task<List<DailyTasks>> DeleteAsync(string url,string id)
        {
            if (!string.IsNullOrEmpty(url) && !string.IsNullOrEmpty(id))
            {
                var response = await _httpClient.DeleteAsync(url + id);
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"Successfully deleted the task with the id {id},{response.StatusCode}");
                    var allTasks = _taskServices.getDailyTasks();
                    var taskIndex = allTasks.FindIndex(t => t.Task_id == id);
                    allTasks.RemoveAt(taskIndex);
                   _taskServices.setAllDailyTasks(allTasks);
                }
                else
                {
                    _logger.LogError($"{response}");
                }
            }
            return (new List<DailyTasks>()); ;
        }

        public async Task<List<DailyTasks>> GetallData(string url)
        {
            if(!string.IsNullOrEmpty(url))
            {
                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogTrace($"Fetched all tasks of this user email {userEmail}");
                    var json = await response.Content.ReadAsStringAsync();

                    var tasks = JsonSerializer.Deserialize<List<DailyTasks>>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true // Allows case-insensitive property matching
                    });

                    return tasks;
                }
                else
                {
                    _logger.LogError($"{response}");
                }
            }

            return (new List<DailyTasks>());
        }
    }
}
