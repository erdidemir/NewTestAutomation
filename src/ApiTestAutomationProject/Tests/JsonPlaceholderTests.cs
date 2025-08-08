using NUnit.Framework;
using RestSharp;
using FluentAssertions;
using Serilog;

namespace ApiTestAutomationProject.Tests
{
    [TestFixture]
    public class JsonPlaceholderTests
    {
        [Test]
        public async Task GetPosts_ShouldReturn200()
        {
            // Arrange
            var client = new RestClient("https://jsonplaceholder.typicode.com");
            var request = new RestRequest("/posts/1", Method.Get);

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
        public async Task CreatePost_ShouldReturn201()
        {
            // Arrange
            var client = new RestClient("https://jsonplaceholder.typicode.com");
            var request = new RestRequest("/posts", Method.Post);
            request.AddJsonBody(new { title = "Test Post", body = "Test Body", userId = 1 });

            // Act
            var response = await client.ExecuteAsync(request);

            // Assert
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
            response.Content.Should().NotBeNullOrEmpty();
            
            Log.Information($"Response Status: {response.StatusCode}");
            Log.Information($"Response Content: {response.Content}");
        }

        [Test]
        public async Task UpdatePost_ShouldReturn200()
        {
            // Arrange
            var client = new RestClient("https://jsonplaceholder.typicode.com");
            var request = new RestRequest("/posts/1", Method.Put);
            request.AddJsonBody(new { id = 1, title = "Updated Post", body = "Updated Body", userId = 1 });

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
        public async Task DeletePost_ShouldReturn200()
        {
            // Arrange
            var client = new RestClient("https://jsonplaceholder.typicode.com");
            var request = new RestRequest("/posts/1", Method.Delete);

            // Act
            var response = await client.ExecuteAsync(request);

            // Assert
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            
            Log.Information($"Response Status: {response.StatusCode}");
        }
    }
} 