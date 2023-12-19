using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using Task = System.Threading.Tasks.Task;
using WakeyWakeyAPI.Models;
using WakeyWakeyAPI.Repositories;

namespace WakeyWakeyAPI.Tests.RepositoriesTests
{
    [TestFixture]
    public class SubjectRepositoryTests
    {
        private SubjectRepository _subjectRepository;
        private wakeyContext _context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<wakeyContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new wakeyContext(options);
            var loggerMock = new Mock<ILogger<SubjectRepository>>();

            _subjectRepository = new SubjectRepository(_context, loggerMock.Object);

            // Seed data for testing
            SeedDatabase();
        }

        private void SeedDatabase()
        {
            var subjects = new[]
            {
                new Subject { Id = 1, Name = "Mathematics", CourseId = 1 },
                new Subject { Id = 2, Name = "Physics", CourseId = 1 },
                new Subject { Id = 3, Name = "Chemistry", CourseId = 2 }
            };

            _context.Subjects.AddRange(subjects);
            _context.SaveChanges();
        }

        [Test]
        public async Task GetSubjectsByCourse_ReturnsSubjectsForGivenCourseId()
        {
            // Arrange
            int courseId = 1;

            // Act
            var subjects = await _subjectRepository.GetSubjectsByCourse(courseId);

            // Assert
            Assert.AreEqual(2, subjects.Count());
            Assert.IsTrue(subjects.All(s => s.CourseId == courseId));
        }

        [Test]
        public async Task AddAsync_AddsSubjectSuccessfully()
        {
            // Arrange
            var newSubject = new Subject { Id = 4, Name = "Biology", CourseId = 3 };

            // Act
            await _subjectRepository.AddAsync(newSubject);
            var addedSubject = await _context.Subjects.FindAsync(4);

            // Assert
            Assert.IsNotNull(addedSubject);
            Assert.AreEqual("Biology", addedSubject.Name);
        }
        
        [Test]
        public async Task DeleteAsync_DeletesSubject()
        {
            // Arrange - assuming subject with Id 1 exists
            var subjectId = 1;

            // Act
            await _subjectRepository.DeleteAsync(subjectId);
            var deletedSubject = await _context.Subjects.FindAsync(subjectId);

            // Assert
            Assert.IsNull(deletedSubject);
        }
        
        
        [Test]
        public async Task GetByIdAsync_ReturnsCorrectSubject()
        {
            // Arrange - assuming subject with Id 1 exists
            var subjectId = 1;

            // Act
            var subject = await _subjectRepository.GetByIdAsync(subjectId);

            // Assert
            Assert.IsNotNull(subject);
            Assert.AreEqual(subjectId, subject.Id);
        }
        
        [Test]
        public async Task GetAllAsync_ReturnsAllSubjects()
        {
            // Act
            var subjects = await _subjectRepository.GetAllAsync();

            // Assert
            Assert.AreEqual(3, subjects.Count()); // Number of seeded subjects
        }
        
        [Test]
        public void Update_UpdatesSubject()
        {
            // Arrange
            var subjectToUpdate = new Subject { Id = 2, Name = "Updated Physics", CourseId = 1 };

            // Act
            _subjectRepository.Update(subjectToUpdate);
            var updatedSubject = _context.Subjects.Find(2);

            // Assert
            Assert.AreEqual("Updated Physics", updatedSubject.Name);
        }
        
        [Test]
        public async Task ExistsAsync_ReturnsTrueForExistingSubject()
        {
            // Arrange - assuming subject with Id 1 exists
            var subjectId = 1;

            // Act
            var exists = await _subjectRepository.ExistsAsync(subjectId);

            // Assert
            Assert.IsTrue(exists);
        }

        [Test]
        public async Task ExistsAsync_ReturnsFalseForNonExistingSubject()
        {
            // Arrange
            var nonExistingSubjectId = 999;

            // Act
            var exists = await _subjectRepository.ExistsAsync(nonExistingSubjectId);

            // Assert
            Assert.IsFalse(exists);
        }
        

    }
    
    
}