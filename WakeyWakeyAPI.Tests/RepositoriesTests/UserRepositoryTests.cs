using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using WakeyWakeyAPI.Models;
using WakeyWakeyAPI.Repositories;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using Task = System.Threading.Tasks.Task;

namespace WakeyWakeyAPI.Tests.RepositoriesTests
{
    public class UserRepositoryTests
    {
        private IUserRepository _userRepository;
        private wakeyContext _context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<wakeyContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new wakeyContext(options);
            _userRepository = new UserRepository(_context, new NullLogger<UserRepository>());

            // Seed the database with test data
            _context.Users.Add(new User { Id = 1, Username = "testUser1", Email = "test1@example.com", Password = "Password1" });
            _context.Users.Add(new User { Id = 2, Username = "testUser2", Email = "test2@example.com", Password = "Password2" });
            _context.SaveChanges();
        }

        [Test]
        public async Task AddAsync_AddsUserSuccessfully()
        {
            // Arrange
            var newUser = new User { Id = 3, Username = "newUser", Email = "newUser@example.com", Password = "Password123" };

            // Act
            await _userRepository.AddAsync(newUser);
            var addedUser = await _context.Users.FindAsync(3);

            // Assert
            Assert.IsNotNull(addedUser);
            Assert.AreEqual("newUser", addedUser.Username);
        }
        
        [Test]
        public async Task GetByUsernameAsync_ReturnsCorrectUser()
        {
            // Act
            var user = await _userRepository.GetByUsernameAsync("testUser1");

            // Assert
            Assert.IsNotNull(user);
            Assert.AreEqual("testUser1", user.Username);
        }
        
        [Test]
        public async Task ExistsAsync_ReturnsTrueForExistingUsername()
        {
            // Act
            var exists = await _userRepository.ExistsAsync("testUser1");

            // Assert
            Assert.IsTrue(exists);
        }

        [Test]
        public async Task ExistsAsync_ReturnsFalseForNonExistingUsername()
        {
            // Act
            var exists = await _userRepository.ExistsAsync("nonExistingUser");

            // Assert
            Assert.IsFalse(exists);
        }
        
        [Test]
        public async Task ExistsEmailAsync_ReturnsTrueForExistingEmail()
        {
            // Act
            var exists = await _userRepository.ExistsEmailAsync("test1@example.com");

            // Assert
            Assert.IsTrue(exists);
        }

        [Test]
        public async Task ExistsEmailAsync_ReturnsFalseForNonExistingEmail()
        {
            // Act
            var exists = await _userRepository.ExistsEmailAsync("nonExistingEmail@example.com");

            // Assert
            Assert.IsFalse(exists);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
        }
    }
}
