using Microsoft.AspNetCore.Mvc;
using Net_Beginner_web_app.Delegate_Events;
using Net_Beginner_web_app.Enums;
using Net_Beginner_web_app.Interfaces;
using Net_Beginner_web_app.Models;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;

namespace Net_Beginner_web_app.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISessionStore DataStore;
        private readonly ApiNotification<DailyTasks> ApiNotification;
        private readonly ApiNotification<List<DailyTasks>> ApiListNotification ;
        private readonly TaskApiActions TaskApiActions;
        private readonly IDataServices TaskDataServices;
        private readonly ILogger<HomeController> Logger;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpFactory, ISessionStore dataStore, ApiNotification<List<DailyTasks>> listInfo,ApiNotification<DailyTasks> api, TaskApiActions taskApiActions, IDataServices taskDataServices)
        {
            DataStore = dataStore;
            ApiNotification = api;
            TaskApiActions = taskApiActions;
            ApiListNotification = listInfo;
            Logger = logger;
            api.Toaster += Toaster;
            ApiListNotification.Toaster += Toaster;
            TaskDataServices = taskDataServices;
        }

        public async Task<IActionResult> Index()
        {
            var allTasksList = TaskDataServices?.GetDailyTasks();
            if (allTasksList?.Count == 0)
            {
                var userEmail = DataStore.GetTheUserDataFromSession();
                Logger.LogInformation($"user Email hello {userEmail}");
                var data = await ApiListNotification.ExecuteApiCall(() =>
                    TaskApiActions.GetallData($"/api/DailyTasks/getalltasks/{userEmail}"), "GetAll");
                TaskDataServices?.SetAllDailyTasks(data);
                return View(data);
            }

            return View(allTasksList);
        }

        public IActionResult AddTask()
        {
            var issueTypes = Enum.GetValues(typeof(IssueTypeEnum))
                            .Cast<IssueTypeEnum>()
                            .Select(e => e.ToString())
                            .ToList();
            var status = Enum.GetValues(typeof(StatusEnum))
                         .Cast<StatusEnum>()
                         .Select(e => e.ToString())
                         .ToList();

            ViewBag.IssueTypes = issueTypes;
            ViewBag.Status = status;
            ViewBag.ActionType = "AddTask";
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> AddTask(DailyTasks task)
        {

            if (task != null)
            {
                var userEmail = DataStore.GetTheUserDataFromSession();
                task.Email = userEmail;
                await ApiListNotification.ExecuteApiCall(() => TaskApiActions.PostAsync("/api/DailyTasks/add", task), "Add");
                return RedirectToAction("Index");
                
            }
            ViewBag.ActionType = "Add";

            return PartialView();
        }

        public async Task<IActionResult> EditTask(string taskID)
        {
            var taskIDData = await ApiNotification.ExecuteApiCall(() => TaskApiActions.GetAsync("/api/DailyTasks/getTaskInfo/", taskID), "Updated");
            var issueTypes = Enum.GetValues(typeof(IssueTypeEnum))
                            .Cast<IssueTypeEnum>()
                            .Select(e => e.ToString())
                            .ToList();
            var status = Enum.GetValues(typeof(StatusEnum))
                         .Cast<StatusEnum>()
                         .Select(e => e.ToString())
                         .ToList();

            ViewBag.IssueTypes = issueTypes;
            ViewBag.Status = status;
            ViewBag.ActionType = "EditTask";
            return PartialView("AddTask",taskIDData);
        }
        [HttpPost]
        public async Task<IActionResult> EditTask(EditDailyTasks task)
        {
            if (task != null)
            {
                await ApiListNotification.ExecuteApiCall(() => TaskApiActions.PutAsync("/api/DailyTasks/edit/",task),"Edit");
                return RedirectToAction("Index");

            }
           
            return PartialView();
        }

     
        public IActionResult DeleteTask(string taskID)
        {
            ViewBag.taskId = taskID;
            return PartialView("DeleteTask");
        }

      
        public  async Task<IActionResult> DeleteTaskData(string taskId)
        {
            if(!string.IsNullOrEmpty(taskId))
            {
               var result = await ApiListNotification.ExecuteApiCall(() => TaskApiActions.DeleteAsync("/api/DailyTasks/delete/", taskId),"Delete");
                return RedirectToAction("Index");
            }
            return PartialView("DeleteTask");
        }
        public IActionResult GetTaskDetails()
        {
            return PartialView();
        }

        public async Task<IActionResult> GetTaskDetails(string taskId)
        {
            if(!string.IsNullOrEmpty(taskId))
            {
                var result = await ApiNotification.ExecuteApiCall(() => TaskApiActions.GetAsync("/api/DailyTasks/getTaskInfo/",taskId),"GetTask");
                return PartialView(result);
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Toaster(string message = "", string type = "success")
        {
            TempData["Message"] = message;
            TempData["Type"] = type;
            return PartialView("Toaster");
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
