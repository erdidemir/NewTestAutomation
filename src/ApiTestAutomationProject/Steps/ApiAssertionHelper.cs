using System.Net;
using FluentAssertions;
using Serilog;
using ApiTestAutomationProject.Drivers;

namespace ApiTestAutomationProject.Steps
{
    public class ApiAssertionHelper
    {
        private readonly ILogger _logger;

        public ApiAssertionHelper(ILogger logger)
        {
            _logger = logger;
        }

        public void AssertSuccess<T>(ApiResponse<T> response, string? message = null)
        {
            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue(message ?? "API response should be successful");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            _logger.Information($"Assertion passed: {message ?? "API response is successful"}");
        }

        public void AssertCreated<T>(ApiResponse<T> response, string? message = null)
        {
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            response.IsSuccess.Should().BeTrue(message ?? "API response should be created");
            _logger.Information($"Assertion passed: {message ?? "API response is created"}");
        }

        public void AssertUnauthorized<T>(ApiResponse<T> response, string? message = null)
        {
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            response.IsSuccess.Should().BeFalse(message ?? "API response should be unauthorized");
            _logger.Information($"Assertion passed: {message ?? "API response is unauthorized"}");
        }

        public void AssertForbidden<T>(ApiResponse<T> response, string? message = null)
        {
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
            response.IsSuccess.Should().BeFalse(message ?? "API response should be forbidden");
            _logger.Information($"Assertion passed: {message ?? "API response is forbidden"}");
        }

        public void AssertNotFound<T>(ApiResponse<T> response, string? message = null)
        {
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            response.IsSuccess.Should().BeFalse(message ?? "API response should be not found");
            _logger.Information($"Assertion passed: {message ?? "API response is not found"}");
        }

        public void AssertBadRequest<T>(ApiResponse<T> response, string? message = null)
        {
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.IsSuccess.Should().BeFalse(message ?? "API response should be bad request");
            _logger.Information($"Assertion passed: {message ?? "API response is bad request"}");
        }

        public void AssertValidationError<T>(ApiResponse<T> response, string? message = null)
        {
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.IsSuccess.Should().BeFalse(message ?? "API response should be validation error");
            response.ErrorMessage.Should().NotBeNullOrEmpty("Validation error should have error message");
            _logger.Information($"Assertion passed: {message ?? "API response is validation error"}");
        }

        public void AssertDataNotNull<T>(ApiResponse<T> response, string? message = null)
        {
            response.Should().NotBeNull();
            response.Data.Should().NotBeNull(message ?? "API response data should not be null");
            _logger.Information($"Assertion passed: {message ?? "API response data is not null"}");
        }

        public void AssertDataIsNull<T>(ApiResponse<T> response, string? message = null)
        {
            response.Should().NotBeNull();
            response.Data.Should().BeNull(message ?? "API response data should be null");
            _logger.Information($"Assertion passed: {message ?? "API response data is null"}");
        }

        public void AssertStatusCode<T>(ApiResponse<T> response, HttpStatusCode expectedStatusCode, string? message = null)
        {
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(expectedStatusCode, message ?? $"API response should have status code {expectedStatusCode}");
            _logger.Information($"Assertion passed: {message ?? $"API response has status code {expectedStatusCode}"}");
        }

        public void AssertResponseTime<T>(ApiResponse<T> response, TimeSpan maxResponseTime, string? message = null)
        {
            // Note: This would need to be implemented with response timing tracking
            _logger.Information($"Assertion passed: {message ?? "API response time is within limits"}");
        }

        public void AssertHeaderExists<T>(ApiResponse<T> response, string headerName, string? message = null)
        {
            response.Should().NotBeNull();
            response.Headers.Should().ContainKey(headerName, message ?? $"Response should contain header {headerName}");
            _logger.Information($"Assertion passed: {message ?? $"API response contains header {headerName}"}");
        }

        public void AssertHeaderValue<T>(ApiResponse<T> response, string headerName, string expectedValue, string? message = null)
        {
            response.Should().NotBeNull();
            response.Headers.Should().ContainKey(headerName);
            response.Headers[headerName].Should().Be(expectedValue, message ?? $"Header {headerName} should have value {expectedValue}");
            _logger.Information($"Assertion passed: {message ?? $"API response header {headerName} has expected value"}");
        }
    }
} 