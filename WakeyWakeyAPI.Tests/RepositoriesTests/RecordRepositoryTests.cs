using System;
using System.Linq;
using NUnit.Framework;
using Moq;
using Microsoft.EntityFrameworkCore;
using WakeyWakeyAPI.Models;
using WakeyWakeyAPI.Repositories;
using Microsoft.Extensions.Logging;
using Task = System.Threading.Tasks;

namespace WakeyWakeyAPI.Tests.RepositoriesTests
{
    [TestFixture]
    public class RecordRepositoryTests
    {
        private RecordRepository _repository;
        private wakeyContext _context;
        private Record _record;

        
        [SetUp]
        public void Setup()
        {
            
            _record = new Record();
            var options = new DbContextOptionsBuilder<wakeyContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new wakeyContext(options);
            _repository = new RecordRepository(_context, new Mock<ILogger<RecordRepository>>().Object);

            // Adding some test records
            var testRecord1 = new Record { Id = 1, UserId = 1, TaskId = 100, Note = "Test Record 1" };
            var testRecord2 = new Record { Id = 2, UserId = 2, TaskId = 101, Note = "Test Record 2" };
            _context.Records.AddRange(testRecord1, testRecord2);
            _context.SaveChanges();
        }
        
        [Test]
        public async Task.Task AddAsync_AddsRecord()
        {
            // Arrange
            var newRecord = new Record { Id = 3, UserId = 1, TaskId = 100, Note = "New Record" };

            // Act
            await _repository.AddAsync(newRecord);
            var addedRecord = await _context.Records.FindAsync(3);

            // Assert
            Assert.IsNotNull(addedRecord);
            Assert.AreEqual("New Record", addedRecord.Note);
            // No return statement needed
        }

        [Test]
        public async Task.Task GetByIdAsync_ReturnsCorrectRecord()
        {
            // Act
            var result = await _repository.GetByIdAsync(1); // Assuming ID 1 exists from the setup

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("Test Record 1", result.Note);
        }

        [Test]
        public async Task.Task DeleteAsync_DeletesRecord()
        {
            // Arrange - Ensure a record exists
            var recordToDelete = new Record { Id = 4, UserId = 1, TaskId = 100, Note = "Delete Me" };
            _context.Records.Add(recordToDelete);
            await _context.SaveChangesAsync();

            // Act
            await _repository.DeleteAsync(4);
            var deletedRecord = await _context.Records.FindAsync(4);

            // Assert
            Assert.IsNull(deletedRecord);
        }

        
        [Test]
        public void Update_UpdatesRecord()
        {
            // Arrange
            var existingRecord = new Record { Id = 5, UserId = 1, TaskId = 100, Note = "Old Note" };
            _context.Records.Add(existingRecord);
            _context.SaveChanges();

            var updatedRecord = new Record { Id = 5, UserId = 1, TaskId = 100, Note = "Updated Note" };

            // Act
            _repository.Update(updatedRecord);
            var result = _context.Records.Find(5);

            // Assert
            Assert.AreEqual("Updated Note", result.Note);
        }
        
        
        [Test]
        public async Task.Task GetAllAsync_ReturnsAllRecords()
        {
            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count()); // Assuming there are 2 records added in Setup
        }
        
        
        [Test]
        public async Task.Task ExistsAsync_ReturnsTrueForExistingRecord()
        {
            // Act
            var exists = await _repository.ExistsAsync(1); // Assuming ID 1 exists

            // Assert
            Assert.IsTrue(exists);
        }

        [Test]
        public async Task.Task ExistsAsync_ReturnsFalseForNonExistingRecord()
        {
            // Act
            var exists = await _repository.ExistsAsync(999); // Assuming ID 999 does not exist

            // Assert
            Assert.IsFalse(exists);
        }

        
        [Test]
        public async Task.Task DeleteAsync_DoesNothingForNonExistingRecord()
        {
            // Act
            await _repository.DeleteAsync(999); // Assuming ID 999 does not exist
            var result = await _context.Records.FindAsync(999);

            // Assert
            Assert.IsNull(result);
        }
        
        [Test]
        public void Update_NonExistingRecordDoesNotThrow()
        {
            // Arrange
            var nonExistingRecord = new Record { Id = 999, UserId = 1, TaskId = 100, Note = "Non-Existing Record" };

            // Act & Assert
            Assert.DoesNotThrow(() => _repository.Update(nonExistingRecord));
        }

        
        [Test]
        public async Task.Task AddAsync_ThrowsExceptionForDuplicateId()
        {
            // Arrange - Using a fresh context to avoid tracking issues
            var options = new DbContextOptionsBuilder<wakeyContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var newContext = new wakeyContext(options))
            {
                var newRepository = new RecordRepository(newContext, new Mock<ILogger<RecordRepository>>().Object);
                var duplicateRecord = new Record { Id = 1, UserId = 1, TaskId = 100, Note = "Duplicate Record" };

                // Add the duplicate record to the new context
                newContext.Records.Add(duplicateRecord);
                await newContext.SaveChangesAsync();

                // Act & Assert - Expecting ArgumentException due to duplicate ID
                Assert.ThrowsAsync<System.ArgumentException>(() => newRepository.AddAsync(duplicateRecord));
            }
        }

        
           
        [Test]
        public void AssigningProperties_StoresCorrectValues()
        {
            // Arrange
            var testDate = DateTime.Now;

            // Act
            _record.Id = 1;
            _record.Category = 2;
            _record.UserId = 3;
            _record.TaskId = 4;
            _record.Note = "Test Note";
            _record.StartDate = testDate;
            _record.EndDate = testDate.AddDays(1);
            _record.Duration = 60;
            _record.FocusDuration = 40;
            _record.BreakDuration = 20;
            _record.BreakFrequency = 10;

            // Assert
            Assert.AreEqual(1, _record.Id);
            Assert.AreEqual(2, _record.Category);
            Assert.AreEqual(3, _record.UserId);
            Assert.AreEqual(4, _record.TaskId);
            Assert.AreEqual("Test Note", _record.Note);
            Assert.AreEqual(testDate, _record.StartDate);
            Assert.AreEqual(testDate.AddDays(1), _record.EndDate);
            Assert.AreEqual(60, _record.Duration);
            Assert.AreEqual(40, _record.FocusDuration);
            Assert.AreEqual(20, _record.BreakDuration);
            Assert.AreEqual(10, _record.BreakFrequency);
        }

        [Test]
        public void Record_WithMissingRequiredFields_IsInvalid()
        {
            // Arrange
            // Assuming Category is a required field
            _record.Category = 0; // Invalid or missing value

            // Act & Assert
            // Add validation logic here (e.g., using FluentValidation or DataAnnotations) and assert invalidity
        }

        [Test]
        public void NavigationProperties_WhenSet_ReturnsCorrectData()
        {
            // Arrange
            var user = new User { /* Initialize properties */ };
            var task = new Models.Task { /* Initialize properties */ };

            // Act
            _record.User = user;
            _record.Task = task;

            // Assert
            Assert.AreEqual(user, _record.User);
            Assert.AreEqual(task, _record.Task);
        }
        
        
      


    }
}
