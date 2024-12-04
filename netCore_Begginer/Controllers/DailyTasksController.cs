using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using netCore_Begginer.Interfaces;
using netCore_Begginer.Mappings;
using netCore_Begginer.Models;

namespace netCore_Begginer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DailyTasksController : ControllerBase
    {

        private readonly IBaseManager<DailyTasks, string> Tasks;
        private readonly TaskMapper TaskMapper;

        public DailyTasksController(IBaseManager<DailyTasks, string> tasks)
        {
            Tasks = tasks;
            TaskMapper = new TaskMapper();
        }

        [Authorize]
        [HttpPost("add")]
        public async Task<IActionResult> AddTask(DailyTasks task)
        {
            if (task != null)
            {
                await Tasks.AddTheData(task);
                return Ok();
            }
            else
            {
                return BadRequest();
            }

        }

      
        [HttpPut("edit/{id}")]
        public async Task<IActionResult>EditTask(string id, [FromBody] EditDailyTasks tasks)
        {
            if (tasks != null && !string.IsNullOrEmpty(id))
            {
                var dailyTasks = TaskMapper.MapTasks(tasks, id);
                await Tasks.EditTheData(dailyTasks, id);
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

      
        [HttpDelete("delete/{TaskId}")]
        public async Task<IActionResult>DeleteTask(string TaskId)
        {
            if(!string.IsNullOrEmpty(TaskId))
            {
                await Tasks.DeleteTheData(TaskId);
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
        [Authorize]
        [HttpGet("getTaskInfo/{id}")]
        public async Task<ActionResult<DailyTasks>>GetTaskData(string id)
        {
            if(!string.IsNullOrEmpty(id))
            {
                var taskData = await Tasks.GetTheData(id);
                return Ok(taskData);
            }
            else
            {
                return BadRequest();
            }
        }

        [Authorize]
        [HttpGet("getAllTasks/{email}")]

        public async Task<ActionResult<DailyTasks>> GetAllTasks(string email)
        {
            if(!string.IsNullOrEmpty(email))
            {
                var data = await Tasks.GetAllTheData("Email",email);

                return Ok(data);
            }else
            {
                return BadRequest();
            }
        }
    }
}

