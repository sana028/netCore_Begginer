

using Microsoft.AspNetCore.Mvc;
using Moq;
using netCore_Begginer.Controllers;
using netCore_Begginer.Interfaces;
using netCore_Begginer.Models;
using System.Threading.Tasks;

namespace netCore_Begginer_testing
{
    public class DailyTasksTestCase
    {
        private readonly Mock<ITaskManager<DailyTasks, string>> _mockTaskService;
        private readonly Mock<ITaskManager<EditDailyTasks, string>> _mockTaskManager;

        private readonly DailyTasksController dailyTasksController;

        public DailyTasksTestCase()
        {
            _mockTaskService = new Mock<ITaskManager<DailyTasks, string>>();
            _mockTaskManager = new Mock<ITaskManager<EditDailyTasks, string>>();
            dailyTasksController = new DailyTasksController(_mockTaskService.Object, _mockTaskManager.Object);
        }

        [Fact]
        public async Task AddTask_ShouldReturnOk_WhenValidTaskIsProvided()
        {
            var task = new DailyTasks
            {
                Task_id = "BG-234",
                Task_name = "Test",
                Issue_type = "Bug",
                Status = "Done",
                Email = "sana@ee.com",
                Assignee = "Sana Syed",
                Attachments = null,
                File_name = "Test",
                Description = "Test"
            };

            _mockTaskService.Setup(s => s.AddTheData(task)).Returns(Task.CompletedTask);

            var result = await dailyTasksController.AddTask(task);
            var okResult = Assert.IsType<OkResult>(result);
            _mockTaskService.Verify(service => service.AddTheData(It.IsAny<DailyTasks>()), Times.Once);
        }

        [Fact]
        public async Task AddTask_ShouldReturnBadRequest_WhenTaskIsNull()
        {
            DailyTasks tasks = null;

            var result = await dailyTasksController.AddTask(tasks);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task EditTask_ShouldReturnOk_WhenValidDataIsProvided()
        {
            var taskId = "BG-234";
            var task = new EditDailyTasks
            {
                Task_name = "Test",
                Issue_type = "Bug",
                Status = "Done",
                Assignee = "Sana Syed",
                Attachments = null,
                File_name = "Test",
                Description = "Test"
            };

            _mockTaskManager.Setup(s => s.EditTheData(task, taskId)).Returns(Task.CompletedTask);
            var result = await dailyTasksController.EditTask(taskId, task);

            var okResult = Assert.IsType<OkResult>(result);
            _mockTaskManager.Verify(service => service.EditTheData(It.IsAny<EditDailyTasks>(), It.IsAny<string>()), Times.Once);
        }


        [Fact]
        public async Task EditTask_ShouldReturnBadRequest_WhenDataIsNull()
        {
            string Id = null;
            EditDailyTasks tasks = null;


            var result = await dailyTasksController.EditTask(Id, tasks);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task DeleteTask_ShouldReturnOk_afterDataDeleted()
        {
            string taskId = "BG-234";

            _mockTaskService.Setup(s=>s.DeleteTheData(taskId)).Returns(Task.CompletedTask);

            var result = await dailyTasksController.DeleteTask(taskId);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            _mockTaskService.Verify(service => service.DeleteTheData(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task DeleteTask_ShouldReturnBadRequest_WhenTaskIdIsNullOrEmpty()
        {
            // Arrange
            string taskId = null;

            // Act
            var result = await dailyTasksController.DeleteTask(taskId);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task GetTaskData_ShouldReturnOk_WhenValidTaskIdIsProvided()
        {
            string taskId = "BG-234";
            var task = new DailyTasks
            {
                Task_id = "BG-234",
                Task_name = "Test",
                Issue_type = "Bug",
                Status = "Done",
                Email = "sana@ee.com",
                Assignee = "Sana Syed",
                Attachments = null,
                File_name = "Test",
                Description = "Test"
            };

            _mockTaskService.Setup(service => service.GetTheData(It.IsAny<string>()))
                   .ReturnsAsync(task);

            var result = await dailyTasksController.GetTaskData(taskId);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedTask = Assert.IsType<DailyTasks>(okResult.Value);
            Assert.Equal(taskId, returnedTask.Task_id);
        }

        [Fact]
        public async Task GetTaskData_ShouldReturnBadRequest_WhenTaskIdIsNullOrEmpty()
        {
            string taskId = null;

            var result = await dailyTasksController.GetTaskData(taskId);

            Assert.IsType<BadRequestResult>(result.Result);
        }

        [Fact]
        public async Task GetAllTasks_ShouldReturnOk_WhenValidEmailIsProvided()
        {
            string email = "sana@ee.com";

            var tasks = new List<DailyTasks>
            {
                new DailyTasks
                {
                 Task_id = "BG-234",
                 Task_name = "Test",
                 Issue_type = "Bug",
                 Status = "Done",
                 Email = "sana@ee.com",
                 Assignee = "Sana Syed",
                 Attachments = null,
                 File_name = "Test",
                 Description = "Test"
                },
                new DailyTasks
                {

                 Task_id = "BG-123",
                 Task_name = "Test1",
                 Issue_type = "Task",
                 Status = "In-Progress",
                 Email = "sana@123.com",
                 Assignee = "Sana",
                 Attachments = null,
                 File_name = "Test",
                 Description = "Test"
                }
               
            };
            _mockTaskService.Setup(service => service.GetAllTheData(email))
                    .ReturnsAsync(tasks); 

            // Act
            var result = await dailyTasksController.GetAllTasks(email);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedTasks = Assert.IsType<List<DailyTasks>>(okResult.Value);
            Assert.Equal(tasks.Count, returnedTasks.Count);

        }

        [Fact]
        public async Task GetAllTasks_ShouldReturnBadRequest_WhenEmailIsNullOrEmpty()
        {
            
            string email = null;

           
            var result = await dailyTasksController.GetAllTasks(email);

            var badRequestResult = Assert.IsType<BadRequestResult>(result.Result);

            // Optionally, ensure `Value` is null (not applicable for BadRequest)
            Assert.Null(result.Value);
        }

    }
}
