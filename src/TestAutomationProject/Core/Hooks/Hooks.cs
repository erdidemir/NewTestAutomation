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
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;

namespace TestAutomationProject.Core.Hooks
{
    [Binding]
    class Hooks
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

        public Hooks(IObjectContainer objectContainer, ScenarioContext scenarioContext)
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
            htmlReporter.Config.Theme = AventStack.ExtentReports.Reporter.Configuration.Theme.Dark;
            extent = new ExtentReports();
            extent.AttachReporter(htmlReporter);

            Serilog.Log.Information("ExtentReport will be saved to: {0}", extentReportPath);

            LoggingLevelSwitch levelSwitch = new LoggingLevelSwitch(Serilog.Events.LogEventLevel.Debug);
            Serilog.Log.Logger = new LoggerConfiguration()
                .MinimumLevel.ControlledBy(levelSwitch)
                .WriteTo.File(reportPath + @"/Logs",
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} | {Level:u3} | {Message} {NewLine}",
                    rollingInterval: RollingInterval.Day).CreateLogger();
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            try
            {
                if (extent != null)
                {
                    extent.Flush();
                    Serilog.Log.Information("ExtentReport flushed to: {0}", extentReportPath);
                    
                    // Check if file was actually created
                    if (File.Exists(extentReportPath))
                    {
                        Serilog.Log.Information("ExtentReport file created successfully at: {0}", extentReportPath);
                        
                        // Copy to Results folder for easier access
                        var resultsPath = Path.Combine(Directory.GetCurrentDirectory(), "Results", "index.html");
                        if (File.Exists(extentReportPath))
                        {
                            File.Copy(extentReportPath, resultsPath, true);
                            Serilog.Log.Information("ExtentReport copied to: {0}", resultsPath);
                        }
                    }
                    else
                    {
                        Serilog.Log.Warning("ExtentReport file was not created at: {0}", extentReportPath);
                    }
                }
                else
                {
                    Serilog.Log.Warning("ExtentReports object is null, cannot flush report");
                }
            }
            catch (Exception ex)
            {
                Serilog.Log.Error("Error flushing ExtentReport: {0}", ex.Message);
            }
        }

        [AfterStep]
        public void AfterStep(ScenarioContext context)
        {
            if (context.TestError != null)
            {
                Serilog.Log.Error("Test Step Failed | {0}", context.TestError.Message);
                if (Configuration.TakeScreenshotOnFailure)
                {
                    var screenshotPath = TakeScreenshot("Failure_Screenshot", context.TestError.Message);
                    scenarioTest.Fail(context.TestError.Message, MediaEntityBuilder.CreateScreenCaptureFromPath(screenshotPath).Build());
                }
            }
            else
            {
                var stepType = scenarioContext.StepContext.StepInfo.StepDefinitionType.ToString();
                if (stepType == "Then" && Configuration.TakeScreenshotOnSuccess)
                {
                    var screenshotPath = TakeScreenshot("Success_Screenshot", "Step completed successfully");
                    scenarioTest.Pass(scenarioContext.StepContext.StepInfo.Text, MediaEntityBuilder.CreateScreenCaptureFromPath(screenshotPath).Build());
                }
                else
                {
                    scenarioTest.Pass(scenarioContext.StepContext.StepInfo.Text);
                }
            }
        }

        [BeforeScenario]
        public void InitializeWebDriver(ScenarioContext context)
        {
            if (!scenarioContext.ScenarioInfo.Tags.Contains("API"))
            {
                webDriver = Browser.InitBrowser(Configuration.BrowserName);
                if (Configuration.IsBrowserHeadless)
                    webDriver.Manage().Window.Size = new Size(1920, 1080);
                else
                    webDriver.Manage().Window.Maximize();

                objectContainer.RegisterInstanceAs(webDriver);
            }

            Serilog.Log.Information("Selecting scenario {0} to run", context.ScenarioInfo.Title);

            // ExtentReports scenario
            scenarioTest = extent.CreateTest(context.ScenarioInfo.Title);
        }

        [AfterScenario]
        public void CloseBrowser()
        {
            if (objectContainer.IsRegistered<IWebDriver>())
            {
                objectContainer.Resolve<IWebDriver>().Close();
                objectContainer.Resolve<IWebDriver>().Dispose();
            }
        }

        [BeforeFeature]
        public static void BeforeFeature(FeatureContext context)
        {
            Serilog.Log.Information("Selecting feature file {0} to run", context.FeatureInfo.Title);
            randomText = DateTime.Now.ToString("yyMMddHHmmss");
        }

        [AfterFeature]
        public static void CloseLogger()
        {
            // Logger.Instance.Close();
        }

        private string TakeScreenshot(string screenshotName, string description)
        {
            try
            {
                if (webDriver != null)
                {
                    var screenshot = ((ITakesScreenshot)webDriver).GetScreenshot();
                    var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                    var fileName = $"{screenshotName}_{timestamp}.{Configuration.ScreenshotFormat}";
                    var screenshotPath = Path.Combine(reportPath, "Screenshots", fileName);
                    Directory.CreateDirectory(Path.GetDirectoryName(screenshotPath));
                    screenshot.SaveAsFile(screenshotPath, ScreenshotImageFormat.Png);
                    Serilog.Log.Information("Screenshot saved: {0}", screenshotPath);
                    return screenshotPath;
                }
            }
            catch (Exception ex)
            {
                Serilog.Log.Error("Error taking screenshot: {0}", ex.Message);
            }
            return null;
        }
    }
}
