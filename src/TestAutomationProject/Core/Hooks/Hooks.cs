using BoDi;
using OpenQA.Selenium;
using System.Drawing;
using Allure.Commons;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using TechTalk.SpecFlow;
using Status = Allure.Commons.Status;
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
        private static InteroperabilityPage _interoperabilityPage;
        private static string reportPath = System.IO.Directory.GetParent(@"../../../").FullName
                                           + Path.DirectorySeparatorChar + "Result"
                                           + Path.DirectorySeparatorChar + "Result_" +
                                           DateTime.Now.ToString("ddMMyyyy HHmmss");

        public Hooks(IObjectContainer objectContainer, ScenarioContext scenarioContext)
        {
            this.objectContainer = objectContainer;
            this.scenarioContext = scenarioContext;
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            // AllureLifecycle.Instance.CleanupResultDirectory();
            LoggingLevelSwitch levelSwitch = new LoggingLevelSwitch(Serilog.Events.LogEventLevel.Debug);
            Serilog.Log.Logger = new LoggerConfiguration()
                .MinimumLevel.ControlledBy(levelSwitch)
                .WriteTo.File(reportPath + @"/Logs",
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} | {Level:u3} | {Message} {NewLine}",
                    rollingInterval: RollingInterval.Day).CreateLogger();

            // ExtentReports kurulumu - geçici olarak kaldırıldı
            // var htmlReporter = new ExtentHtmlReporter(@"extent-reports/extent-report.html");
            // htmlReporter.Config.Theme = AventStack.ExtentReports.Reporter.Configuration.Theme.Dark;
            // extent = new ExtentReports();
            // extent.AttachReporter(htmlReporter);
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            // Finalizing and saving report  
            // extent.Flush();
        }

        [BeforeStep]
        public void BeforeStep()
        {
            List<IWebElement> e = new List<IWebElement>();
            string errorMessage = "Error Message";
            try
            {
                webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
                var GetErrorMessageElement =
                    webDriver.FindElement(
                        By.XPath($"(//*[@id='toast-container']/div/div[text()=' Error '])[1]"));
                e.Add(GetErrorMessageElement);

                var GetErrorMessageElementErrorMessage = webDriver.FindElement(By.XPath($"(//*[@id='toast-container']/div/div[text()=' Error '])[1]/following-sibling::div"));
                errorMessage = GetErrorMessageElementErrorMessage.Text;
            }
            catch
            {
            }

            if (e.Count > 0)
            {
                // byte[] successScreenshot = ((ITakesScreenshot)webDriver).GetScreenshot().AsByteArray;
                // AllureLifecycle.Instance.AddAttachment(errorMessage, "image/png", successScreenshot);
                // var screenshot = ((ITakesScreenshot)webDriver).GetScreenshot().AsBase64EncodedString;
                // scenarioTest.Info(scenarioContext.StepContext.StepInfo.Text, MediaEntityBuilder.CreateScreenCaptureFromBase64String(screenshot).Build());
            }

            // scenarioTest.CreateNode(scenarioContext.StepContext.StepInfo.Text);
        }

        [AfterStep]
        public void AfterStep(ScenarioContext context)
        {
            if (context.TestError != null)
            {
                //Allure Reporting
                Serilog.Log.Error("Test Step Failed | {0}", context.TestError.Message);
                // byte[] screenshot = ((ITakesScreenshot)webDriver).GetScreenshot().AsByteArray;
                // AllureLifecycle.Instance.AddAttachment("Failed Screenshot", "image/png", screenshot);

                //Extend Reporting
                // var screenshotEncoded = ((ITakesScreenshot)webDriver).GetScreenshot().AsBase64EncodedString;
                // scenarioTest.Fail(scenarioContext.TestError.Message, MediaEntityBuilder.CreateScreenCaptureFromBase64String(screenshotEncoded).Build());
            }
            else
            {
                var stepType = scenarioContext.StepContext.StepInfo.StepDefinitionType.ToString();

                if (stepType == "Then")
                {
                    //Allure Reporting
                    // byte[] successScreenshot = ((ITakesScreenshot)webDriver).GetScreenshot().AsByteArray;
                    // AllureLifecycle.Instance.AddAttachment("Success Screenshot", "image/png", successScreenshot);

                    //Extend Reporting
                    // var screenshot = ((ITakesScreenshot)webDriver).GetScreenshot().AsBase64EncodedString;
                    // scenarioTest.Pass(scenarioContext.StepContext.StepInfo.Text,
                    //     MediaEntityBuilder.CreateScreenCaptureFromBase64String(screenshot).Build());
                }
                else
                {
                    // scenarioTest.Pass(scenarioContext.StepContext.StepInfo.Text);
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

            // scenarioTest = extent.CreateTest(context.ScenarioInfo.Title);
        }

        [AfterScenario]
        public void CloseBrowser()
        {
            if (objectContainer.IsRegistered<IWebDriver>())
            {
                objectContainer.Resolve<IWebDriver>().Close();
                objectContainer.Resolve<IWebDriver>().Dispose();
            }

            // if (scenarioContext.TestError != null)
            // {
            //     scenarioTest.Fail(scenarioContext.TestError.Message);
            // }
            // else
            // {
            //     scenarioTest.Pass("Scenario passed successfully.");
            // }
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
    }
}
