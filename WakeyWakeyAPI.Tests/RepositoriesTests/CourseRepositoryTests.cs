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
    public class CourseRepositoryTests
    {
        private ICourseRepository _repository;
        private wakeyContext _context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<wakeyContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new wakeyContext(options);
            _repository = new CourseRepository(_context, new Mock<ILogger<CourseRepository>>().Object);

            // Set up test data with hierarchy
            var testCourse = new Course { Id = 100, Name = "Sample Course", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(30), UserId = 1 };
            var testSubject = new Subject { Id = 200, Name = "Sample Subject", CourseId = 100 };
            var testTask = new Models.Task { Id = 300, Name = "Sample Task", SubjectId = 200, ParentId = null };

            _context.Courses.Add(testCourse);
            _context.Subjects.Add(testSubject);
            _context.Tasks.Add(testTask);
            _context.SaveChanges();
        }

        [Test]
        public async Task GetAllAsync_ReturnsAllCourses()
        {
            var result = await _repository.GetAllAsync();
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("Sample Course", result.First().Name);
        }

        [Test]
        public async Task GetByIdAsync_ReturnsCorrectCourse()
        {
            var courseId = 100;
            var result = await _repository.GetByIdAsync(courseId);
            Assert.IsNotNull(result);
            Assert.AreEqual("Sample Course", result.Name);
        }

        [Test]
        public async Task GetByUserIdAsync_ReturnsCoursesForUser()
        {
            var userId = 1;
            var result = await _repository.GetByUserIdAsync(userId);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("Sample Course", result.First().Name);
        }


        [Test]
        public async Task AddAsync_AddsCourse()
        {
            var newCourse = new Course { Id = 101, Name = "New Course", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(30), UserId = 1 };
            await _repository.AddAsync(newCourse);
            var addedCourse = await _context.Courses.FindAsync(101);
            Assert.IsNotNull(addedCourse);
            Assert.AreEqual("New Course", addedCourse.Name);
        }

        [Test]
        public void Update_UpdatesCourse()
        {
            var existingCourse = new Course { Id = 102, Name = "Old Course", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(30), UserId = 1 };
            _context.Courses.Add(existingCourse);
            _context.SaveChanges();

            var updatedCourse = new Course { Id = 102, Name = "Updated Course", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(30), UserId = 1 };
            _repository.Update(updatedCourse);
            var result = _context.Courses.Find(102);
            Assert.AreEqual("Updated Course", result.Name);
        }

        [Test]
        public async Task DeleteAsync_DeletesCourse()
        {
            var courseToDelete = new Course { Id = 103, Name = "Delete Me", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(30), UserId = 1 };
            _context.Courses.Add(courseToDelete);
            await _context.SaveChangesAsync();

            await _repository.DeleteAsync(103);
            var deletedCourse = await _context.Courses.FindAsync(103);
            Assert.IsNull(deletedCourse);
        }

        [Test]
        public async Task ExistsAsync_ReturnsTrueForExistingCourse()
        {
            var existingCourse = new Course { Id = 104, Name = "Existing Course", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(30), UserId = 1 };
            _context.Courses.Add(existingCourse);
            await _context.SaveChangesAsync();

            var result = await _repository.ExistsAsync(104);
            Assert.IsTrue(result);
        }
        
        [Test]
        public async Task GetAllHierarchyAsync_ReturnsCoursesWithHierarchy()
        {
            var userId = 1;
            var result = await _repository.GetAllHierarchyAsync(userId);

            Assert.AreEqual(1, result.Count());
            var course = result.First();
            Assert.AreEqual("Sample Course", course.Name);
            Assert.AreEqual(1, course.Subjects.Count);
            var subject = course.Subjects.First();
            Assert.AreEqual("Sample Subject", subject.Name);
            Assert.AreEqual(1, subject.Tasks.Count);
            var task = subject.Tasks.First();
            Assert.AreEqual("Sample Task", task.Name);
        }

        [Test]
        public async Task GetSubTasksAsync_ReturnsSubTasksForTask()
        {
            var parentTaskId = 300;
            var subTask = new Models.Task { Id = 301, Name = "Sub Task", SubjectId = 200, ParentId = parentTaskId };
            _context.Tasks.Add(subTask);
            _context.SaveChanges();

            var result = await _repository.GetSubTasksAsync(parentTaskId);

            Assert.AreEqual(1, result.Count);
            var fetchedSubTask = result.First();
            Assert.AreEqual("Sub Task", fetchedSubTask.Name);
            Assert.AreEqual(parentTaskId, fetchedSubTask.ParentId);
        }
        
        
    }
}
