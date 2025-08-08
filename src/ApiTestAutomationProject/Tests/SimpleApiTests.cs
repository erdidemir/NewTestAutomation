using NUnit.Framework;
using ApiTestAutomationProject.Core;
using ApiTestAutomationProject.Models;
using FluentAssertions;
using Serilog;

namespace ApiTestAutomationProject.Tests
{
    [TestFixture]
    public class SimpleApiTests
    {
        private ApiClient _apiClient;

        [SetUp]
        public void Setup()
        {
            _apiClient = new ApiClient();
            Log.Information("Simple API tests setup completed");
        }

        [Test]
        public async Task GetUsers_ShouldReturnUserList()
        {
            // Act
            var response = await _apiClient.GetUsersAsync(1);

            // Assert
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            response.Data.Should().NotBeNull();
            response.Data.Data.Should().NotBeEmpty();
            
            Log.Information($"Retrieved {response.Data.Data.Count} users from page {response.Data.Page}");
        }

        [Test]
        public async Task Login_WithValidCredentials_ShouldReturnToken()
        {
            // Arrange
            var loginRequest = new LoginRequest
            {
                Email = "eve.holt@reqres.in",
                Password = "cityslicka"
            };

            // Act
            var response = await _apiClient.LoginAsync(loginRequest);

            // Assert
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            response.Data.Should().NotBeNull();
            response.Data.Token.Should().NotBeNullOrEmpty();
            
            Log.Information($"Login successful. Token: {response.Data.Token}");
        }
    }
} 