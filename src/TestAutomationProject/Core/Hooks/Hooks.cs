using System;
using System.Drawing;
using System.IO;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using BoDi;
using OpenQA.Selenium;
using Serilog;
using Serilog.Core;
using TechTalk.SpecFlow;
using System.Collections.Generic;

namespace TestAutomationProject.Core.Hooks
{
    [Binding]
    class Hooks
    {
        private static ExtentReports extent;
        private static ExtentTest scenarioTest;

        private readonly IObjectContainer objectContainer;
        private readonly ScenarioContext scenarioContext;
        private IWebDriver webDriver;

        private static readonly Random _random = new Random();
        private static string reportPath = Path.Combine(Directory.GetCurrentDirectory(), "Results", DateTime.Now.ToString("ddMMyyyy_HHmmss"));
        private static string extentReportPath = Path.Combine(reportPath, "ExtentReport.html");
        
        // Allure step'leri için liste
        private static List<object> currentScenarioSteps = new List<object>();

        public Hooks(IObjectContainer objectContainer, ScenarioContext scenarioContext)
        {
            this.objectContainer = objectContainer;
            this.scenarioContext = scenarioContext;
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            Directory.CreateDirectory(reportPath);
            Directory.CreateDirectory(Path.Combine(reportPath, "Screenshots"));
            Directory.CreateDirectory(Path.Combine(reportPath, "Logs"));

            var htmlReporter = new ExtentHtmlReporter(extentReportPath);
            htmlReporter.Config.Theme = AventStack.ExtentReports.Reporter.Configuration.Theme.Dark;
            extent = new ExtentReports();
            extent.AttachReporter(htmlReporter);

            LoggingLevelSwitch levelSwitch = new LoggingLevelSwitch(Serilog.Events.LogEventLevel.Debug);
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.ControlledBy(levelSwitch)
                .WriteTo.File(Path.Combine(reportPath, "Logs", "log.txt"),
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} | {Level:u3} | {Message}{NewLine}",
                    rollingInterval: RollingInterval.Day)
                .CreateLogger();

            Log.Information("Test başladı. Rapor yolu: {0}", extentReportPath);
        }

        [BeforeScenario]
        public void InitializeScenario()
        {
            // Step listesini temizle
            currentScenarioSteps.Clear();
            
            if (!scenarioContext.ScenarioInfo.Tags.Contains("API"))
            {
                webDriver = Browser.InitBrowser(Configuration.BrowserName);
                if (Configuration.IsBrowserHeadless)
                    webDriver.Manage().Window.Size = new Size(1920, 1080);
                else
                    webDriver.Manage().Window.Maximize();

                objectContainer.RegisterInstanceAs(webDriver);
            }

            scenarioTest = extent.CreateTest(scenarioContext.ScenarioInfo.Title);
            Log.Information("Senaryo başlıyor: {0}", scenarioContext.ScenarioInfo.Title);
        }

        [AfterStep]
        public void AfterEachStep()
        {
            var stepInfo = scenarioContext.StepContext.StepInfo.Text;
            var stepType = scenarioContext.StepContext.StepInfo.StepDefinitionType.ToString();

            // Then adımında ekran görüntüsü al
            string screenshotPath = null;
            if (stepType == "Then")
            {
                screenshotPath = TakeScreenshot("Then_" + stepInfo.Replace(" ", "_"), stepInfo);
            }

            // Allure step'ini listeye ekle
            var allureStep = new
            {
                name = stepInfo,
                status = scenarioContext.TestError != null ? "failed" : "passed",
                stage = "finished",
                start = DateTimeOffset.UtcNow.AddSeconds(-2).ToUnixTimeMilliseconds(),
                stop = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                statusDetails = scenarioContext.TestError != null ? new
                {
                    message = scenarioContext.TestError.Message,
                    trace = scenarioContext.TestError.StackTrace
                } : null,
                attachments = !string.IsNullOrEmpty(screenshotPath) ? new object[]
                {
                    new
                    {
                        name = "Screenshot",
                        type = "image/png",
                        source = screenshotPath
                    }
                } : new object[] { }
            };
            currentScenarioSteps.Add(allureStep);

            if (scenarioContext.TestError != null)
            {
                Log.Error("Adım başarısız: {0} | Hata: {1}", stepInfo, scenarioContext.TestError.Message);
                var failureScreenshotPath = TakeScreenshot("Failure_" + stepType, scenarioContext.TestError.Message);
                scenarioTest.Fail(scenarioContext.TestError.Message, MediaEntityBuilder.CreateScreenCaptureFromPath(failureScreenshotPath).Build());
            }
            else
            {
                if (stepType == "Then" && Configuration.TakeScreenshotOnSuccess)
                {
                    var successScreenshotPath = TakeScreenshot("Success_" + stepType, "Success");
                    scenarioTest.Pass(stepInfo, MediaEntityBuilder.CreateScreenCaptureFromPath(successScreenshotPath).Build());
                }
                else
                {
                    scenarioTest.Pass(stepInfo);
                }
            }
        }

        [AfterScenario]
        public void AfterScenario()
        {
            // Allure results JSON dosyası oluştur
            CreateAllureResults();
            
            if (objectContainer.IsRegistered<IWebDriver>())
            {
                var driver = objectContainer.Resolve<IWebDriver>();
                driver.Close();
                driver.Dispose();
            }
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            extent.Flush();
            Log.Information("Rapor tamamlandı: {0}", extentReportPath);
        }

        private void CreateAllureResults()
        {
            try
            {
                if (scenarioContext == null)
                {
                    Log.Warning("ScenarioContext null, Allure results oluşturulamıyor");
                    return;
                }

                var allureResultsPath = Path.Combine(Directory.GetCurrentDirectory(), "allure-results");
                if (!Directory.Exists(allureResultsPath))
                {
                    Directory.CreateDirectory(allureResultsPath);
                }

                var testResult = new
                {
                    uuid = Guid.NewGuid().ToString(),
                    name = scenarioContext.ScenarioInfo.Title,
                    fullName = $"TestAutomationProject.Features.GoogleAramaTestleriFeature.{scenarioContext.ScenarioInfo.Title}",
                    status = scenarioContext.TestError != null ? "failed" : "passed",
                    statusDetails = new
                    {
                        message = scenarioContext.TestError?.Message ?? "",
                        trace = scenarioContext.TestError?.StackTrace ?? ""
                    },
                    stage = "finished",
                    start = DateTimeOffset.UtcNow.AddSeconds(-10).ToUnixTimeMilliseconds(),
                    stop = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                    description = "Google arama testi",
                    severity = "normal",
                    links = new object[] { },
                    labels = new object[]
                    {
                        new { name = "suite", value = "GoogleAramaTestleriFeature" },
                        new { name = "testClass", value = "GoogleAramaTestleriFeature" },
                        new { name = "testMethod", value = scenarioContext.ScenarioInfo.Title },
                        new { name = "package", value = "TestAutomationProject.Features" },
                        new { name = "framework", value = "NUnit" },
                        new { name = "language", value = "C#" },
                        new { name = "severity", value = "normal" }
                    },
                    parameters = new object[] { },
                    steps = currentScenarioSteps.ToArray(),
                    attachments = new object[] { }
                };

                var json = System.Text.Json.JsonSerializer.Serialize(testResult, new System.Text.Json.JsonSerializerOptions 
                { 
                    WriteIndented = true,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                });

                var fileName = $"test-result.json";
                var filePath = Path.Combine(allureResultsPath, fileName);
                
                File.WriteAllText(filePath, json);
                Log.Information($"Allure test sonucu oluşturuldu: {filePath}");
            }
            catch (Exception ex)
            {
                Log.Error("Allure results oluşturulurken hata: {0}", ex.Message);
            }
        }

        private string TakeScreenshot(string name, string description)
        {
            try
            {
                if (webDriver != null)
                {
                    var screenshot = ((ITakesScreenshot)webDriver).GetScreenshot();
                    var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                    var fileName = $"{name}_{timestamp}.{Configuration.ScreenshotFormat}";
                    var path = Path.Combine(reportPath, "Screenshots", fileName);
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                    screenshot.SaveAsFile(path, ScreenshotImageFormat.Png);
                    Log.Information("Ekran görüntüsü alındı: {0}", path);
                    return path;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Ekran görüntüsü alınamadı: {0}", ex.Message);
            }
            return null;
        }

        [BeforeFeature]
        public static void BeforeFeature(FeatureContext context)
        {
            Log.Information("Feature başlıyor: {0}", context.FeatureInfo.Title);
        }
    }
}
