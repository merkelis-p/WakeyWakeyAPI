using System;
using NUnit.Framework;
using Moq;
using WakeyWakeyAPI.Controllers;
using WakeyWakeyAPI.Models;
using WakeyWakeyAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Task = System.Threading.Tasks.Task;

namespace WakeyWakeyAPI.Tests.ControllersTests
{
    [TestFixture]
    public class UserControllerTests
    {
        private Mock<IUserRepository> _mockRepository;
        private UserController _controller;

        private List<User> ExpectedUsers()
        {
            return new List<User>
            {
                new User
                {
                    Id = 1,
                    Username = "admin",
                    Email = "admin@wakey.com",
                    Password = "0Bej6EiFAII0Zepwe42UW5GRYOsN/RAk3CpgRuIvP6aG8Zpgzfua9VSexeuSygwV8izim42F+5NS/HNaCnrVJA==",
                    Salt = "PcN0dAF7rL0ZlLTOiNPfCy/FkMaC4ZeTFlfuCHKAWxCV/0ADZdw2/9loa5JUmIMN/f99CnXs2IsVPHrn62zrGhsfQrcpTgD8npyAMOFXH9AEqeCLfxWkM4aEqqWQxaEKYgT6mvfY6klOoPqzvRM8qrxJqR92cBMX5lbqC7RrjyE="
                },
                new User
                {
                    Id = 2,
                    Username = "user",
                    Email = "user@wakey.com",
                    Password = "IPb/zqY4jtO3PNRuqMS/Y12CxTTZHfJCNYi/2G9B4yw7+jzJlXFXx5hvbwCWWihK8Dbcme1zOIcDa2ivBNgFFA==",
                    Salt = "nEv9PcuXySfj6F3z8XYugvwdywBdpNtnLgPcA5dhbv3MFEQwqYSNuXK6rg9CDV764Dh3UncRgQ/S/ezEFJWQs7YNjwqkC3x/OV7fhmXCyUIyzDi7jUewtvT4aIRzDjgU3QdDppV3SWNg3/h4CvUpd7Uj7gWbbQyqzwk0uxheUqk=",
                }
            };
        }

        [SetUp]
        public void Setup()
        {
            // Prepare a list of sample users
            var sampleUsers = ExpectedUsers();

            // Mocking IUserRepository
            _mockRepository = new Mock<IUserRepository>();

            // Setup mock behavior for GetByIdAsync()
            _mockRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) => sampleUsers.FirstOrDefault(u => u.Id == id));

            // Creating the controller with the mocked repository
            _controller = new UserController(_mockRepository.Object);
        }
        
        
        [Test]
        public async Task ValidateLogin_WithCorrectCredentials_ReturnsValidResult()
        {
            // Arrange
            var expectedUser = ExpectedUsers().First(u => u.Username == "admin"); // Get the expected user
            _mockRepository.Setup(repo => repo.GetByUsernameAsync("admin")).ReturnsAsync(expectedUser);

            var loginRequest = new UserLoginRequest
            {
                Username = "admin",
                Password = "admin" 
            };

            // Hash the password as it would be in the actual application
            using var hmac = new HMACSHA512();
            var adminPasswordHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes("admin")));
            var adminSalt = Convert.ToBase64String(hmac.Key);
            Console.WriteLine($"Password Hash: {adminPasswordHash}");
            Console.WriteLine($"Salt: {adminSalt}");

            // Act
            var actionResult = await _controller.ValidateLogin(loginRequest);

            // Assert
            Assert.IsNotNull(actionResult, "Action result is null");
            Assert.IsInstanceOf<OkObjectResult>(actionResult.Result, "Result is not OkObjectResult");
            var okResult = actionResult.Result as OkObjectResult;
            var loginResult = okResult.Value as LoginValidationResult;
            Assert.IsTrue(loginResult.IsValid, "Login should be valid");
            Assert.AreEqual(expectedUser.Id, loginResult.UserId, "User ID does not match");
        }
        
        
        [Test]
        public async Task Register_WithUniqueUser_ReturnsCreatedUser()
        {
            // Arrange
            var registerRequest = new UserRegisterRequest
            {
                Username = "newuser",
                Password = "newpassword",
                Email = "newuser@wakey.com"
            };

            var newUser = new User
            {
                // Do not set Id here as it's supposed to be set by AddAsync
                Username = registerRequest.Username,
                Email = registerRequest.Email
                // Other necessary fields...
            };

            _mockRepository.Setup(repo => repo.ExistsAsync(It.IsAny<string>())).ReturnsAsync(false);
            _mockRepository.Setup(repo => repo.ExistsEmailAsync(It.IsAny<string>())).ReturnsAsync(false);

            // Mock the AddAsync method to simulate setting an ID on the newUser
            _mockRepository.Setup(repo => repo.AddAsync(It.IsAny<User>()))
                .Callback<User>(user => user.Id = 3)
                .Returns(Task.CompletedTask); // We are returning a completed task directly.

            
            // Act
            var actionResult = await _controller.Register(registerRequest);

            // Assert
            Assert.IsInstanceOf<CreatedAtActionResult>(actionResult.Result);
            var createdAtResult = actionResult.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdAtResult);
            var returnedUser = createdAtResult.Value as User;
            Assert.IsNotNull(returnedUser);
            Assert.AreEqual(3, returnedUser.Id); // Check if the ID is as expected
            // Other assertions for User properties
        }


        
        
        [Test]
        public async Task Register_WithExistingUsernameOrEmail_ReturnsBadRequest()
        {
            var registerRequest = new UserRegisterRequest
            {
                Username = "user",
                Password = "user",
                Email = "user@wakey.com"
            };

            _mockRepository.Setup(repo => repo.ExistsAsync(registerRequest.Username)).ReturnsAsync(true);
            _mockRepository.Setup(repo => repo.ExistsEmailAsync(registerRequest.Email)).ReturnsAsync(true);

            var actionResult = await _controller.Register(registerRequest);

            Assert.IsInstanceOf<BadRequestObjectResult>(actionResult.Result);
            var badRequestResult = actionResult.Result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            // Optionally check the content of the BadRequest (e.g., error message)
        }
        
        
        [Test]
        public async Task GetUserById_WithExistingAndNonExistingId_CoversSetupMethod()
        {
            // Arrange
            var existingUserId = 1; // Assuming this ID exists based on your ExpectedUsers method
            var nonExistingUserId = 99; // Assuming this ID does not exist

            // Act and Assert for existing user
            var existingUserResult = await _controller.GetById(existingUserId);
            Assert.IsInstanceOf<OkObjectResult>(existingUserResult.Result, "Existing user was not found when it should have been");

            // Act and Assert for non-existing user
            var nonExistingUserResult = await _controller.GetById(nonExistingUserId);
            Assert.IsInstanceOf<NotFoundResult>(nonExistingUserResult.Result, "Non-existing user was found when it should not have been");
        }


        [TearDown]
        public void TearDown()
        {
            _controller = null;
        }
    }
}
