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
        
        
        [Test]
        public async Task GetBySubjectIdAsync_ReturnsTasksForSubject()
        {
            // Arrange
            var subjectId = 101;
            _context.Tasks.AddRange(
                new Models.Task { Id = Guid.NewGuid().GetHashCode(), SubjectId = subjectId, Name = "Subject Task 1" },
                new Models.Task { Id = Guid.NewGuid().GetHashCode(), SubjectId = subjectId, Name = "Subject Task 2" },
                new Models.Task { Id = Guid.NewGuid().GetHashCode(), SubjectId = 102, Name = "Other Subject Task" }
            );
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetBySubjectIdAsync(subjectId);

            // Assert
            Assert.AreEqual(2, result.Count());
        }
        
        
        [Test]
        public async Task GetChildrenByParentIdAsync_ReturnsChildTasks()
        {
            // Arrange
            var parentId = 103;
            _context.Tasks.AddRange(
                new Models.Task { Id = Guid.NewGuid().GetHashCode(), ParentId = parentId, Name = "Child Task 1" },
                new Models.Task { Id = Guid.NewGuid().GetHashCode(), ParentId = parentId, Name = "Child Task 2" },
                new Models.Task { Id = Guid.NewGuid().GetHashCode(), ParentId = 104, Name = "Other Child Task" }
            );
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetChildrenByParentIdAsync(parentId);

            // Assert
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public async Task GetTasksWithHierarchyBySubjectId_ReturnsHierarchicalTasks()
        {
            // Arrange
            var subjectId = 105;
            var parentId = Guid.NewGuid().GetHashCode();
            _context.Tasks.AddRange(
                new Models.Task { Id = parentId, SubjectId = subjectId, Name = "Parent Task" },
                new Models.Task { Id = Guid.NewGuid().GetHashCode(), ParentId = parentId, Name = "Child Task" },
                new Models.Task { Id = Guid.NewGuid().GetHashCode(), SubjectId = subjectId, Name = "Independent Task" }
            );
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetTasksWithHierarchyBySubjectId(subjectId);

            // Assert
            Assert.IsTrue(result.Any(t => t.SubTasks != null && t.SubTasks.Any()));
        }
        
        [Test]
        public async Task GetIndependentTasksWithHierarchy_ReturnsIndependentHierarchicalTasks()
        {
            // Arrange
            var subjectId = 106;
            var parentId = Guid.NewGuid().GetHashCode();
            _context.Tasks.AddRange(
                new Models.Task { Id = parentId, SubjectId = subjectId, Name = "Independent Parent Task" },
                new Models.Task { Id = Guid.NewGuid().GetHashCode(), ParentId = parentId, Name = "Independent Child Task" }
            );
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetIndependentTasksWithHierarchy(subjectId);

            // Assert
            Assert.IsTrue(result.Any(t => t.ParentId == null && t.SubTasks != null && t.SubTasks.Any()));
        }
        
        
        [Test]
        public async Task GetTasksWithHierarchyByUserId_ReturnsHierarchicalTasksForUser()
        {
            // Arrange
            var userId = 107;
            var parentId = Guid.NewGuid().GetHashCode();
            _context.Tasks.AddRange(
                new Models.Task { Id = parentId, UserId = userId, Name = "User Parent Task" },
                new Models.Task { Id = Guid.NewGuid().GetHashCode(), ParentId = parentId, UserId = userId, Name = "User Child Task" }
            );
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetTasksWithHierarchyByUserId(userId);

            // Assert
            Assert.IsTrue(result.Any(t => t.UserId == userId && t.SubTasks != null && t.SubTasks.Any()));
        }
        
        
        [Test]
        public async Task GetTasksWithHierarchy_ReturnsAllHierarchicalTasks()
        {
            // Arrange
            var parentId = Guid.NewGuid().GetHashCode();
            _context.Tasks.AddRange(
                new Models.Task { Id = parentId, Name = "General Parent Task" },
                new Models.Task { Id = Guid.NewGuid().GetHashCode(), ParentId = parentId, Name = "General Child Task" }
            );
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetTasksWithHierarchy();

            // Assert
            Assert.IsTrue(result.Any(t => t.ParentId == null && t.SubTasks != null && t.SubTasks.Any()));
        }
        
        
        
        [Test]
        public async Task AddTaskAsync_CreatesNewTask()
        {
            // Arrange
            var taskCreateRequest = new TaskCreateRequest
            {
                Name = "Newly Added Task",
                // Add other required fields as per TaskCreateRequest structure
            };

            // Act
            var addedTask = await _repository.AddTaskAsync(taskCreateRequest);
            var retrievedTask = await _context.Tasks.FindAsync(addedTask.Id);

            // Assert
            Assert.IsNotNull(retrievedTask);
            Assert.AreEqual("Newly Added Task", retrievedTask.Name);
        }
        
        
        [Test]
        public async Task GetByUserIdAsync_ReturnsEmptyWhenNoTasksForUser()
        {
            // Arrange
            var nonExistentUserId = 999; // Use an ID that's not present in the database

            // Act
            var result = await _repository.GetByUserIdAsync(nonExistentUserId);

            // Assert
            Assert.IsEmpty(result);
        }
        
        
        [Test]
        public async Task DeleteAsync_NoEffectForNonExistentTask()
        {
            // Arrange
            var nonExistentTaskId = 999; // Use an ID that's not present in the database

            // Act
            await _repository.DeleteAsync(nonExistentTaskId);
            var result = await _context.Tasks.FindAsync(nonExistentTaskId);

            // Assert
            Assert.IsNull(result);
        }
        
        [Test]
        public void Update_NoEffectForNonExistentTask()
        {
            // Arrange
            var nonExistentTask = new Models.Task { Id = 999, Name = "Non-Existent Task" };

            // Act
            _repository.Update(nonExistentTask); // This should not throw an exception
            var result = _context.Tasks.Find(999);

            // Assert
            Assert.IsNull(result); // No new entity should be added
            // Optionally, verify that a warning was logged
        }

        
        [Test]
        public async Task ExistsAsync_ReturnsFalseForNonExistentTask()
        {
            // Arrange
            var nonExistentTaskId = 999; // Use an ID that's not present in the database

            // Act
            var result = await _repository.ExistsAsync(nonExistentTaskId);

            // Assert
            Assert.IsFalse(result);
        }
        
        
        
        [Test]
        public async Task GetChildrenByParentIdAsync_ReturnsEmptyForNonExistentParent()
        {
            // Arrange
            var nonExistentParentId = 999; // Use an ID that's not present in the database

            // Act
            var result = await _repository.GetChildrenByParentIdAsync(nonExistentParentId);

            // Assert
            Assert.IsEmpty(result);
        }

        
   








        
        



    }
}