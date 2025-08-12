using TechTalk.SpecFlow;
using Microsoft.Extensions.Configuration;
using ApiTestAutomationProject.Steps;
using BoDi;

namespace ApiTestAutomationProject.Hooks
{
    [Binding]
    public class SpecFlowHooks
    {
        private readonly IObjectContainer _objectContainer;

        public SpecFlowHooks(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            // Test run başlamadan önce yapılacak işlemler
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            // Her senaryo başlamadan önce dependency injection'ı yapılandır
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            DependencyInjection.RegisterSpecFlowDependencies(_objectContainer, configuration);
        }

        [AfterScenario]
        public void AfterScenario()
        {
            // Her senaryo sonrasında temizlik işlemleri
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            // Test run sonrasında yapılacak işlemler
        }
    }
} 