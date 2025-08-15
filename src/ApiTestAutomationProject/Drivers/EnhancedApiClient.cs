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
using Allure.Commons;

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
        public TimeSpan Duration { get; set; }
        public string? RequestBody { get; set; }
        public string? ResponseBody { get; set; }
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
                ThrowOnAnyError = false,
                Timeout = timeout
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
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            
            try
            {
                var request = CreateRequest(endpoint, method, body, headers);
                
                LogRequest(request, body);
                
                var response = await _client.ExecuteAsync<T>(request);
                
                stopwatch.Stop();
                var apiResponse = CreateApiResponse<T>(response, stopwatch.Elapsed, body);
                LogResponse(apiResponse);
                
                // Allure report için API çağrısı bilgisi ekle
                AddAllureAttachment(apiResponse, endpoint, method);
                
                return apiResponse;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.Error(ex, "API request failed");
                
                var errorResponse = new ApiResponse<T>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrorMessage = ex.Message,
                    Duration = stopwatch.Elapsed
                };
                
                // Allure report için hata bilgisi ekle
                AllureLifecycle.Instance.AddAttachment("api-error.txt", "text/plain", 
                    $"Endpoint: {endpoint}\nMethod: {method}\nError: {ex.Message}\nStackTrace: {ex.StackTrace}");
                
                return errorResponse;
            }
        }

        private void AddAllureAttachment<T>(ApiResponse<T> response, string endpoint, HttpMethod method)
        {
            try
            {
                var apiInfo = new
                {
                    Endpoint = endpoint,
                    Method = method.ToString(),
                    StatusCode = response.StatusCode,
                    Duration = response.Duration.TotalMilliseconds,
                    IsSuccess = response.IsSuccess,
                    RequestBody = response.RequestBody,
                    ResponseBody = response.ResponseBody,
                    ErrorMessage = response.ErrorMessage
                };

                var jsonInfo = JsonConvert.SerializeObject(apiInfo, Formatting.Indented);
                AllureLifecycle.Instance.AddAttachment("api-call.json", "application/json", jsonInfo);

                if (!response.IsSuccess && !string.IsNullOrEmpty(response.ErrorMessage))
                {
                    AllureLifecycle.Instance.AddAttachment("api-error.txt", "text/plain", 
                        $"Error: {response.ErrorMessage}\nStatusCode: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.Warning($"Failed to add Allure attachment: {ex.Message}");
            }
        }

        private RestRequest CreateRequest(string endpoint, HttpMethod method, object? body, Dictionary<string, string>? headers)
        {
            var request = new RestRequest(endpoint, ConvertToRestSharpMethod(method));
            
            // API Key ekle
            var apiKey = _configuration.GetValue<string>("ApiSettings:ApiKey");
            if (!string.IsNullOrEmpty(apiKey))
            {
                request.AddHeader("X-API-Key", apiKey);
            }
            
            if (!string.IsNullOrEmpty(_authToken))
            {
                request.AddHeader("Authorization", $"Bearer {_authToken}");
            }
            
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.AddHeader(header.Key, header.Value);
                }
            }
            
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

        private ApiResponse<T> CreateApiResponse<T>(RestResponse<T> response, TimeSpan duration, object? requestBody)
        {
            var apiResponse = new ApiResponse<T>
            {
                Data = response.Data,
                StatusCode = response.StatusCode,
                ErrorMessage = response.ErrorMessage,
                Duration = duration,
                RequestBody = requestBody != null ? JsonConvert.SerializeObject(requestBody, Formatting.Indented) : null,
                ResponseBody = response.Content
            };

            if (response.Headers != null)
            {
                foreach (var header in response.Headers)
                {
                    if (header.Name != null && header.Value != null)
                    {
                        apiResponse.Headers[header.Name] = header.Value.ToString() ?? string.Empty;
                    }
                }
            }

            return apiResponse;
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
    }
} 