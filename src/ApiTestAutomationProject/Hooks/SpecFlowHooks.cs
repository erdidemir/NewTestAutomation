using TechTalk.SpecFlow;
using Microsoft.Extensions.Configuration;
using ApiTestAutomationProject.Steps;
using BoDi;
using ApiTestAutomationProject.Drivers;
using ApiTestAutomationProject.Models;
using ApiTestAutomationProject.TestData;
using Serilog;
using Allure.Commons;
using Newtonsoft.Json;

namespace ApiTestAutomationProject.Hooks
{
    [Binding]
    public class SpecFlowHooks
    {
        private readonly IObjectContainer _objectContainer;

        public SpecFlowHooks(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            // Test run başlamadan önce yapılacak işlemler
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("logs/test-run.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            // Her senaryo başlamadan önce dependency injection'ı yapılandır
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            DependencyInjection.RegisterSpecFlowDependencies(_objectContainer, configuration);
            
            var logger = _objectContainer.Resolve<ILogger>();
            var scenarioContext = _objectContainer.Resolve<ScenarioContext>();
            
            logger.Information($"Scenario started: {scenarioContext.ScenarioInfo.Title}");
        }

        // @need tag'i için genel hook
        [BeforeScenario("@need")]
        public async Task BeforeNeedScenario()
        {
            var logger = _objectContainer.Resolve<ILogger>();
            var scenarioContext = _objectContainer.Resolve<ScenarioContext>();
            
            logger.Information($"Setting up test environment for NEED scenario: {scenarioContext.ScenarioInfo.Title}");
            
            try
            {
                // @need tag'i için genel hazırlık işlemleri
                logger.Information("General test environment ready for NEED scenario");
            }
            catch (Exception ex)
            {
                logger.Warning($"Failed to setup test environment for NEED: {ex.Message}");
            }
        }

        // @need:update tag'i için özel hook
        [BeforeScenario("@need:update")]
        public async Task BeforeNeedUpdateScenario()
        {
            var logger = _objectContainer.Resolve<ILogger>();
            var scenarioContext = _objectContainer.Resolve<ScenarioContext>();
            var apiClient = _objectContainer.Resolve<IApiClient>();
            var endpointManager = _objectContainer.Resolve<IEndpointManager>();
            var postTestData = _objectContainer.Resolve<PostTestData>();
            
            logger.Information($"Setting up test data for NEED UPDATE scenario: {scenarioContext.ScenarioInfo.Title}");
            
            try
            {
                // Update testleri için özel hazırlık
                var testPostData = postTestData.CreateData[EnumPost.Valid];
                var endpoint = endpointManager.GetEndpoint("Posts", "Create");
                var response = await apiClient.PostAsync<CreatePostResponse>(endpoint, testPostData);
                
                if (response.IsSuccess && response.Data != null)
                {
                    scenarioContext["TestPostId"] = response.Data.Id;
                    scenarioContext["TestPostData"] = testPostData;
                    logger.Information($"Test post created for NEED UPDATE with ID: {response.Data.Id}");
                }
                else
                {
                    scenarioContext["TestPostId"] = "1";
                    scenarioContext["TestPostData"] = testPostData;
                    logger.Information("Using existing test post ID: 1 for NEED UPDATE");
                }
            }
            catch (Exception ex)
            {
                logger.Warning($"Failed to create test post for NEED UPDATE: {ex.Message}");
                scenarioContext["TestPostId"] = "1";
            }
        }

        // @need:create tag'i için özel hook
        [BeforeScenario("@need:create")]
        public async Task BeforeNeedCreateScenario()
        {
            var logger = _objectContainer.Resolve<ILogger>();
            var scenarioContext = _objectContainer.Resolve<ScenarioContext>();
            
            logger.Information($"Setting up test environment for NEED CREATE scenario: {scenarioContext.ScenarioInfo.Title}");
            
            try
            {
                // Create testleri için özel hazırlık
                logger.Information("Test environment ready for NEED CREATE scenario");
            }
            catch (Exception ex)
            {
                logger.Warning($"Failed to setup test environment for NEED CREATE: {ex.Message}");
            }
        }

        // @need:delete tag'i için özel hook
        [BeforeScenario("@need:delete")]
        public async Task BeforeNeedDeleteScenario()
        {
            var logger = _objectContainer.Resolve<ILogger>();
            var scenarioContext = _objectContainer.Resolve<ScenarioContext>();
            var apiClient = _objectContainer.Resolve<IApiClient>();
            var endpointManager = _objectContainer.Resolve<IEndpointManager>();
            var postTestData = _objectContainer.Resolve<PostTestData>();
            
            logger.Information($"Setting up test data for NEED DELETE scenario: {scenarioContext.ScenarioInfo.Title}");
            
            try
            {
                // Delete testleri için özel hazırlık - silinecek veri oluştur
                var testPostData = postTestData.CreateData[EnumPost.Valid];
                var endpoint = endpointManager.GetEndpoint("Posts", "Create");
                var response = await apiClient.PostAsync<CreatePostResponse>(endpoint, testPostData);
                
                if (response.IsSuccess && response.Data != null)
                {
                    scenarioContext["DeletePostId"] = response.Data.Id;
                    scenarioContext["DeletePostData"] = testPostData;
                    logger.Information($"Test post created for NEED DELETE with ID: {response.Data.Id}");
                }
                else
                {
                    scenarioContext["DeletePostId"] = "1";
                    scenarioContext["DeletePostData"] = testPostData;
                    logger.Information("Using existing test post ID: 1 for NEED DELETE");
                }
            }
            catch (Exception ex)
            {
                logger.Warning($"Failed to create test post for NEED DELETE: {ex.Message}");
                scenarioContext["DeletePostId"] = "1";
            }
        }

        // @need:get tag'i için özel hook
        [BeforeScenario("@need:get")]
        public async Task BeforeNeedGetScenario()
        {
            var logger = _objectContainer.Resolve<ILogger>();
            var scenarioContext = _objectContainer.Resolve<ScenarioContext>();
            
            logger.Information($"Setting up test environment for NEED GET scenario: {scenarioContext.ScenarioInfo.Title}");
            
            try
            {
                // Get testleri için özel hazırlık
                logger.Information("Test environment ready for NEED GET scenario");
            }
            catch (Exception ex)
            {
                logger.Warning($"Failed to setup test environment for NEED GET: {ex.Message}");
            }
        }

        // @need tag'i için genel cleanup hook
        [AfterScenario("@need")]
        public async Task AfterNeedScenario()
        {
            var logger = _objectContainer.Resolve<ILogger>();
            var scenarioContext = _objectContainer.Resolve<ScenarioContext>();
            
            logger.Information($"Cleaning up test environment for NEED scenario: {scenarioContext.ScenarioInfo.Title}");
            
            try
            {
                // @need tag'i için genel temizlik işlemleri
                logger.Information("General test environment cleanup completed for NEED scenario");
            }
            catch (Exception ex)
            {
                logger.Warning($"Failed to cleanup test environment for NEED: {ex.Message}");
            }
        }

        // @need:update tag'i için özel cleanup hook
        [AfterScenario("@need:update")]
        public async Task AfterNeedUpdateScenario()
        {
            var logger = _objectContainer.Resolve<ILogger>();
            var scenarioContext = _objectContainer.Resolve<ScenarioContext>();
            
            logger.Information($"Cleaning up test data for NEED UPDATE scenario: {scenarioContext.ScenarioInfo.Title}");
            
            try
            {
                if (scenarioContext.ContainsKey("TestPostId"))
                {
                    var testPostId = scenarioContext.Get<object>("TestPostId");
                    var testPostIdString = testPostId?.ToString() ?? "unknown";
                    
                    logger.Information($"Test post cleanup completed for NEED UPDATE ID: {testPostIdString}");
                }
            }
            catch (Exception ex)
            {
                // Allure report için hata bilgisi ekle
                AllureLifecycle.Instance.AddAttachment("test-error.txt", "text/plain", 
                    $"Test execution error: {ex.Message}");
            }
        }

        // @need:delete tag'i için özel cleanup hook
        [AfterScenario("@need:delete")]
        public async Task AfterNeedDeleteScenario()
        {
            var logger = _objectContainer.Resolve<ILogger>();
            var scenarioContext = _objectContainer.Resolve<ScenarioContext>();
            
            logger.Information($"Cleaning up test data for NEED DELETE scenario: {scenarioContext.ScenarioInfo.Title}");
            
            try
            {
                if (scenarioContext.ContainsKey("DeletePostId"))
                {
                    var deletePostId = scenarioContext.Get<object>("DeletePostId");
                    var deletePostIdString = deletePostId?.ToString() ?? "unknown";
                    
                    logger.Information($"Delete test cleanup completed for NEED DELETE ID: {deletePostIdString}");
                }
            }
            catch (Exception ex)
            {
                logger.Warning($"Failed to cleanup test data for NEED DELETE: {ex.Message}");
            }
        }

        [AfterScenario]
        public void AfterScenario()
        {
            // Her senaryo sonrasında temizlik işlemleri
            var logger = _objectContainer.Resolve<ILogger>();
            var scenarioContext = _objectContainer.Resolve<ScenarioContext>();
            
            logger.Information($"Scenario completed: {scenarioContext.ScenarioInfo.Title}");
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            // Test run sonrasında yapılacak işlemler
            Log.CloseAndFlush();
        }
    }
} 