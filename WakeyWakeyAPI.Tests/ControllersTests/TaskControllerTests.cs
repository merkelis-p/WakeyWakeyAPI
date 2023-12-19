// using NUnit.Framework;
// using Moq;
// using WakeyWakeyAPI.Controllers;
// using WakeyWakeyAPI.Models;
// using WakeyWakeyAPI.Repositories;
// using Microsoft.AspNetCore.Mvc;
// using System.Collections.Generic;
// using Task = System.Threading.Tasks.Task;
// using System.Linq;
//
// namespace WakeyWakeyAPI.Tests.ControllersTests
// {
//     [TestFixture]
//     public class TaskControllerTests
//     {
//         private Mock<ITaskRepository> _mockRepository;
//         private TaskController _controller;
//
//         [SetUp]
//         public void Setup()
//         {
//             // Arrange
//             _mockRepository = new Mock<ITaskRepository>();
//             var sampleTasks = new List<Models.Task>
//             {
//                 new Models.Task { Id = 1, Name = "Task 1" },
//                 new Models.Task { Id = 2, Name = "Task 2" }
//             };
//
//             _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(sampleTasks);
//             _controller = new TaskController(_mockRepository.Object);
//         }
//
//         [Test]
//         public async Task GetAll_ReturnsAllTasks()
//         {
//             // Act
//             var result = await _controller.GetAll();
//
//             // Assert
//             var okResult = result.Result as OkObjectResult;
//             Assert.IsNotNull(okResult);
//             var tasks = okResult.Value as IEnumerable<Task>;
//             Assert.IsNotNull(tasks);
//             Assert.AreEqual(2, tasks.Count());
//         }
//
//         // Additional tests...
//
//         [TearDown]
//         public void TearDown()
//         {
//             _controller = null;
//         }
//     }
// }