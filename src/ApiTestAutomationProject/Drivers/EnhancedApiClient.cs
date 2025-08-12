using RestSharp;
using Newtonsoft.Json;
using ApiTestAutomationProject.Models;
using Serilog;
using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Retry;
using System.Text;

namespace ApiTestAutomationProject.Drivers
{
    public interface IApiClient
    {
        Task<ApiResponse<T>> ExecuteAsync<T>(string endpoint, HttpMethod method, object? body = null, Dictionary<string, string>? headers = null);
        Task<ApiResponse<T>> GetAsync<T>(string endpoint, Dictionary<string, string>? headers = null);
        Task<ApiResponse<T>> PostAsync<T>(string endpoint, object body, Dictionary<string, string>? headers = null);
        Task<ApiResponse<T>> PutAsync<T>(string endpoint, object body, Dictionary<string, string>? headers = null);
        Task<ApiResponse<T>> DeleteAsync<T>(string endpoint, Dictionary<string, string>? headers = null);
        void SetAuthToken(string token);
        void ClearAuthToken();
        string? GetAuthToken();
    }

    public class ApiResponse<T>
    {
        public T? Data { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string? ErrorMessage { get; set; }
        public Dictionary<string, string> Headers { get; set; } = new();
        public bool IsSuccess => (int)StatusCode >= 200 && (int)StatusCode < 300;
    }

    public class EnhancedApiClient : IApiClient
    {
        private readonly RestClient _client;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly IEndpointManager _endpointManager;
        private string? _authToken;

        public EnhancedApiClient(IConfiguration configuration, ILogger logger, IEndpointManager endpointManager)
        {
            _configuration = configuration;
            _logger = logger;
            _endpointManager = endpointManager;
            
            var baseUrl = _endpointManager.GetBaseUrl();
            var timeout = _configuration.GetValue<int>("ApiSettings:Timeout", 30000);
            var options = new RestClientOptions(baseUrl)
            {
                ThrowOnDeserializationError = false,
                ThrowOnAnyError = false
            };
            
            _client = new RestClient(options);
        }

        public void SetAuthToken(string token)
        {
            _authToken = token;
            _logger.Information("Authentication token set");
        }

        public void ClearAuthToken()
        {
            _authToken = null;
            _logger.Information("Authentication token cleared");
        }

        public string? GetAuthToken()
        {
            return _authToken;
        }

        public async Task<ApiResponse<T>> ExecuteAsync<T>(string endpoint, HttpMethod method, object? body = null, Dictionary<string, string>? headers = null)
        {
            try
            {
                var request = CreateRequest(endpoint, method, body, headers);
                
                LogRequest(request, body);
                
                var response = await _client.ExecuteAsync<T>(request);
                
                var apiResponse = CreateApiResponse<T>(response);
                LogResponse(apiResponse);
                
                return apiResponse;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "API request failed");
                return new ApiResponse<T>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrorMessage = ex.Message
                };
            }
        }

        public async Task<ApiResponse<T>> GetAsync<T>(string endpoint, Dictionary<string, string>? headers = null)
        {
            return await ExecuteAsync<T>(endpoint, HttpMethod.Get, null, headers);
        }

        public async Task<ApiResponse<T>> PostAsync<T>(string endpoint, object body, Dictionary<string, string>? headers = null)
        {
            return await ExecuteAsync<T>(endpoint, HttpMethod.Post, body, headers);
        }

        public async Task<ApiResponse<T>> PutAsync<T>(string endpoint, object body, Dictionary<string, string>? headers = null)
        {
            return await ExecuteAsync<T>(endpoint, HttpMethod.Put, body, headers);
        }

        public async Task<ApiResponse<T>> DeleteAsync<T>(string endpoint, Dictionary<string, string>? headers = null)
        {
            return await ExecuteAsync<T>(endpoint, HttpMethod.Delete, null, headers);
        }

        private RestRequest CreateRequest(string endpoint, HttpMethod method, object? body, Dictionary<string, string>? headers)
        {
            var request = new RestRequest(endpoint, ConvertToRestSharpMethod(method));
            
            // Default headers
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            
            // API Key header
            var apiKey = _configuration["ApiSettings:ApiKey"];
            if (!string.IsNullOrEmpty(apiKey))
            {
                request.AddHeader("X-API-Key", apiKey);
                _logger.Debug("Added API key to request");
            }
            
            // Custom headers
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.AddHeader(header.Key, header.Value);
                }
            }
            
            // Authentication header (if token is available)
            if (!string.IsNullOrEmpty(_authToken))
            {
                request.AddHeader("Authorization", $"Bearer {_authToken}");
                _logger.Debug("Added Bearer token to request");
            }
            else
            {
                // Fallback to configuration token if no session token
                var configToken = _configuration["ApiSettings:AuthToken"];
                if (!string.IsNullOrEmpty(configToken))
                {
                    request.AddHeader("Authorization", $"Bearer {configToken}");
                    _logger.Debug("Added configuration Bearer token to request");
                }
            }
            
            // Request body
            if (body != null)
            {
                var jsonBody = JsonConvert.SerializeObject(body);
                request.AddJsonBody(jsonBody);
            }
            
            return request;
        }

        private Method ConvertToRestSharpMethod(HttpMethod method)
        {
            return method.Method.ToUpper() switch
            {
                "GET" => Method.Get,
                "POST" => Method.Post,
                "PUT" => Method.Put,
                "DELETE" => Method.Delete,
                "PATCH" => Method.Patch,
                _ => Method.Get
            };
        }

        private ApiResponse<T> CreateApiResponse<T>(RestResponse<T> response)
        {
            var headers = new Dictionary<string, string>();
            if (response.Headers != null)
            {
                foreach (var header in response.Headers)
                {
                    var headerName = header.Name ?? "";
                    var headerValue = header.Value?.ToString() ?? "";
                    
                    // Skip duplicate headers or use the last one
                    if (!string.IsNullOrEmpty(headerName))
                    {
                        headers[headerName] = headerValue;
                    }
                }
            }

            return new ApiResponse<T>
            {
                Data = response.Data,
                StatusCode = response.StatusCode,
                ErrorMessage = response.ErrorMessage,
                Headers = headers
            };
        }

        private void LogRequest(RestRequest request, object? body)
        {
            _logger.Information($"API Request: {request.Method} {request.Resource}");
            if (body != null)
            {
                _logger.Information($"Request Body: {JsonConvert.SerializeObject(body, Formatting.Indented)}");
            }
        }

        private void LogResponse<T>(ApiResponse<T> response)
        {
            _logger.Information($"API Response: {response.StatusCode}");
            if (!response.IsSuccess)
            {
                _logger.Warning($"API Error: {response.ErrorMessage}");
            }
        }
    }
} 