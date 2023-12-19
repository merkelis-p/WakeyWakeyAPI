using System;
using NUnit.Framework;
using Moq;
using Microsoft.EntityFrameworkCore;
using WakeyWakeyAPI.Models;
using WakeyWakeyAPI.Repositories;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Linq;
using Task = System.Threading.Tasks.Task;

namespace WakeyWakeyAPI.Tests.RepositoriesTests
{
    [TestFixture]
    public class EventRepositoryTests
    {
        private EventRepository _repository;
        private wakeyContext _context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<wakeyContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new wakeyContext(options);
            _repository = new EventRepository(_context, new Mock<ILogger<EventRepository>>().Object);

            // Adding an event with a known ID
            var testEvent = new Event { Id = 100, Name = "Sample Event", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1), UserId = 1 };
            _context.Events.Add(testEvent);
            _context.SaveChanges();
        }

        [Test]
        public async Task GetByIdAsync_ReturnsCorrectEvent()
        {
            // Act
            var result = await _repository.GetByIdAsync(100); // Use the same ID as added in the setup

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(100, result.Id);
            Assert.AreEqual("Sample Event", result.Name);
        }

        [Test]
        public async Task GetAllAsync_ReturnsAllEvents()
        {
            // Arrange - Adding more unique events
            _context.Events.Add(new Event { Id = Guid.NewGuid().GetHashCode(), Name = "Event 2", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1), UserId = 1 });
            _context.Events.Add(new Event { Id = Guid.NewGuid().GetHashCode(), Name = "Event 3", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1), UserId = 1 });
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.AreEqual(3, result.Count()); // Assuming one event was added in the setup
        }

        [Test]
        public async Task AddAsync_AddsEvent()
        {
            // Arrange
            var newEvent = new Event { Id = 3, Name = "New Event", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1), UserId = 1 };

            // Act
            await _repository.AddAsync(newEvent);
            var addedEvent = await _context.Events.FindAsync(3);

            // Assert
            Assert.IsNotNull(addedEvent);
            Assert.AreEqual("New Event", addedEvent.Name);
        }

        [Test]
        public void Update_UpdatesEvent()
        {
            // Arrange
            var existingEvent = new Event { Id = 4, Name = "Old Event", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1), UserId = 1 };
            _context.Events.Add(existingEvent);
            _context.SaveChanges();

            var updatedEvent = new Event { Id = 4, Name = "Updated Event", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1), UserId = 1 };

            // Act
            _repository.Update(updatedEvent);
            var result = _context.Events.Find(4);

            // Assert
            Assert.AreEqual("Updated Event", result.Name);
        }

        [Test]
        public async Task DeleteAsync_DeletesEvent()
        {
            // Arrange
            var eventToDelete = new Event { Id = 5, Name = "Delete Me", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1), UserId = 1 };
            _context.Events.Add(eventToDelete);
            await _context.SaveChangesAsync();

            // Act
            await _repository.DeleteAsync(5);
            var deletedEvent = await _context.Events.FindAsync(5);

            // Assert
            Assert.IsNull(deletedEvent);
        }

        [Test]
        public async Task ExistsAsync_ReturnsTrueForExistingEvent()
        {
            // Arrange
            var existingEvent = new Event { Id = 6, Name = "Existing Event", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1), UserId = 1 };
            _context.Events.Add(existingEvent);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.ExistsAsync(6);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task GetByUserIdAsync_ReturnsEventsForUser()
        {
            // Arrange
            var userId = 1;
            _context.Events.AddRange(
                new Event { Id = 7, UserId = userId, Name = "User Event 1", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1) },
                new Event { Id = 8, UserId = userId, Name = "User Event 2", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1) },
                new Event { Id = 9, UserId = 2, Name = "Other User Event", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1) }
            );
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByUserIdAsync(userId);

            // Assert
            Assert.AreEqual(2, result.Count());
        }
    }
}
