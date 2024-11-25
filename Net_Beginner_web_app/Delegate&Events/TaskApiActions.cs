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

        public TaskApiActions(IHttpClientFactory httpFactory,IDataStore data, ITaskDataServices taskServices)
        {
            _httpClient = httpFactory.CreateClient("ApiClient");
            userEmail = data.GetTheUserDataFromSession();
            _taskServices = taskServices;
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

                    return tasks;

                }
                else
                {
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
                    Console.WriteLine(_taskServices.getDailyTasks());
                    _taskServices.setDailyTask(tasks);
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
                        File_name = tasks.File_name,
                        Attachments = tasks.Attachments,
                    };
                    var taskIndex = allTasks.FindIndex(t => t.Task_id == tasks.Task_id);
                    allTasks[taskIndex] = list;
                   _taskServices.setAllDailyTasks(allTasks);
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
                    var allTasks = _taskServices.getDailyTasks();
                    var taskIndex = allTasks.FindIndex(t => t.Task_id == id);
                    allTasks.RemoveAt(taskIndex);
                   _taskServices.setAllDailyTasks(allTasks);
                }
            }
            return (new List<DailyTasks>()); ;
        }

        public async Task<List<DailyTasks>> GetallData(string url)
        {
            if(!string.IsNullOrEmpty(url))
            {
                var response = await _httpClient.GetAsync(url);
                if(response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                    var tasks = JsonSerializer.Deserialize<List<DailyTasks>>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true // Allows case-insensitive property matching
                    });

                    return tasks;
                }
            }

            return (new List<DailyTasks>());
        }
    }
}
