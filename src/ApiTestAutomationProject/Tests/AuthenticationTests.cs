using NUnit.Framework;
using ApiTestAutomationProject.Core;
using ApiTestAutomationProject.Models;
using FluentAssertions;
using Serilog;

namespace ApiTestAutomationProject.Tests
{
    [TestFixture]
    public class AuthenticationTests
    {
        private ApiClient _apiClient;

        [SetUp]
        public void Setup()
        {
            _apiClient = new ApiClient();
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

        [Test]
        public async Task Login_WithInvalidCredentials_ShouldReturnError()
        {
            // Arrange
            var loginRequest = new LoginRequest
            {
                Email = "invalid@email.com",
                Password = "wrongpassword"
            };

            // Act
            var response = await _apiClient.LoginAsync(loginRequest);

            // Assert
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            
            Log.Information($"Login failed as expected with status: {response.StatusCode}");
        }
    }
} 