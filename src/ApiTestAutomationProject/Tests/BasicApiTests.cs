using NUnit.Framework;
using RestSharp;
using FluentAssertions;
using Serilog;

namespace ApiTestAutomationProject.Tests
{
    [TestFixture]
    public class BasicApiTests
    {
        [Test]
        public async Task GetUsers_ShouldReturn200()
        {
            // Arrange
            var client = new RestClient("https://reqres.in/api");
            var request = new RestRequest("/users?page=1", Method.Get);

            // Act
            var response = await client.ExecuteAsync(request);

            // Assert
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            response.Content.Should().NotBeNullOrEmpty();
            
            Log.Information($"Response Status: {response.StatusCode}");
            Log.Information($"Response Content: {response.Content}");
        }

        [Test]
        public async Task Login_ShouldReturn200()
        {
            // Arrange
            var client = new RestClient("https://reqres.in/api");
            var request = new RestRequest("/login", Method.Post);
            request.AddJsonBody(new { email = "eve.holt@reqres.in", password = "cityslicka" });

            // Act
            var response = await client.ExecuteAsync(request);

            // Assert
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            response.Content.Should().NotBeNullOrEmpty();
            
            Log.Information($"Response Status: {response.StatusCode}");
            Log.Information($"Response Content: {response.Content}");
        }
    }
} 