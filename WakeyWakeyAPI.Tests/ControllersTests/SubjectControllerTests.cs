using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Task = System.Threading.Tasks.Task;
using WakeyWakeyAPI.Controllers;
using WakeyWakeyAPI.Models;
using WakeyWakeyAPI.Repositories;


namespace WakeyWakeyAPI.Tests.ControllersTests
{
    [TestFixture]
    public class SubjectControllerTests
    {
        private Mock<ISubjectRepository> _mockRepository;
        private Mock<ILogger<SubjectController>> _mockLogger;
        private SubjectController _subjectController;

        [SetUp]
        public void SetUp()
        {
            _mockRepository = new Mock<ISubjectRepository>();
            _mockLogger = new Mock<ILogger<SubjectController>>();
            _subjectController = new SubjectController(_mockRepository.Object);
        }

        [Test]
        public async Task GetSubjectsByCourse_ValidCourseId_ReturnsOkResult()
        {
            // Arrange
            var courseId = 1;
            var subjectsList = new List<Subject>
            {
                new Subject { Id = 1, Name = "Subject 1", Status = 1 },
                new Subject { Id = 2, Name = "Subject 2", Status = 0 }
            };

            _mockRepository.Setup(c => c.GetSubjectsByCourse(courseId)).ReturnsAsync(subjectsList);

            // Act
            var result = await _subjectController.GetSubjectsByCourse(courseId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = (OkObjectResult)result.Result;
            Assert.IsInstanceOf<IEnumerable<Subject>>(okResult.Value);
            var returnedSubjects = (IEnumerable<Subject>)okResult.Value;
            Assert.AreEqual(subjectsList.Count, returnedSubjects.Count());
        }

        [Test]
        public async Task GetSubjectsByCourse_InvalidCourseId_ReturnsNotFoundResult()
        {
            // Arrange
            var courseId = 999;
            _mockRepository.Setup(c => c.GetSubjectsByCourse(courseId)).ReturnsAsync(new List<Subject>());

            // Act
            var result = await _subjectController.GetSubjectsByCourse(courseId);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }
    }
}
