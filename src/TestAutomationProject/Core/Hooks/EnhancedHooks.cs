using BoDi;
using OpenQA.Selenium;
using System.Drawing;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using TechTalk.SpecFlow;
using NUnit.Framework;
using TestAutomationProject.Pages;
using TestAutomationProject.Core;
using TestAutomationProject.Helpers;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;

namespace TestAutomationProject.Core.Hooks
{
    [Binding]
    class EnhancedHooks
    {
        private static ExtentReports extent;
        private static ExtentTest scenarioTest;
        private static Random _random = new Random();
        public static string randomText;
        private readonly IObjectContainer objectContainer;
        private readonly ScenarioContext scenarioContext;
        private IWebDriver webDriver;
        private CommonPage _commonPage;
        private static string reportPath = System.IO.Directory.GetParent(@"../../../").FullName
                                           + Path.DirectorySeparatorChar + (Configuration.ResultsFolder ?? "Results")
                                           + Path.DirectorySeparatorChar + (Configuration.ReportPathBase ?? "Result") + "_" +
                                           DateTime.Now.ToString("ddMMyyyy HHmmss");
        private static string extentReportPath = Path.Combine(Directory.GetCurrentDirectory(), "Results", "index.html");

        public EnhancedHooks(IObjectContainer objectContainer, ScenarioContext scenarioContext)
        {
            this.objectContainer = objectContainer;
            this.scenarioContext = scenarioContext;
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            // Ensure Results directory exists
            var resultsDir = Path.Combine(Directory.GetCurrentDirectory(), "Results");
            if (!Directory.Exists(resultsDir))
            {
                Directory.CreateDirectory(resultsDir);
            }

            // ExtentReports setup
            var htmlReporter = new ExtentHtmlReporter(extentReportPath);
            extent = new ExtentReports();
            extent.AttachReporter(htmlReporter);

            // Performance monitoring başlat
            if (Configuration.EnablePerformanceMonitoring)
            {
                PerformanceMonitor.ClearMetrics();
                Log.Information("Performans monitoring başlatıldı");
            }

            // Driver uyumluluk kontrolü
            if (Configuration.AutoUpdateDrivers)
            {
                Task.Run(async () =>
                {
                    try
                    {
                        await EnhancedBrowser.EnsureDriverCompatibilityAsync(Configuration.Browser);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "Driver uyumluluk kontrolü sırasında hata oluştu");
                    }
                });
            }
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            try
            {
                // Performans raporu oluştur
                if (Configuration.EnablePerformanceMonitoring && Configuration.GeneratePerformanceReport)
                {
                    PerformanceHelper.GeneratePerformanceReport();
                }

                // Yavaş işlemleri logla
                if (Configuration.LogSlowOperations)
                {
                    PerformanceHelper.LogSlowOperations(Configuration.SlowOperationThresholdMs);
                }

                // Browser performans metriklerini logla
                PerformanceMonitor.LogPerformanceMetrics();

                // ExtentReports kapat
                extent?.Flush();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Test run sonrası işlemler sırasında hata oluştu");
            }
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            try
            {
                PerformanceMonitor.StartTimer($"Scenario_{scenarioContext.ScenarioInfo.Title}");
                
                // ExtentReports scenario başlat
                scenarioTest = extent.CreateTest(scenarioContext.ScenarioInfo.Title);
                scenarioTest.Info($"Scenario başlatıldı: {scenarioContext.ScenarioInfo.Title}");

                // Browser başlat
                PerformanceMonitor.StartTimer("Browser_Start");
                webDriver = EnhancedBrowser.InitBrowser(Configuration.Browser);
                PerformanceMonitor.StopTimer("Browser_Start");

                objectContainer.RegisterInstanceAs(webDriver);
                _commonPage = new CommonPage(webDriver);
                objectContainer.RegisterInstanceAs(_commonPage);

                // Browser performansını logla
                EnhancedBrowser.LogBrowserPerformance(webDriver);

                Log.Information($"Scenario başlatıldı: {scenarioContext.ScenarioInfo.Title}");
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Scenario başlatılırken hata oluştu: {scenarioContext.ScenarioInfo.Title}");
                throw;
            }
        }

        [AfterScenario]
        public void AfterScenario()
        {
            try
            {
                var scenarioTime = PerformanceMonitor.StopTimer($"Scenario_{scenarioContext.ScenarioInfo.Title}");
                
                // Scenario sonucunu logla
                var scenarioResult = scenarioContext.TestError != null ? "FAILED" : "PASSED";
                Log.Information($"Scenario tamamlandı: {scenarioContext.ScenarioInfo.Title} - {scenarioResult} ({scenarioTime}ms)");

                // ExtentReports scenario sonucu
                if (scenarioContext.TestError != null)
                {
                    scenarioTest.Fail($"Scenario başarısız: {scenarioContext.TestError.Message}");
                    Log.Error($"Scenario hatası: {scenarioContext.TestError.Message}");
                }
                else
                {
                    scenarioTest.Pass("Scenario başarılı");
                }

                // Screenshot al
                if (Configuration.TakeScreenshotOnFailure && scenarioContext.TestError != null)
                {
                    try
                    {
                        PerformanceMonitor.StartTimer("Screenshot_Failure");
                        var screenshotPath = TakeScreenshot("FAILURE");
                        PerformanceMonitor.StopTimer("Screenshot_Failure");
                        
                        scenarioTest.AddScreenCaptureFromPath(screenshotPath);
                        Log.Information($"Başarısız test screenshot'ı alındı: {screenshotPath}");
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "Screenshot alınırken hata oluştu");
                    }
                }
                else if (Configuration.TakeScreenshotOnSuccess && scenarioContext.TestError == null)
                {
                    try
                    {
                        PerformanceMonitor.StartTimer("Screenshot_Success");
                        var screenshotPath = TakeScreenshot("SUCCESS");
                        PerformanceMonitor.StopTimer("Screenshot_Success");
                        
                        scenarioTest.AddScreenCaptureFromPath(screenshotPath);
                        Log.Information($"Başarılı test screenshot'ı alındı: {screenshotPath}");
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "Screenshot alınırken hata oluştu");
                    }
                }

                // Browser'ı kapat
                PerformanceMonitor.StartTimer("Browser_Close");
                webDriver?.Quit();
                PerformanceMonitor.StopTimer("Browser_Close");

                Log.Information($"Browser kapatıldı, scenario tamamlandı: {scenarioContext.ScenarioInfo.Title}");
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Scenario sonlandırılırken hata oluştu: {scenarioContext.ScenarioInfo.Title}");
            }
        }

        [BeforeStep]
        public void BeforeStep()
        {
            try
            {
                var stepName = $"{scenarioContext.StepContext.StepInfo.StepDefinitionType}_{scenarioContext.StepContext.StepInfo.Text}";
                PerformanceMonitor.StartTimer($"Step_{stepName}");
                
                Log.Information($"Step başlatıldı: {scenarioContext.StepContext.StepInfo.Text}");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Step başlatılırken hata oluştu");
            }
        }

        [AfterStep]
        public void AfterStep()
        {
            try
            {
                var stepName = $"{scenarioContext.StepContext.StepInfo.StepDefinitionType}_{scenarioContext.StepContext.StepInfo.Text}";
                var stepTime = PerformanceMonitor.StopTimer($"Step_{stepName}");
                
                var stepResult = scenarioContext.TestError != null ? "FAILED" : "PASSED";
                Log.Information($"Step tamamlandı: {scenarioContext.StepContext.StepInfo.Text} - {stepResult} ({stepTime}ms)");

                // Yavaş step'leri logla
                if (stepTime > Configuration.SlowOperationThresholdMs)
                {
                    Log.Warning($"Yavaş step tespit edildi: {scenarioContext.StepContext.StepInfo.Text} ({stepTime}ms)");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Step sonlandırılırken hata oluştu");
            }
        }

        private string TakeScreenshot(string status)
        {
            try
            {
                var screenshotDir = Path.Combine(reportPath, "Screenshots");
                if (!Directory.Exists(screenshotDir))
                {
                    Directory.CreateDirectory(screenshotDir);
                }

                var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                var fileName = $"Screenshot_{status}_{timestamp}.{Configuration.ScreenshotFormat}";
                var filePath = Path.Combine(screenshotDir, fileName);

                var screenshot = ((ITakesScreenshot)webDriver).GetScreenshot();
                screenshot.SaveAsFile(filePath, ScreenshotImageFormat.Png);

                return filePath;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Screenshot alınırken hata oluştu");
                return string.Empty;
            }
        }
    }
} 