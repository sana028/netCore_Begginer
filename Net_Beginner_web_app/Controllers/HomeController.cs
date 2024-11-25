using Microsoft.AspNetCore.Mvc;
using Net_Beginner_web_app.Delegate_Events;
using Net_Beginner_web_app.Interfaces;
using Net_Beginner_web_app.Models;
using netCore_Begginer.Models.Tasks;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;

namespace Net_Beginner_web_app.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDataStore _dataStore;
        private readonly ApiNotification<DailyTasks> apiNotification;
        private readonly ApiNotification<List<DailyTasks>> allListInfo;
        private readonly TaskApiActions _taskApiActions;
        private readonly ITaskDataServices _taskDataServices;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpFactory, IDataStore dataStore, ApiNotification<List<DailyTasks>> listInfo,ApiNotification<DailyTasks> api, TaskApiActions taskApiActions, ITaskDataServices taskDataServices)
        {
            _logger = logger;
            _dataStore = dataStore;
            apiNotification = api;
            _taskApiActions = taskApiActions;
            allListInfo = listInfo;

            api.Toaster += Toaster;
            allListInfo.Toaster += Toaster;
            _taskDataServices = taskDataServices;
        }

        public async Task<IActionResult> Index()
        {
            var allTasksList = _taskDataServices?.getDailyTasks();
            if (allTasksList?.Count == 0)
            {
                var userEmail = _dataStore.GetTheUserDataFromSession();

                var data = await allListInfo.ExecuteApiCall(() =>
                    _taskApiActions.GetallData($"/api/DailyTasks/getalltasks/{userEmail}"), "GetAll");
                _taskDataServices?.setAllDailyTasks(data);
                return View(data);
            }

            return View(allTasksList);
        }

        public IActionResult AddTask()
        {
            var issueTypes = Enum.GetValues(typeof(IssueTypes))
                            .Cast<IssueTypes>()
                            .Select(e => e.ToString())
                            .ToList();
            var status = Enum.GetValues(typeof(Status))
                         .Cast<Status>()
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
                var userEmail = _dataStore.GetTheUserDataFromSession();
                task.Email = userEmail;
                await allListInfo.ExecuteApiCall(() => _taskApiActions.PostAsync("/api/DailyTasks/add", task), "Add");
                return RedirectToAction("Index");
                
            }
            ViewBag.ActionType = "Add";

            return PartialView();
        }

        public async Task<IActionResult> EditTask(string taskID)
        {
            var taskIDData = await apiNotification.ExecuteApiCall(() => _taskApiActions.GetAsync("/api/DailyTasks/getTaskInfo/", taskID), "GetTask");
            var issueTypes = Enum.GetValues(typeof(IssueTypes))
                            .Cast<IssueTypes>()
                            .Select(e => e.ToString())
                            .ToList();
            var status = Enum.GetValues(typeof(Status))
                         .Cast<Status>()
                         .Select(e => e.ToString())
                         .ToList();

            ViewBag.IssueTypes = issueTypes;
            ViewBag.Status = status;
            ViewBag.ActionType = "EditTask";
            return PartialView("AddTask",taskIDData);
        }
        [HttpPost]
        public async Task<IActionResult> EditTask(EditTasks task)
        {
            if (task != null)
            {
                await allListInfo.ExecuteApiCall(() => _taskApiActions.PutAsync("/api/DailyTasks/edit/",task),"Edit");
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
               var result = await allListInfo.ExecuteApiCall(() => _taskApiActions.DeleteAsync("/api/DailyTasks/delete/", taskId),"Delete");
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
                var result = await apiNotification.ExecuteApiCall(() => _taskApiActions.GetAsync("/api/DailyTasks/getTaskInfo/",taskId),"GetTask");
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
