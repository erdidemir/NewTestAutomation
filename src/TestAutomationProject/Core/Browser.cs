using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Safari;
using System.Drawing;

namespace TestAutomationProject.Core
{
    public static class Browser
    {
        public static IWebDriver InitBrowser(string browserName)
        {
            return browserName.ToLower() switch
            {
                "chrome" => InitChromeDriver(),
                "firefox" => InitFirefoxDriver(),
                "edge" => InitEdgeDriver(),
                "safari" => InitSafariDriver(),
                _ => InitChromeDriver()
            };
        }

        private static IWebDriver InitChromeDriver()
        {
            var options = new ChromeOptions();
            
            // En basit ayarlar
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");
            
            if (Configuration.Headless)
            {
                options.AddArgument("--headless");
            }
            
            // Selenium Manager'ı tamamen devre dışı bırak
            options.AddArgument("--disable-selenium-manager");
            options.AddArgument("--disable-web-security");
            options.AddArgument("--disable-features=VizDisplayCompositor");
            
            return new ChromeDriver(options);
        }

        private static IWebDriver InitFirefoxDriver()
        {
            var options = new FirefoxOptions();
            if (Configuration.Headless)
            {
                options.AddArgument("--headless");
            }
            return new FirefoxDriver(options);
        }

        private static IWebDriver InitEdgeDriver()
        {
            var options = new EdgeOptions();
            if (Configuration.Headless)
            {
                options.AddArgument("--headless");
            }
            
            // Basit Edge ayarları
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");
            
            // Selenium Manager'ı devre dışı bırak
            options.AddArgument("--disable-selenium-manager");
            
            return new EdgeDriver(options);
        }

        private static IWebDriver InitSafariDriver()
        {
            return new SafariDriver();
        }
    }
} 