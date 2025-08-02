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
        static Browser()
        {
            // Selenium Manager'ı tamamen devre dışı bırak
            Environment.SetEnvironmentVariable("SELENIUM_MANAGER_DRIVER_PATH", "");
            Environment.SetEnvironmentVariable("SELENIUM_MANAGER_BROWSER_PATH", "");
            Environment.SetEnvironmentVariable("SELENIUM_MANAGER_DRIVER_PATH_CHROME", "");
            Environment.SetEnvironmentVariable("SELENIUM_MANAGER_DRIVER_PATH_EDGE", "");
            Environment.SetEnvironmentVariable("SELENIUM_MANAGER_DRIVER_PATH_FIREFOX", "");
            Environment.SetEnvironmentVariable("SELENIUM_MANAGER_DRIVER_PATH_SAFARI", "");
            
            // Edge driver için özel ayarlar
            Environment.SetEnvironmentVariable("SELENIUM_MANAGER_DRIVER_PATH_MICROSOFTEDGE", "");
        }

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
            options.AddArgument("--disable-gpu");
            options.AddArgument("--disable-extensions");
            options.AddArgument("--disable-plugins");
            
            if (Configuration.Headless)
            {
                options.AddArgument("--headless");
            }
            
            // Selenium Manager'ı tamamen devre dışı bırak
            options.AddArgument("--disable-selenium-manager");
            options.AddArgument("--disable-web-security");
            options.AddArgument("--disable-features=VizDisplayCompositor");
            
            // Chrome driver'ı manuel olarak yönet
            var service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;
            
            return new ChromeDriver(service, options);
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
            options.AddArgument("--disable-gpu");
            options.AddArgument("--disable-extensions");
            options.AddArgument("--disable-plugins");
            
            // Selenium Manager'ı tamamen devre dışı bırak
            options.AddArgument("--disable-selenium-manager");
            options.AddArgument("--disable-web-security");
            options.AddArgument("--disable-features=VizDisplayCompositor");
            
            // Edge driver'ı manuel olarak yönet - Selenium Manager'ı tamamen devre dışı bırak
            var service = EdgeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;
            
            // Edge driver'ı manuel olarak başlat
            try
            {
                return new EdgeDriver(service, options);
            }
            catch (WebDriverException ex)
            {
                // Eğer Edge driver bulunamazsa, Chrome'a fallback yap
                Console.WriteLine($"Edge driver bulunamadı, Chrome'a geçiliyor: {ex.Message}");
                return InitChromeDriver();
            }
        }

        private static IWebDriver InitSafariDriver()
        {
            return new SafariDriver();
        }
    }
} 