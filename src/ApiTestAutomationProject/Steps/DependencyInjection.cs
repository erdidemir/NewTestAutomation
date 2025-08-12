using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Serilog;
using Polly;
using Polly.Retry;
using ApiTestAutomationProject.Drivers;
using ApiTestAutomationProject.TestData;
using BoDi;

namespace ApiTestAutomationProject.Steps
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiTestAutomation(this IServiceCollection services, IConfiguration configuration)
        {
            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("logs/api-test-.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            services.AddSingleton(Log.Logger);

            // Register Endpoint Manager
            services.AddSingleton<IEndpointManager, EndpointManager>();

            // Register API Client
            services.AddHttpClient<IApiClient, EnhancedApiClient>(client =>
            {
                // Default to JOMM service if no specific service is configured
                var baseUrl = configuration["ApiClients:BaseAddresses:JOMM"] ?? "http://localhost:7090";
                client.BaseAddress = new Uri(baseUrl);
                client.Timeout = TimeSpan.FromSeconds(30);
            });

            // Register Test Data
            services.AddSingleton<TestDataManager>();
            services.AddSingleton<PostTestData>();

            // Register Assertion Helpers
            services.AddSingleton<ApiAssertionHelper>();

            return services;
        }

        public static void RegisterSpecFlowDependencies(IObjectContainer objectContainer, IConfiguration configuration)
        {
            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("logs/api-test-.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            // Register Configuration
            objectContainer.RegisterInstanceAs(configuration);

            // Register Logger
            objectContainer.RegisterInstanceAs(Log.Logger);

            // Register Endpoint Manager
            objectContainer.RegisterTypeAs<EndpointManager, IEndpointManager>();

            // Register API Client
            objectContainer.RegisterTypeAs<EnhancedApiClient, IApiClient>();

            // Register Test Data Manager
            objectContainer.RegisterTypeAs<TestDataManager, TestDataManager>();

            // Register Post Test Data
            objectContainer.RegisterTypeAs<PostTestData, PostTestData>();

            // Register Assertion Helper
            objectContainer.RegisterTypeAs<ApiAssertionHelper, ApiAssertionHelper>();
        }
    }
} 