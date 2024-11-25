﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using netCore_Begginer.Interfaces;
using netCore_Begginer.Models;

namespace netCore_Begginer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DailyTasksController : ControllerBase
    {

        private readonly ITaskManager<DailyTasks, string> _tasks;
        private readonly ITaskManager<EditDailyTasks, string> _taskManager;

        public DailyTasksController(ITaskManager<DailyTasks, string> tasks,ITaskManager<EditDailyTasks,string>taskManager)
        {
            _tasks = tasks;
            _taskManager = taskManager;
        }

        [Authorize]
        [HttpPost("add")]
        public async Task<IActionResult> AddTask(DailyTasks task)
        {
            if (task != null)
            {
                await _tasks.AddTheData(task);
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
                var dailyTasks = new DailyTasks
                {
                    Task_id = id,
                    Task_name = tasks.Task_name,
                    Issue_type = tasks.Issue_type,
                    Status = tasks.Status,
                    Assignee = tasks.Assignee,
                    File_name = tasks.File_name,
                    Attachments = tasks.Attachments,
                    Description = tasks.Description,
                };
                await _tasks.EditTheData(dailyTasks, id);
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
                await _tasks.DeleteTheData(TaskId);
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
                var taskData = await _tasks.GetTheData(id);
                return Ok(taskData);
            }
            else
            {
                return BadRequest();
            }
        }

        [Authorize]
        [HttpGet("getalltasks/{email}")]

        public async Task<ActionResult<DailyTasks>> GetAllTasks(string email)
        {
            if(!string.IsNullOrEmpty(email))
            {
                var data = await _tasks.GetAllTheData(email);

                return Ok(data);
            }else
            {
                return BadRequest();
            }
        }
    }
}
