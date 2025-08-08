using RestSharp;
using Newtonsoft.Json;
using ApiTestAutomationProject.Models;
using Serilog;

namespace ApiTestAutomationProject.Core
{
    public class ApiClient
    {
        private readonly RestClient _client;

        public ApiClient()
        {
            _client = new RestClient("https://reqres.in/api");
        }

        private RestRequest CreateRequest(string endpoint, Method method)
        {
            var request = new RestRequest(endpoint, method);
            request.AddHeader("Content-Type", "application/json");
            return request;
        }

        public async Task<RestResponse<T>> ExecuteAsync<T>(string endpoint, Method method, object? body = null)
        {
            var request = CreateRequest(endpoint, method);
            
            if (body != null)
            {
                var jsonBody = JsonConvert.SerializeObject(body);
                request.AddJsonBody(jsonBody);
            }

            Log.Information($"API Request: {method} {endpoint}");
            if (body != null)
            {
                Log.Information($"Request Body: {JsonConvert.SerializeObject(body, Formatting.Indented)}");
            }

            var response = await _client.ExecuteAsync<T>(request);
            
            Log.Information($"API Response: {response.StatusCode} - {response.Content}");
            
            return response;
        }

        // Authentication
        public async Task<RestResponse<LoginResponse>> LoginAsync(LoginRequest loginRequest)
        {
            return await ExecuteAsync<LoginResponse>("/login", Method.Post, loginRequest);
        }

        // CRUD Operations - ReqRes API doesn't require authentication for these endpoints
        public async Task<RestResponse<UsersListResponse>> GetUsersAsync(int page = 1)
        {
            return await ExecuteAsync<UsersListResponse>($"/users?page={page}", Method.Get);
        }

        public async Task<RestResponse<User>> GetUserAsync(int userId)
        {
            return await ExecuteAsync<User>($"/users/{userId}", Method.Get);
        }

        public async Task<RestResponse<CreateUserResponse>> CreateUserAsync(CreateUserRequest userRequest)
        {
            return await ExecuteAsync<CreateUserResponse>("/users", Method.Post, userRequest);
        }

        public async Task<RestResponse<UpdateUserResponse>> UpdateUserAsync(int userId, UpdateUserRequest userRequest)
        {
            return await ExecuteAsync<UpdateUserResponse>($"/users/{userId}", Method.Put, userRequest);
        }

        public async Task<RestResponse> DeleteUserAsync(int userId)
        {
            return await ExecuteAsync<object>($"/users/{userId}", Method.Delete);
        }
    }
} 