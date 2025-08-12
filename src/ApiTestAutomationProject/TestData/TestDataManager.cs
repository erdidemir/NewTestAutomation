using Microsoft.Extensions.Configuration;
using Serilog;

namespace ApiTestAutomationProject.TestData
{
    public class TestDataManager
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public TestDataManager(IConfiguration configuration, ILogger logger)
        {
            _configuration = configuration;
            _logger = logger;
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

        public string GetTestDataValue(string key)
        {
            var value = _configuration[$"TestData:{key}"];
            
            if (string.IsNullOrEmpty(value))
            {
                _logger.Warning($"Test data value not found: {key}");
            }
            else
            {
                _logger.Debug($"Loaded test data value: {key} = {value}");
            }
            
            return value ?? string.Empty;
        }
    }
} 