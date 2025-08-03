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
            IWebDriver driver;
            
            switch (browserName.ToLower())
            {
                case "firefox":
                    driver = new FirefoxDriver();
                    break;
                    
                case "chrome":
                    var chromeOptions = new ChromeOptions();
                    if (Configuration.Headless)
                    {
                        chromeOptions.AddArgument("--headless");
                    }
                    chromeOptions.AddArguments("--ignore-certificate-errors");
                    chromeOptions.AddArguments("--no-sandbox");
                    chromeOptions.AddArguments("--disable-dev-shm-usage");
                    driver = new ChromeDriver(chromeOptions);
                    break;
                    
                case "edge":
                    EdgeOptions edgeOptions = new EdgeOptions();
                    edgeOptions.AddArguments("--ignore-certificate-errors");
                    edgeOptions.AddArguments("--window-size=1920,1080");
                    edgeOptions.AddArguments("inprivate");
                    if (Configuration.Headless)
                    {
                        edgeOptions.AddArguments("--headless=new");
                    }
                    edgeOptions.AddUserProfilePreference("download.prompt_for_download", false);
                    edgeOptions.AddUserProfilePreference("safebrowsing.enabled", true);
                    driver = new EdgeDriver(edgeOptions);
                    break;
                    
                case "safari":
                    driver = new SafariDriver();
                    break;
                    
                default:
                    // Default olarak Chrome kullan
                    var defaultChromeOptions = new ChromeOptions();
                    if (Configuration.Headless)
                    {
                        defaultChromeOptions.AddArgument("--headless");
                    }
                    defaultChromeOptions.AddArguments("--ignore-certificate-errors");
                    defaultChromeOptions.AddArguments("--no-sandbox");
                    defaultChromeOptions.AddArguments("--disable-dev-shm-usage");
                    driver = new ChromeDriver(defaultChromeOptions);
                    break;
            }
            
            ConfigureDriver(driver);
            return driver;
        }

        private static void ConfigureDriver(IWebDriver driver)
        {
            driver.Manage().Window.Size = new Size(1920, 1080);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(Configuration.ElementDelayMilliSeconds / 1000);
        }
    }
} 