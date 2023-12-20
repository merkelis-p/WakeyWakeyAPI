using NUnit.Framework;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WakeyWakeyAPI;
using WakeyWakeyAPI.Models;
using System.Net.Http;
using Task = System.Threading.Tasks.Task;
using System.Text;
using Newtonsoft.Json;
using System.Net;
using System.Linq;
using System;

namespace WakeyWakeyAPI.Tests.IntegrationTests
{
    [TestFixture]
    public class UserControllerIntegrationTests
    {
        private WebApplicationFactory<Startup> _factory;
        private HttpClient _client;

        [SetUp]
        public void Setup()
        {
            _factory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var descriptor = services.SingleOrDefault(
                            d => d.ServiceType == typeof(DbContextOptions<wakeyContext>));

                        if (descriptor != null)
                        {
                            services.Remove(descriptor);
                        }

                        services.AddDbContext<wakeyContext>(options =>
                        {
                            options.UseInMemoryDatabase("WakeyWakeyTestDb");
                        });
                    });
                });

            _client = _factory.CreateClient();
        }

        [Test]
        public async Task Register_User_ReturnsCreatedResponse()
        {
            var user = new UserRegisterRequest
            {
                Username = "testuser" + Guid.NewGuid().ToString(),
                Email = "testuser" + Guid.NewGuid().ToString() + "@example.com",
                Password = "Test@123"
            };

            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/User/Register", content);

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }

        [Test]
        public async Task Login_ValidUser_ReturnsSuccess()
        {
            var user = new UserRegisterRequest
            {
                Username = "testuser",
                Email = "testuser@example.com",
                Password = "Test@123"
            };

            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            await _client.PostAsync("/api/User/Register", content);

            var loginRequest = new UserLoginRequest
            {
                Username = "testuser",
                Password = "Test@123"
            };

            content = new StringContent(JsonConvert.SerializeObject(loginRequest), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/User/Login", content);
            var result = JsonConvert.DeserializeObject<LoginValidationResult>(await response.Content.ReadAsStringAsync());

            Assert.IsTrue(result.IsValid);
        }

        [TearDown]
        public void TearDown()
        {
            _client.Dispose();
            _factory.Dispose();
        }
    }
}