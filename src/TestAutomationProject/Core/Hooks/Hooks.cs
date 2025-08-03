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

        public Hooks(IObjectContainer objectContainer, ScenarioContext scenarioContext)
        {
            this.objectContainer = objectContainer;
            this.scenarioContext = scenarioContext;
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
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
                // Screenshot logic removed for now
            }
        }

        [AfterStep]
        public void AfterStep(ScenarioContext context)
        {
            if (context.TestError != null)
            {
                Serilog.Log.Error("Test Step Failed | {0}", context.TestError.Message);
            }
            else
            {
                var stepType = scenarioContext.StepContext.StepInfo.StepDefinitionType.ToString();

                if (stepType == "Then")
                {
                    // Success screenshot logic removed for now
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
    }
}
