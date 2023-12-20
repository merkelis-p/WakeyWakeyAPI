using NUnit.Framework;
using Moq;
using Microsoft.EntityFrameworkCore;
using WakeyWakeyAPI.Models;
using WakeyWakeyAPI.Repositories;
using Microsoft.Extensions.Logging;
using Task = System.Threading.Tasks.Task;
using System.Linq;
using System;

namespace WakeyWakeyAPI.Tests.RepositoriesTests
{
    [TestFixture]
    public class ReminderRepositoryTests
    {
        private ReminderRepository _repository;
        private wakeyContext _context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<wakeyContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new wakeyContext(options);
            _repository = new ReminderRepository(_context, new Mock<ILogger<ReminderRepository>>().Object);

            var testEvent = new Event { Id = 1, Name = "Test Event", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1), UserId = 1 };
            _context.Events.Add(testEvent);

            var testReminder = new Reminder { Id = 100, ReminderDate = DateTime.Now.AddDays(1), EventId = 1 };
            _context.Reminders.Add(testReminder);
            _context.SaveChanges();
        }

        [Test]
        public async Task GetAllAsync_ReturnsAllReminders()
        {
            var result = await _repository.GetAllAsync();
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(100, result.First().Id);
        }

        [Test]
        public async Task GetByIdAsync_ReturnsCorrectReminder()
        {
            var reminderId = 100;
            var result = await _repository.GetByIdAsync(reminderId);
            Assert.IsNotNull(result);
            Assert.AreEqual(reminderId, result.Id);
        }

        [Test]
        public async Task AddAsync_AddsReminder()
        {
            var newReminder = new Reminder { Id = 101, ReminderDate = DateTime.Now.AddDays(2), EventId = 1 };
            await _repository.AddAsync(newReminder);
            var addedReminder = await _context.Reminders.FindAsync(101);
            Assert.IsNotNull(addedReminder);
            Assert.AreEqual(newReminder.ReminderDate, addedReminder.ReminderDate);
        }

        [Test]
        public void Update_UpdatesReminder()
        {
            var existingReminder = new Reminder { Id = 102, ReminderDate = DateTime.Now.AddDays(3), EventId = 1 };
            _context.Reminders.Add(existingReminder);
            _context.SaveChanges();

            var updatedReminder = new Reminder { Id = 102, ReminderDate = DateTime.Now.AddDays(4), EventId = 1 };
            _repository.Update(updatedReminder);
            var result = _context.Reminders.Find(102);
            Assert.AreEqual(updatedReminder.ReminderDate, result.ReminderDate);
        }

        [Test]
        public async Task DeleteAsync_DeletesReminder()
        {
            var reminderToDelete = new Reminder { Id = 103, ReminderDate = DateTime.Now.AddDays(5), EventId = 1 };
            _context.Reminders.Add(reminderToDelete);
            await _context.SaveChangesAsync();

            await _repository.DeleteAsync(103);
            var deletedReminder = await _context.Reminders.FindAsync(103);
            Assert.IsNull(deletedReminder);
        }

        [Test]
        public async Task ExistsAsync_ReturnsTrueForExistingReminder()
        {
            var existingReminder = new Reminder { Id = 104, ReminderDate = DateTime.Now.AddDays(6), EventId = 1 };
            _context.Reminders.Add(existingReminder);
            await _context.SaveChangesAsync();

            var result = await _repository.ExistsAsync(104);
            Assert.IsTrue(result);
        }
    }
}
