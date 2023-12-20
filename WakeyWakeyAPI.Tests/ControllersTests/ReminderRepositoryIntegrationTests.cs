using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using WakeyWakeyAPI.Models;
using WakeyWakeyAPI.Repositories;
using System.Linq;
using Task = System.Threading.Tasks.Task;
using Microsoft.Extensions.Logging.Abstractions;

namespace WakeyWakeyAPI.Tests.IntegrationTests
{
    [TestFixture]
    public class ReminderRepositoryIntegrationTests
    {
        private ReminderRepository _repository;
        private wakeyContext _context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<wakeyContext>()
                .UseInMemoryDatabase(databaseName: "WakeyWakeyTestDb")
                .Options;

            _context = new wakeyContext(options);
            _repository = new ReminderRepository(_context, new NullLogger<ReminderRepository>());
        }

        [Test]
        public async Task AddAsync_AddsReminder()
        {
            var reminder = new Reminder { ReminderDate = System.DateTime.Now, EventId = 1 };
            await _repository.AddAsync(reminder);

            var reminders = await _context.Reminders.ToListAsync();
            Assert.AreEqual(1, reminders.Count);
            Assert.AreEqual(reminder.ReminderDate, reminders.First().ReminderDate);
        }

        [Test]
        public async Task GetAllAsync_ReturnsAllReminders()
        {
            var reminder1 = new Reminder { ReminderDate = System.DateTime.Now, EventId = 1 };
            var reminder2 = new Reminder { ReminderDate = System.DateTime.Now.AddDays(1), EventId = 1 };
            await _context.AddRangeAsync(reminder1, reminder2);
            await _context.SaveChangesAsync();

            var reminders = await _repository.GetAllAsync();
            Assert.AreEqual(2, reminders.Count());
        }

        [Test]
        public async Task GetByIdAsync_ReturnsCorrectReminder()
        {
            var reminder = new Reminder { ReminderDate = System.DateTime.Now, EventId = 1 };
            await _context.AddAsync(reminder);
            await _context.SaveChangesAsync();

            var fetchedReminder = await _repository.GetByIdAsync(reminder.Id);
            Assert.IsNotNull(fetchedReminder);
            Assert.AreEqual(reminder.ReminderDate, fetchedReminder.ReminderDate);
        }

        [Test]
        public async Task Update_UpdatesReminder()
        {
            var reminder = new Reminder { ReminderDate = System.DateTime.Now, EventId = 1 };
            await _context.AddAsync(reminder);
            await _context.SaveChangesAsync();

            reminder.ReminderDate = System.DateTime.Now.AddDays(2);
            _repository.Update(reminder);
            await _context.SaveChangesAsync();

            var updatedReminder = await _context.Reminders.FindAsync(reminder.Id);
            Assert.AreEqual(reminder.ReminderDate, updatedReminder.ReminderDate);
        }

        [Test]
        public async Task DeleteAsync_DeletesReminder()
        {
            var reminder = new Reminder { ReminderDate = System.DateTime.Now, EventId = 1 };
            await _context.AddAsync(reminder);
            await _context.SaveChangesAsync();

            await _repository.DeleteAsync(reminder.Id);
            var deletedReminder = await _context.Reminders.FindAsync(reminder.Id);
            Assert.IsNull(deletedReminder);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
        }
    }
}
