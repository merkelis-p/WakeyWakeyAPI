using System;
using NUnit.Framework;
using Moq;
using WakeyWakeyAPI.Controllers;
using WakeyWakeyAPI.Models;
using WakeyWakeyAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters.Xml;
using Task = System.Threading.Tasks.Task;

namespace WakeyWakeyAPI.Tests.ControllersTests
{
    [TestFixture]
    public class TaskControllerTests
    {
        private Mock<ITaskRepository> _mockRepository;
        private TaskController _controller;


        public List<Models.Task> ExpectedTask()
        {
            return new List<Models.Task>
            {
                new Models.Task
                {
                    Id = 1,
                    Name = "Task 1",
                    Category = 1,
                    ParentId = null, // Assuming this task has no parent
                    SubjectId = 101, // Example subject ID
                    Subject = new Subject { /* Initialize properties of Subject if needed */ },
                    UserId = 1,
                    User = new User { /* Initialize properties of User if needed */ },
                    Description = "Description of Task 1",
                    EstimatedDuration = 60, // Estimated duration in minutes
                    OverallDuration = 30, // Duration already spent
                    DeadlineDate = DateTime.Now.AddDays(5), // Deadline 5 days from now
                    Score = 10,
                    ScoreWeight = 1,
                    Status = 1, // Assuming some status code
                    SubTasks = new List<Models.Task>(), // Assuming no subtasks initially
                    Records = new List<Record>() // Assuming no records initially
                },
                new Models.Task
                {
                    Id = 2,
                    Name = "Task 2",
                    Category = 2,
                    ParentId = 1, // Assuming this is a subtask of Task 1
                    SubjectId = 102,
                    Subject = new Subject { /* Initialize properties of Subject if needed */ },
                    UserId = 1,
                    User = new User { /* Initialize properties of User if needed */ },
                    Description = "Description of Task 2",
                    EstimatedDuration = 120,
                    OverallDuration = 60,
                    DeadlineDate = DateTime.Now.AddDays(7),
                    Score = 20,
                    ScoreWeight = 2,
                    Status = 2,
                    SubTasks = new List<Models.Task>(),
                    Records = new List<Record>()
                }
            };
        }

        [SetUp]
        public void Setup()
        {
            // Prepare a list of sample tasks
            var sampleTasks = new List<Models.Task>
            {
                new Models.Task
                {
                    Id = 1,
                    Name = "Task 1",
                    Category = 1,
                    ParentId = null, // Assuming this task has no parent
                    SubjectId = 101, // Example subject ID
                    Subject = new Subject { /* Initialize properties of Subject if needed */ },
                    UserId = 1,
                    User = new User { /* Initialize properties of User if needed */ },
                    Description = "Description of Task 1",
                    EstimatedDuration = 60, // Estimated duration in minutes
                    OverallDuration = 30, // Duration already spent
                    DeadlineDate = DateTime.Now.AddDays(5), // Deadline 5 days from now
                    Score = 10,
                    ScoreWeight = 1,
                    Status = 1, // Assuming some status code
                    SubTasks = new List<Models.Task>(), // Assuming no subtasks initially
                    Records = new List<Record>() // Assuming no records initially
                },
                new Models.Task
                {
                    Id = 2,
                    Name = "Task 2",
                    Category = 2,
                    ParentId = 1, // Assuming this is a subtask of Task 1
                    SubjectId = 102,
                    Subject = new Subject { /* Initialize properties of Subject if needed */ },
                    UserId = 1,
                    User = new User { /* Initialize properties of User if needed */ },
                    Description = "Description of Task 2",
                    EstimatedDuration = 120,
                    OverallDuration = 60,
                    DeadlineDate = DateTime.Now.AddDays(7),
                    Score = 20,
                    ScoreWeight = 2,
                    Status = 2,
                    SubTasks = new List<Models.Task>(),
                    Records = new List<Record>()
                }
            };

            // Mocking ITaskRepository
            _mockRepository = new Mock<ITaskRepository>();

            // Setup mock behavior for GetAllAsync()
            _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(sampleTasks);

            // Creating the controller with the mocked repository
            _controller = new TaskController(_mockRepository.Object);
        }

        [Test]
        public async Task GetAll_ReturnsAllTasks()
        {
            // Act
            var actionResult = await _controller.GetAll();

            // Check if the result is OkObjectResult
            Assert.IsInstanceOf<OkObjectResult>(actionResult.Result, "Result is not OkObjectResult");

            var okResult = actionResult.Result as OkObjectResult;

            // Check if the OkObjectResult is not null
            Assert.IsNotNull(okResult, "OkObjectResult is null");

            var tasks = okResult.Value as IEnumerable<Models.Task>;

            // Check if the data is not null
            Assert.IsNotNull(tasks, "Tasks data is null");

            // Check if the data count is as expected
            Assert.AreEqual(2, tasks.Count(), "Tasks count mismatch");
        }
        
        
        [Test]
        public async Task GetChildrenByParentId_ReturnsSubTasksForValidId()
        {
            // Arrange
            var parentId = 1; // Example parent ID
            var expectedSubTasks = ExpectedTask().Where(task => task.ParentId == parentId);
            _mockRepository.Setup(repo => repo.GetChildrenByParentIdAsync(parentId)).ReturnsAsync(expectedSubTasks);

            // Act
            var actionResult = await _controller.GetChildrenByParentId(parentId);

            // Assert
            // Verify result as in previous tests
            
        }
        
        
        [Test]
        public async Task GetTasksWithHierarchyByUserId_ReturnsTasksForValidId()
        {
            // Arrange
            var userId = 1; // Example user ID
            var expectedTasks = new List<Models.Task> { /* Initialize with expected tasks */ };
            _mockRepository.Setup(repo => repo.GetTasksWithHierarchyByUserId(userId)).ReturnsAsync(expectedTasks);

            // Act
            var actionResult = await _controller.GetTasksWithHierarchyByUserId(userId);

            // Assert
            // Verify the result similarly
        }
        
        
        
        [Test]
        public async Task CreateTask_AddsNewTaskAndReturnsCreatedResult()
        {
            // Arrange
            var taskCreateRequest = new TaskCreateRequest { /* Initialize with required data */ };
            var createdTask = new Models.Task { /* Initialize with expected created task data */ };
            _mockRepository.Setup(repo => repo.AddTaskAsync(taskCreateRequest)).ReturnsAsync(createdTask);

            // Act
            var actionResult = await _controller.CreateTask(taskCreateRequest);

            // Assert
            Assert.IsInstanceOf<CreatedAtActionResult>(actionResult.Result);
            var createdAtResult = actionResult.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdAtResult);
            var task = createdAtResult.Value as Models.Task;
            Assert.IsNotNull(task);
            // Further assertions based on expected values
        }
        

        
        [Test]
        public async Task GetBySubjectId_ReturnsTasksForValidSubjectId()
        {
            int subjectId = 123; // Example subject ID
            var expectedTasks = new List<Models.Task>
            {
                new Models.Task { /* Initialize properties as needed */ },
                new Models.Task { /* Initialize properties as needed */ }
            };

            _mockRepository.Setup(repo => repo.GetTasksWithHierarchyBySubjectId(subjectId)).ReturnsAsync(expectedTasks);

            var actionResult = await _controller.GetBySubjectId(subjectId);

            Assert.IsInstanceOf<OkObjectResult>(actionResult.Result);
            var okResult = actionResult.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var tasks = okResult.Value as IEnumerable<Models.Task>;
            Assert.IsNotNull(tasks);
            Assert.AreEqual(expectedTasks.Count, tasks.Count());

            // Test case where repository returns null
            _mockRepository.Setup(repo => repo.GetTasksWithHierarchyBySubjectId(It.IsAny<int>())).ReturnsAsync((IEnumerable<Models.Task>)null);
            var notFoundResult = await _controller.GetBySubjectId(0);
            Assert.IsInstanceOf<NotFoundResult>(notFoundResult.Result);
        }
        
        
        
        [Test]
        public async Task GetChildrenByParentId_ReturnsChildTasksForValidParentId()
        {
            int parentId = 456; // Example parent task ID
            var expectedTasks = new List<Models.Task>
            {
                new Models.Task { /* Initialize properties as needed */ },
                new Models.Task { /* Initialize properties as needed */ }
            };

            _mockRepository.Setup(repo => repo.GetChildrenByParentIdAsync(parentId)).ReturnsAsync(expectedTasks);

            var actionResult = await _controller.GetChildrenByParentId(parentId);

            Assert.IsInstanceOf<OkObjectResult>(actionResult.Result);
            var okResult = actionResult.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var tasks = okResult.Value as IEnumerable<Models.Task>;
            Assert.IsNotNull(tasks);
            Assert.AreEqual(expectedTasks.Count, tasks.Count());

            // Test case where repository returns an empty list
            _mockRepository.Setup(repo => repo.GetChildrenByParentIdAsync(It.IsAny<int>())).ReturnsAsync(new List<Models.Task>());
            var notFoundResult = await _controller.GetChildrenByParentId(0);
            Assert.IsInstanceOf<NotFoundResult>(notFoundResult.Result);
        }
        
        [Test]
        public async Task GetTasksWithHierarchyByUserId_ReturnsTasksForValidUserId()
        {
            int userId = 789; // Example user ID
            var expectedTasks = new List<Models.Task>
            {
                new Models.Task { /* Initialize properties as needed */ },
                new Models.Task { /* Initialize properties as needed */ }
            };

            _mockRepository.Setup(repo => repo.GetTasksWithHierarchyByUserId(userId)).ReturnsAsync(expectedTasks);

            var actionResult = await _controller.GetTasksWithHierarchyByUserId(userId);

            Assert.IsInstanceOf<OkObjectResult>(actionResult.Result);
            var okResult = actionResult.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var tasks = okResult.Value as IEnumerable<Models.Task>;
            Assert.IsNotNull(tasks);
            Assert.AreEqual(expectedTasks.Count, tasks.Count());

            // Test case for no tasks found
            _mockRepository.Setup(repo => repo.GetTasksWithHierarchyByUserId(It.IsAny<int>())).ReturnsAsync((IEnumerable<Models.Task>)null);
            var notFoundResult = await _controller.GetTasksWithHierarchyByUserId(0);
            Assert.IsInstanceOf<NotFoundResult>(notFoundResult.Result);
        }
        
        [Test]
        public async Task GetTasksWithHierarchy_ReturnsAllTasksWithHierarchy()
        {
            var expectedTasks = new List<Models.Task>
            {
                new Models.Task { /* Initialize properties as needed */ },
                new Models.Task { /* Initialize properties as needed */ }
            };

            _mockRepository.Setup(repo => repo.GetTasksWithHierarchy()).ReturnsAsync(expectedTasks);

            var actionResult = await _controller.GetTasksWithHierarchy();

            Assert.IsInstanceOf<OkObjectResult>(actionResult.Result);
            var okResult = actionResult.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var tasks = okResult.Value as IEnumerable<Models.Task>;
            Assert.IsNotNull(tasks);
            Assert.AreEqual(expectedTasks.Count, tasks.Count());

            // Test case for no tasks found
            _mockRepository.Setup(repo => repo.GetTasksWithHierarchy()).ReturnsAsync((IEnumerable<Models.Task>)null);
            var notFoundResult = await _controller.GetTasksWithHierarchy();
            Assert.IsInstanceOf<NotFoundResult>(notFoundResult.Result);
        }
        
        
        
        [Test]
        public async Task CreateTask_ReturnsCreatedTask()
        {
            var taskCreateRequest = new TaskCreateRequest 
            {
                Id = 10,
                Name = "Sample",
                Category = 1,
                DeadlineDate = DateTime.Now.AddDays(5),
                Description = "Sample Description",
                EstimatedDuration = 60,
                OverallDuration = 54,
                ParentId = null,
                Score = 10,
                ScoreWeight = 5,
                Status = 1,
                SubjectId = 5,
                UserId = 1
            };
            var createdTask = new Models.Task
            {
                Id = 10,
                Name = "Sample",
                Category = 1,
                DeadlineDate = DateTime.Now.AddDays(5),
                Description = "Sample Description",
                EstimatedDuration = 60,
                OverallDuration = 54,
                ParentId = null,
                Score = 10,
                ScoreWeight = 5,
                Status = 1,
                SubjectId = 5,
                UserId = 1
            };
            
            _mockRepository.Setup(repo => repo.AddTaskAsync(taskCreateRequest)).ReturnsAsync(createdTask);

            // Act & Assert for a valid request
            var validActionResult = await _controller.CreateTask(taskCreateRequest);
            Assert.IsInstanceOf<CreatedAtActionResult>(validActionResult.Result);
            var createdAtActionResult = validActionResult.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdAtActionResult);
            var task = createdAtActionResult.Value as Models.Task;
            Assert.IsNotNull(task);
            Assert.AreEqual(createdTask.Id, task.Id);

            // Act & Assert for a null request
            var nullActionResult = await _controller.CreateTask(null);
            Assert.IsInstanceOf<BadRequestObjectResult>(nullActionResult.Result); // Note: It's BadRequestObjectResult if you return a message with BadRequest()
        }
        
        
        


     
        
        



        
        
        

        [TearDown]
        public void TearDown()
        {
            _controller = null;
        }
    }
}