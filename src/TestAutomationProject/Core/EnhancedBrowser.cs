using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Safari;
using System.Drawing;
using Serilog;

namespace TestAutomationProject.Core
{
    public static class EnhancedBrowser
    {
        public static IWebDriver InitBrowser(string browserName)
        {
            PerformanceMonitor.StartTimer($"Browser_Init_{browserName}");
            
            try
            {
                // Driver uyumluluk kontrolü
                if (!DriverManager.CheckDriverCompatibility(browserName))
                {
                    Log.Warning($"{browserName} driver uyumluluğu kontrol edilemedi");
                }
                
                IWebDriver driver;
                
                switch (browserName.ToLower())
                {
                    case "firefox":
                        driver = InitFirefoxDriver();
                        break;
                        
                    case "chrome":
                        driver = InitChromeDriver();
                        break;
                        
                    case "edge":
                        driver = InitEdgeDriver();
                        break;
                        
                    case "safari":
                        driver = InitSafariDriver();
                        break;
                        
                    default:
                        Log.Warning($"Bilinmeyen tarayıcı: {browserName}, Chrome kullanılıyor");
                        driver = InitChromeDriver();
                        break;
                }
                
                ConfigureDriver(driver);
                
                var elapsed = PerformanceMonitor.StopTimer($"Browser_Init_{browserName}");
                Log.Information($"{browserName} tarayıcısı {elapsed}ms'de başlatıldı");
                
                return driver;
            }
            catch (Exception ex)
            {
                PerformanceMonitor.StopTimer($"Browser_Init_{browserName}");
                Log.Error(ex, $"{browserName} tarayıcısı başlatılırken hata oluştu");
                throw;
            }
        }
        
        private static IWebDriver InitFirefoxDriver()
        {
            PerformanceMonitor.StartTimer("Firefox_Init");
            var driver = new FirefoxDriver();
            PerformanceMonitor.StopTimer("Firefox_Init");
            return driver;
        }
        
        private static IWebDriver InitChromeDriver()
        {
            PerformanceMonitor.StartTimer("Chrome_Init");
            
            var chromeOptions = new ChromeOptions();
            if (Configuration.Headless)
            {
                chromeOptions.AddArgument("--headless");
            }
            
            // Performans optimizasyonları
            chromeOptions.AddArguments("--ignore-certificate-errors");
            chromeOptions.AddArguments("--no-sandbox");
            chromeOptions.AddArguments("--disable-dev-shm-usage");
            chromeOptions.AddArguments("--disable-gpu");
            chromeOptions.AddArguments("--disable-extensions");
            chromeOptions.AddArguments("--disable-plugins");
            chromeOptions.AddArguments("--disable-images");
            chromeOptions.AddArguments("--disable-javascript");
            chromeOptions.AddArguments("--disable-background-timer-throttling");
            chromeOptions.AddArguments("--disable-backgrounding-occluded-windows");
            chromeOptions.AddArguments("--disable-renderer-backgrounding");
            chromeOptions.AddArguments("--disable-features=TranslateUI");
            chromeOptions.AddArguments("--disable-ipc-flooding-protection");
            
            // Memory optimizasyonu
            chromeOptions.AddArguments("--memory-pressure-off");
            chromeOptions.AddArguments("--max_old_space_size=4096");
            
            var driver = new ChromeDriver(chromeOptions);
            PerformanceMonitor.StopTimer("Chrome_Init");
            return driver;
        }
        
        private static IWebDriver InitEdgeDriver()
        {
            PerformanceMonitor.StartTimer("Edge_Init");
            
            EdgeOptions edgeOptions = new EdgeOptions();
            edgeOptions.AddArguments("--ignore-certificate-errors");
            edgeOptions.AddArguments("--window-size=1920,1080");
            edgeOptions.AddArguments("inprivate");
            
            if (Configuration.Headless)
            {
                edgeOptions.AddArguments("--headless=new");
            }
            
            // Performans optimizasyonları
            edgeOptions.AddArguments("--disable-gpu");
            edgeOptions.AddArguments("--disable-extensions");
            edgeOptions.AddArguments("--disable-plugins");
            edgeOptions.AddArguments("--disable-images");
            edgeOptions.AddArguments("--disable-javascript");
            edgeOptions.AddArguments("--disable-background-timer-throttling");
            edgeOptions.AddArguments("--disable-backgrounding-occluded-windows");
            edgeOptions.AddArguments("--disable-renderer-backgrounding");
            edgeOptions.AddArguments("--disable-features=TranslateUI");
            edgeOptions.AddArguments("--disable-ipc-flooding-protection");
            
            // Memory optimizasyonu
            edgeOptions.AddArguments("--memory-pressure-off");
            edgeOptions.AddArguments("--max_old_space_size=4096");
            
            edgeOptions.AddUserProfilePreference("download.prompt_for_download", false);
            edgeOptions.AddUserProfilePreference("safebrowsing.enabled", true);
            
            var driver = new EdgeDriver(edgeOptions);
            PerformanceMonitor.StopTimer("Edge_Init");
            return driver;
        }
        
        private static IWebDriver InitSafariDriver()
        {
            PerformanceMonitor.StartTimer("Safari_Init");
            var driver = new SafariDriver();
            PerformanceMonitor.StopTimer("Safari_Init");
            return driver;
        }

        private static void ConfigureDriver(IWebDriver driver)
        {
            PerformanceMonitor.StartTimer("Driver_Configure");
            
            driver.Manage().Window.Size = new Size(1920, 1080);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(Configuration.ElementDelayMilliSeconds / 1000);
            
            // Performans için ek timeout ayarları
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);
            driver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromSeconds(10);
            
            PerformanceMonitor.StopTimer("Driver_Configure");
        }
        
        public static void LogBrowserPerformance(IWebDriver driver)
        {
            try
            {
                var metrics = PerformanceMonitor.GetCurrentMetrics();
                var statistics = PerformanceMonitor.GetPerformanceStatistics();
                
                Log.Information("=== Browser Performance Metrics ===");
                Log.Information($"Memory Usage: {metrics.MemoryUsageMB:F2}MB");
                Log.Information($"Thread Count: {metrics.ThreadCount}");
                Log.Information($"Process Time: {metrics.ProcessTime:F2}ms");
                
                Log.Information("=== Browser Operation Statistics ===");
                foreach (var stat in statistics.Values)
                {
                    if (stat.OperationName.Contains("Browser") || stat.OperationName.Contains("Driver"))
                    {
                        Log.Information($"{stat.OperationName}: Avg={stat.AverageTime:F2}ms, Min={stat.MinTime}ms, Max={stat.MaxTime}ms, Count={stat.TotalExecutions}");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Browser performans metrikleri loglanırken hata oluştu");
            }
        }
        
        public static async Task<bool> EnsureDriverCompatibilityAsync(string browserName)
        {
            try
            {
                Log.Information($"{browserName} driver uyumluluğu kontrol ediliyor...");
                
                if (!DriverManager.CheckDriverCompatibility(browserName))
                {
                    Log.Warning($"{browserName} driver uyumlu değil, güncelleme deneniyor...");
                    
                    bool updateSuccess = false;
                    
                    if (browserName.ToLower() == "chrome")
                    {
                        updateSuccess = await DriverManager.UpdateChromeDriverAsync();
                    }
                    else if (browserName.ToLower() == "edge")
                    {
                        updateSuccess = await DriverManager.UpdateEdgeDriverAsync();
                    }
                    
                    if (updateSuccess)
                    {
                        Log.Information($"{browserName} driver başarıyla güncellendi");
                        return true;
                    }
                    else
                    {
                        Log.Error($"{browserName} driver güncellenemedi");
                        return false;
                    }
                }
                
                Log.Information($"{browserName} driver uyumlu");
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"{browserName} driver uyumluluk kontrolü sırasında hata oluştu");
                return false;
            }
        }
    }
} 