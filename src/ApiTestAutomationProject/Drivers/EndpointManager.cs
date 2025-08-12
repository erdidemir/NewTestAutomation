using Microsoft.Extensions.Configuration;
using Serilog;
using ApiTestAutomationProject.Constants;

namespace ApiTestAutomationProject.Drivers
{
    public interface IEndpointManager
    {
        string GetEndpoint(string category, string operation, params object[] parameters);
        string GetBaseUrl();
        T? GetTestData<T>(string key) where T : class;
    }

    public class EndpointManager : IEndpointManager
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public EndpointManager(IConfiguration configuration, ILogger logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public string GetEndpoint(string category, string operation, params object[] parameters)
        {
            string endpoint = "";
            
            // ReqRes API endpoints
            switch (category.ToLower())
            {
                case "users":
                    endpoint = operation.ToLower() switch
                    {
                        "getall" => ApiEndpoints.ReqRes.Users.GetAll,
                        "getbyid" => ApiEndpoints.ReqRes.Users.GetById,
                        "create" => ApiEndpoints.ReqRes.Users.Create,
                        "update" => ApiEndpoints.ReqRes.Users.Update,
                        "delete" => ApiEndpoints.ReqRes.Users.Delete,
                        _ => string.Empty
                    };
                    break;
                    
                case "authentication":
                    endpoint = operation.ToLower() switch
                    {
                        "login" => ApiEndpoints.ReqRes.Authentication.Login,
                        "logout" => ApiEndpoints.ReqRes.Authentication.Logout,
                        "register" => ApiEndpoints.ReqRes.Authentication.Register,
                        _ => string.Empty
                    };
                    break;
                    
                case "posts":
                    endpoint = operation.ToLower() switch
                    {
                        "getall" => ApiEndpoints.JsonPlaceholder.Posts.GetAll,
                        "getbyid" => ApiEndpoints.JsonPlaceholder.Posts.GetById,
                        "create" => ApiEndpoints.JsonPlaceholder.Posts.Create,
                        "update" => ApiEndpoints.JsonPlaceholder.Posts.Update,
                        "delete" => ApiEndpoints.JsonPlaceholder.Posts.Delete,
                        _ => string.Empty
                    };
                    break;
                    
                default:
                    _logger.Warning($"Unknown category: {category}");
                    return string.Empty;
            }
            
            if (string.IsNullOrEmpty(endpoint))
            {
                _logger.Warning($"Endpoint not found: {category}:{operation}");
                return string.Empty;
            }

            // Replace placeholders with parameters
            for (int i = 0; i < parameters.Length; i++)
            {
                endpoint = endpoint.Replace($"{{{i}}}", parameters[i]?.ToString() ?? "");
            }

            // Replace named placeholders
            if (parameters.Length > 0 && parameters[0] is string id)
            {
                endpoint = ApiEndpoints.ReplaceId(endpoint, id);
            }

            _logger.Debug($"Resolved endpoint: {category}:{operation} -> {endpoint}");
            return endpoint;
        }

        public string GetBaseUrl()
        {
            // Get BaseUrl from appsettings.json
            var baseUrl = _configuration["ApiSettings:BaseUrl"];
            if (string.IsNullOrEmpty(baseUrl))
            {
                _logger.Warning("BaseUrl not found in appsettings.json, using default ReqRes URL");
                baseUrl = "https://reqres.in/api";
            }
            _logger.Debug($"Base URL: {baseUrl}");
            return baseUrl;
        }

        public T? GetTestData<T>(string key) where T : class
        {
            var section = _configuration.GetSection($"TestData:{key}");
            var data = section.Get<T>();
            
            if (data == null)
            {
                _logger.Warning($"Test data not found: {key}");
            }
            else
            {
                _logger.Debug($"Loaded test data: {key}");
            }
            
            return data;
        }
    }
} 