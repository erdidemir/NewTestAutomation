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
                .SetBasePath(Directory.GetCurrentDirectory()) // test çalıştırma dizini
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            _config = builder.Build();
        }

        public static string Browser => _config["Configuration:Browser"] ?? "edge";
        public static bool Headless => bool.TryParse(_config["Configuration:Headless"], out var val) && val;
        public static string BaseUrl => _config["Configuration:BaseUrl"] ?? "https://defaulturl.com";
        public static string Organization => _config["Configuration:Organization"];
        public static string Workspace => _config["Configuration:Workspace"];
        public static int ElementDelayMilliSeconds =>
            int.TryParse(_config["Configuration:ElementDelayMilliSeconds"], out var val) ? val : 500;

        public static string ReportPathBase => _config["Configuration:reportPathBase");

        public static class UITestUsers
        {
            public static (string username, string password) Anonymous =>
                (_config["Configuration:UITestUsers:Anonymous:Username"], _config["Configuration:UITestUsers:Anonymous:Password"]);

            public static (string username, string password) AuthenticatedOnly =>
                (_config["Configuration:UITestUsers:AuthenticatedOnly:Username"], _config["Configuration:UITestUsers:AuthenticatedOnly:Password"]);

            public static (string username, string password) SiteAdmin =>
                (_config["Configuration:UITestUsers:SiteAdmin:Username"], _config["Configuration:UITestUsers:SiteAdmin:Password"]);
        }
    }
}
