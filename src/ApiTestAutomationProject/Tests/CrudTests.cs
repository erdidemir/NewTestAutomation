using NUnit.Framework;
using ApiTestAutomationProject.Core;
using ApiTestAutomationProject.Models;
using FluentAssertions;
using Serilog;

namespace ApiTestAutomationProject.Tests
{
    [TestFixture]
    public class CrudTests
    {
        private ApiClient _apiClient;

        [SetUp]
        public void Setup()
        {
            _apiClient = new ApiClient();
            Log.Information("CRUD tests setup completed");
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
            response.Data.Page.Should().Be(1);
            
            Log.Information($"Retrieved {response.Data.Data.Count} users from page {response.Data.Page}");
        }

        [Test]
        public async Task GetUser_WithValidId_ShouldReturnUser()
        {
            // Arrange
            int userId = 1;

            // Act
            var response = await _apiClient.GetUserAsync(userId);

            // Assert
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            response.Data.Should().NotBeNull();
            response.Data.Id.Should().Be(userId);
            response.Data.Email.Should().NotBeNullOrEmpty();
            
            Log.Information($"Retrieved user: {response.Data.FirstName} {response.Data.LastName}");
        }

        [Test]
        public async Task CreateUser_WithValidData_ShouldReturnCreatedUser()
        {
            // Arrange
            var createUserRequest = new CreateUserRequest
            {
                Name = "John Doe",
                Job = "Software Engineer"
            };

            // Act
            var response = await _apiClient.CreateUserAsync(createUserRequest);

            // Assert
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
            response.Data.Should().NotBeNull();
            response.Data.Name.Should().Be(createUserRequest.Name);
            response.Data.Job.Should().Be(createUserRequest.Job);
            response.Data.Id.Should().NotBeNullOrEmpty();
            response.Data.CreatedAt.Should().NotBeNullOrEmpty();
            
            Log.Information($"Created user with ID: {response.Data.Id}");
        }

        [Test]
        public async Task UpdateUser_WithValidData_ShouldReturnUpdatedUser()
        {
            // Arrange
            int userId = 1;
            var updateUserRequest = new UpdateUserRequest
            {
                Name = "Jane Smith",
                Job = "QA Engineer"
            };

            // Act
            var response = await _apiClient.UpdateUserAsync(userId, updateUserRequest);

            // Assert
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            response.Data.Should().NotBeNull();
            response.Data.Name.Should().Be(updateUserRequest.Name);
            response.Data.Job.Should().Be(updateUserRequest.Job);
            response.Data.UpdatedAt.Should().NotBeNullOrEmpty();
            
            Log.Information($"Updated user {userId} with new name: {response.Data.Name}");
        }

        [Test]
        public async Task DeleteUser_WithValidId_ShouldReturnSuccess()
        {
            // Arrange
            int userId = 1;

            // Act
            var response = await _apiClient.DeleteUserAsync(userId);

            // Assert
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
            
            Log.Information($"Deleted user with ID: {userId}");
        }
    }
} 