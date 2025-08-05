using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace TestAutomationProject.Core
{
    public static class Configuration
    {
        private static readonly IConfigurationRoot _config;

        static Configuration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            _config = builder.Build();
        }

        public static string Browser => _config["Configuration:Browser"] ?? "edge";
        public static string BrowserName => _config["Configuration:Browser"] ?? "edge";
        public static bool Headless => bool.TryParse(_config["Configuration:Headless"], out var val) && val;
        public static bool IsBrowserHeadless => bool.TryParse(_config["Configuration:Headless"], out var val) && val;
        public static string BaseUrl => _config["Configuration:BaseUrl"] ?? "https://defaulturl.com";
        public static string Organization => _config["Configuration:Organization"];
        public static string Workspace => _config["Configuration:Workspace"];
        public static int ElementDelayMilliSeconds =>
            int.TryParse(_config["Configuration:ElementDelayMilliSeconds"], out var val) ? val : 500;

        public static int ElementDelayMilliseconds =>
            int.TryParse(_config["Configuration:ElementDelayMilliSeconds"], out var val) ? val : 500;

        public static string ReportPathBase => _config["Configuration:ReportPathBase"];
        public static string ResultsFolder => _config["Configuration:ResultsFolder"] ?? "Results";

        // Screenshot Settings
        public static bool TakeScreenshotOnSuccess => bool.TryParse(_config["Configuration:ScreenshotSettings:TakeScreenshotOnSuccess"], out var val) && val;
        public static bool TakeScreenshotOnFailure => bool.TryParse(_config["Configuration:ScreenshotSettings:TakeScreenshotOnFailure"], out var val) && val;
        public static string ScreenshotFormat => _config["Configuration:ScreenshotSettings:ScreenshotFormat"] ?? "png";
        public static int ScreenshotQuality => int.TryParse(_config["Configuration:ScreenshotSettings:ScreenshotQuality"], out var val) ? val : 90;

        // Allure Settings
        public static bool GenerateAllureReport => bool.TryParse(_config["Configuration:AllureSettings:GenerateAllureReport"], out var val) && val;
        public static string AllureResultsPath => _config["Configuration:AllureSettings:AllureResultsPath"] ?? "allure-results";
        public static string AllureReportPath => _config["Configuration:AllureSettings:AllureReportPath"] ?? "allure-report";

        // Driver Settings
        public static bool AutoUpdateDrivers => bool.TryParse(_config["Configuration:DriverSettings:AutoUpdateDrivers"], out var val) && val;
        public static bool CheckDriverCompatibility => bool.TryParse(_config["Configuration:DriverSettings:CheckDriverCompatibility"], out var val) && val;
        public static int DriverUpdateIntervalDays => int.TryParse(_config["Configuration:DriverSettings:DriverUpdateIntervalDays"], out var val) ? val : 7;
        public static int DriverDownloadTimeoutSeconds => int.TryParse(_config["Configuration:DriverSettings:DriverDownloadTimeoutSeconds"], out var val) ? val : 300;
        public static bool EnableDriverLogging => bool.TryParse(_config["Configuration:DriverSettings:EnableDriverLogging"], out var val) && val;

        public static class UITestUsers
        {
            public static (string username, string password) Anonymous =>
                (_config["Configuration:UITestUsers:Anonymous:Username"], _config["Configuration:UITestUsers:Anonymous:Password"]);

            public static (string username, string password) AuthenticatedOnly =>
                (_config["Configuration:UITestUsers:AuthenticatedOnly:Username"], _config["Configuration:UITestUsers:AuthenticatedOnly:Password"]);

            public static (string username, string password) Admin =>
                (_config["Configuration:UITestUsers:Admin:Username"], _config["Configuration:UITestUsers:Admin:Password"]);
        }
    }
}
