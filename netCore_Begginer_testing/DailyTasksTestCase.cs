

using Microsoft.AspNetCore.Mvc;
using Moq;
using netCore_Begginer.Controllers;
using netCore_Begginer.Interfaces;
using netCore_Begginer.Models;
using netCore_Begginer.Repository;
using System.Threading.Tasks;

namespace netCore_Begginer_testing
{
    public class DailyTasksTestCase
    {
        private readonly Mock<IBaseManager<DailyTasks, string>> MockTaskService;
        private readonly DailyTasksController DailyTasksController;

        public DailyTasksTestCase()
        {
            MockTaskService = new Mock<IBaseManager<DailyTasks, string>>();
            DailyTasksController = new DailyTasksController(MockTaskService.Object); 
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
                Description = "Test"
            };

            MockTaskService.Setup(s => s.AddTheData(task)).Returns(Task.CompletedTask);

            var result = await DailyTasksController.AddTask(task);
            Assert.IsType<OkResult>(result);

            MockTaskService.Verify(service => service.AddTheData(task), Times.Once);

        }

        [Fact]
        public async Task AddTask_ShouldReturnBadRequest_WhenTaskIsNull()
        {
            DailyTasks tasks = null;

            var result = await DailyTasksController.AddTask(tasks);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task EditTask_ShouldReturnOk_WhenValidDataIsProvided()
        {
            // Arrange
            string taskId = "BG-234";

            var editTask = new EditDailyTasks
            {
                Task_name = "Test",
                Issue_type = "Bug",
                Status = "Done",
                Assignee = "Sana Syed",
                Description = "Test"
            };

            MockTaskService.Setup(s => s.EditTheData(It.IsAny<DailyTasks>(), taskId))
                            .Returns(Task.CompletedTask);


            var result = await DailyTasksController.EditTask(taskId, editTask);

            var okResult = Assert.IsType<OkResult>(result);
            MockTaskService.Verify(service => service.EditTheData(It.IsAny<DailyTasks>(), taskId), Times.Once);
        }



        [Fact]
        public async Task EditTask_ShouldReturnBadRequest_WhenDataIsNull()
        {
            string Id = null;
            EditDailyTasks tasks = null;


            var result = await DailyTasksController.EditTask(Id, tasks);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task DeleteTask_ShouldReturnOk_afterDataDeleted()
        {
            string taskId = "BG-234";

            MockTaskService.Setup(s=>s.DeleteTheData(taskId)).Returns(Task.CompletedTask);

            var result = await DailyTasksController.DeleteTask(taskId);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            MockTaskService.Verify(service => service.DeleteTheData(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task DeleteTask_ShouldReturnBadRequest_WhenTaskIdIsNullOrEmpty()
        {
            // Arrange
            string taskId = null;

            // Act
            var result = await DailyTasksController.DeleteTask(taskId);

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
                Description = "Test"
            };

            MockTaskService.Setup(service => service.GetTheData(It.IsAny<string>()))
                   .ReturnsAsync(task);

            var result = await DailyTasksController.GetTaskData(taskId);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedTask = Assert.IsType<DailyTasks>(okResult.Value);
            Assert.Equal(taskId, returnedTask.Task_id);
        }

        [Fact]
        public async Task GetTaskData_ShouldReturnBadRequest_WhenTaskIdIsNullOrEmpty()
        {
            string taskId = null;

            var result = await DailyTasksController.GetTaskData(taskId);

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
                 Description = "Test"
                }
               
            };
            MockTaskService.Setup(service => service.GetAllTheData("Email", email))
                    .ReturnsAsync(tasks); 

            // Act
            var result = await DailyTasksController.GetAllTasks(email);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedTasks = Assert.IsType<List<DailyTasks>>(okResult.Value);
            Assert.Equal(tasks.Count, returnedTasks.Count);

        }

        [Fact]
        public async Task GetAllTasks_ShouldReturnBadRequest_WhenEmailIsNullOrEmpty()
        {
            
            string email = null;

           
            var result = await DailyTasksController.GetAllTasks(email);

            var badRequestResult = Assert.IsType<BadRequestResult>(result.Result);

            // Optionally, ensure `Value` is null (not applicable for BadRequest)
            Assert.Null(result.Value);
        }

    }
}
