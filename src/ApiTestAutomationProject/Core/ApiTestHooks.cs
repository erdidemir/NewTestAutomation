using NUnit.Framework;
using Allure.Commons;
using Serilog;
using System.IO;

namespace ApiTestAutomationProject.Core
{
    [TestFixture]
    public class ApiTestHooks
    {
        [OneTimeSetUp]
        public void SetupLogging()
        {
            var logPath = Path.Combine(Directory.GetCurrentDirectory(), "logs", "api-tests.log");
            Directory.CreateDirectory(Path.GetDirectoryName(logPath));
            
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.File(logPath, rollingInterval: Serilog.RollingInterval.Day)
                .CreateLogger();
            
            Log.Information("API Test Suite started");
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            Log.Information("API Test Suite completed");
            Log.CloseAndFlush();
        }
    }
} 