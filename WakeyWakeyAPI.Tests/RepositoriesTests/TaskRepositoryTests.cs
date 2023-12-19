using System;
using NUnit.Framework;
using Moq;
using Microsoft.EntityFrameworkCore;
using WakeyWakeyAPI.Models;
using WakeyWakeyAPI.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore.InMemory;
using System.Threading.Tasks;
using System.Linq;
using System.Threading;
using Task = System.Threading.Tasks.Task;


namespace WakeyWakeyAPI.Tests.RepositoriesTests
{
    
    [TestFixture]
    public class TaskRepositoryTests
    {
        private TaskRepository _repository;
        private wakeyContext _context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<wakeyContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new wakeyContext(options);
            _repository = new TaskRepository(_context, new Mock<ILogger<TaskRepository>>().Object);
    
            // Adding a task with a known ID
            var testTask = new Models.Task { Id = 100, Name = "Sample Task" };
            _context.Tasks.Add(testTask);
            _context.SaveChanges();
        }


        
        [Test]
        public async Task GetByIdAsync_ReturnsCorrectTask()
        {
            // Act
            var result = await _repository.GetByIdAsync(100); // Use the same ID as added in the setup

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(100, result.Id);
            Assert.AreEqual("Sample Task", result.Name);
        }

        
        
        [Test]
        public async Task GetAllAsync_ReturnsAllTasks()
        {
            // Arrange - Adding more unique tasks
            _context.Tasks.Add(new Models.Task { Id = Guid.NewGuid().GetHashCode(), Name = "Task 2" });
            _context.Tasks.Add(new Models.Task { Id = Guid.NewGuid().GetHashCode(), Name = "Task 3" });
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.AreEqual(3, result.Count()); // Assuming one task was added in the setup
        }

        
        [Test]
        public async Task AddAsync_AddsTask()
        {
            // Arrange
            var newTask = new Models.Task { Id = 3, Name = "New Task" };

            // Act
            await _repository.AddAsync(newTask);
            var addedTask = await _context.Tasks.FindAsync(3);

            // Assert
            Assert.IsNotNull(addedTask);
            Assert.AreEqual("New Task", addedTask.Name);
        }
        
        
       
        [Test]
        public void Update_UpdatesTask()
        {
            // Arrange
            var existingTask = new Models.Task { Id = 4, Name = "Old Task" };
            _context.Tasks.Add(existingTask);
            _context.SaveChanges();

            var updatedTask = new Models.Task { Id = 4, Name = "Updated Task" };

            // Act
            _repository.Update(updatedTask);
            var result = _context.Tasks.Find(4);

            // Assert
            Assert.AreEqual("Updated Task", result.Name);
        }
        
        [Test]
        public async Task DeleteAsync_DeletesTask()
        {
            // Arrange
            var taskToDelete = new Models.Task { Id = 5, Name = "Delete Me" };
            _context.Tasks.Add(taskToDelete);
            await _context.SaveChangesAsync();

            // Act
            await _repository.DeleteAsync(5);
            var deletedTask = await _context.Tasks.FindAsync(5);

            // Assert
            Assert.IsNull(deletedTask);
        }
        
        
        [Test]
        public async Task ExistsAsync_ReturnsTrueForExistingTask()
        {
            // Arrange
            var existingTask = new Models.Task { Id = 6, Name = "Existing Task" };
            _context.Tasks.Add(existingTask);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.ExistsAsync(6);

            // Assert
            Assert.IsTrue(result);
        }
        
        
        [Test]
        public async Task GetByUserIdAsync_ReturnsTasksForUser()
        {
            // Arrange
            var userId = 1;
            _context.Tasks.AddRange(
                new Models.Task { Id = 7, UserId = userId, Name = "User Task 1" },
                new Models.Task { Id = 8, UserId = userId, Name = "User Task 2" },
                new Models.Task { Id = 9, UserId = 2, Name = "Other User Task" }
            );
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByUserIdAsync(userId);

            // Assert
            Assert.AreEqual(2, result.Count());
        }







        

    }
}