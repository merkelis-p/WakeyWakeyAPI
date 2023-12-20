using System;
using NUnit.Framework;
using Moq;
using WakeyWakeyAPI.Controllers;
using WakeyWakeyAPI.Models;
using WakeyWakeyAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Task = System.Threading.Tasks.Task;

namespace WakeyWakeyAPI.Tests.ControllersTests
{
    [TestFixture]
    public class CourseControllerTests
    {
        private Mock<ICourseRepository> _mockRepository;
        private CourseController _controller;

        private List<Course> ExpectedCourses()
        {
            return new List<Course>
            {
                new Course
                {
                    Id = 1,
                    Name = "Introduction to Programming",
                    Description = "A beginner's course on programming concepts",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(30),
                    Status = 1, 
                    Score = 85,
                    UserId = 1,
                    Subjects = new List<Subject>() 
                },
                new Course
                {
                    Id = 2,
                    Name = "Advanced Databases",
                    Description = "A course on advanced database techniques",
                    StartDate = DateTime.Now.AddDays(-10),
                    EndDate = DateTime.Now.AddDays(20),
                    Status = 1, 
                    Score = 90,
                    UserId = 2,
                    Subjects = new List<Subject>() 
                }
            };
        }


        [SetUp]
        public void Setup()
        {
            var sampleCourses = ExpectedCourses();
            _mockRepository = new Mock<ICourseRepository>();

            _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(sampleCourses);

            _controller = new CourseController(_mockRepository.Object);
        }

        [Test]
        public async Task Create_CreatesNewCourseAndReturnsCreatedResult()
        {
            // Arrange
            var courseCreateRequest = new Course
            {
                // Initialize with required data
            };
            var createdCourse = new Course
            {
                // Initialize with expected created course data
            };

            _mockRepository.Setup(repo => repo.AddAsync(It.IsAny<Course>()))
                .Callback<Course>(course => course.Id = 3) // Or whatever logic you need in the callback
                .Returns(Task.CompletedTask);


            // Act
            var actionResult = await _controller.Create(courseCreateRequest);

            // Assert
            Assert.IsInstanceOf<CreatedAtActionResult>(actionResult.Result);
            var createdAtResult = actionResult.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdAtResult);
            var course = createdAtResult.Value as Course;
            Assert.IsNotNull(course);
            // Further assertions based on expected values
        }

        [Test]
        public async Task GetByUserId_ReturnsCoursesForValidUserId()
        {
            // Arrange
            var userId = 1; // Example user ID
            var expectedCourses = ExpectedCourses().Where(c => c.UserId == userId).ToList();
            _mockRepository.Setup(repo => repo.GetByUserIdAsync(userId)).ReturnsAsync(expectedCourses);

            // Act
            var actionResult = await _controller.GetByUserId(userId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(actionResult.Result);
            var okResult = actionResult.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var courses = okResult.Value as List<Course>;
            Assert.IsNotNull(courses);
            Assert.AreEqual(expectedCourses.Count, courses.Count);
        }
        
        
        [Test]
        public async Task Update_WithValidIdAndEntity_UpdatesEntity()
        {
            var courseId = 1;
            var courseToUpdate = new Course { Id = courseId, Name = "Updated Course" };

            _mockRepository.Setup(repo => repo.ExistsAsync(courseId)).ReturnsAsync(true);
            _mockRepository.Setup(repo => repo.Update(courseToUpdate)); // Assuming Update method is void

            var actionResult = await _controller.Update(courseId, courseToUpdate);

            Assert.IsInstanceOf<NoContentResult>(actionResult);
        }

        [Test]
        public async Task Update_WithIdMismatch_ReturnsBadRequest()
        {
            var courseId = 1;
            var courseToUpdate = new Course { Id = 2, Name = "Updated Course" };

            var actionResult = await _controller.Update(courseId, courseToUpdate);

            Assert.IsInstanceOf<BadRequestObjectResult>(actionResult);
        }

        [Test]
        public async Task Update_WithNonExistingId_ReturnsNotFound()
        {
            var courseId = 1;
            var courseToUpdate = new Course { Id = courseId, Name = "Updated Course" };

            _mockRepository.Setup(repo => repo.ExistsAsync(courseId)).ReturnsAsync(false);

            var actionResult = await _controller.Update(courseId, courseToUpdate);

            Assert.IsInstanceOf<NotFoundResult>(actionResult);
        }
        
        
        [Test]
        public async Task Delete_WithExistingId_DeletesEntity()
        {
            var courseId = 1;

            _mockRepository.Setup(repo => repo.ExistsAsync(courseId)).ReturnsAsync(true);
            _mockRepository.Setup(repo => repo.DeleteAsync(courseId)).Returns(Task.CompletedTask);

            var actionResult = await _controller.Delete(courseId);

            Assert.IsInstanceOf<NoContentResult>(actionResult);
        }

        [Test]
        public async Task Delete_WithNonExistingId_ReturnsNotFound()
        {
            var courseId = 1;

            _mockRepository.Setup(repo => repo.ExistsAsync(courseId)).ReturnsAsync(false);

            var actionResult = await _controller.Delete(courseId);

            Assert.IsInstanceOf<NotFoundResult>(actionResult);
        }
        
        
        [Test]
        public async Task GetAllHierarchy_WithValidId_ReturnsCourses()
        {
            var userId = 1;
            var expectedCourses = new List<Course>
            {
                new Course { Id = 1, Name = "Course 1", UserId = userId },
                new Course { Id = 2, Name = "Course 2", UserId = userId }
            };
            _mockRepository.Setup(repo => repo.GetAllHierarchyAsync(userId)).ReturnsAsync(expectedCourses);

            var actionResult = await _controller.GetAllHierarchy(userId);

            Assert.IsInstanceOf<OkObjectResult>(actionResult.Result);
            var okResult = actionResult.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var courses = okResult.Value as List<Course>;
            Assert.IsNotNull(courses);
            Assert.AreEqual(expectedCourses.Count, courses.Count);
        }


        [Test]
        public async Task GetAllHierarchy_WithNoCourses_ReturnsNotFound()
        {
            var userId = 1;
            _mockRepository.Setup(repo => repo.GetAllHierarchyAsync(userId)).ReturnsAsync(new List<Course>());

            var actionResult = await _controller.GetAllHierarchy(userId);

            Assert.IsInstanceOf<NotFoundResult>(actionResult.Result);
        }



        
        
        

        [TearDown]
        public void TearDown()
        {
            _controller = null;
        }
    }
}
