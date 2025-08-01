using BoDi;
using NUnit.Framework;
using OpenQA.Selenium;
using TechTalk.SpecFlow;
using System.Threading;

namespace TestAutomationProject.StepDefinitions
{
    [Binding]
    public class CommonSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;
        private IWebDriver _driver;

        public CommonSteps(IObjectContainer objectContainer, ScenarioContext scenarioContext)
        {
            _objectContainer = objectContainer;
            _scenarioContext = scenarioContext;
        }

        [BeforeScenario]
        public void Setup()
        {
            // WebDriver'ı Hooks'tan al
            if (_objectContainer.IsRegistered<IWebDriver>())
            {
                _driver = _objectContainer.Resolve<IWebDriver>();
            }
        }

        [Given(@"kullanıcı test için hazır")]
        public void GivenKullaniciTestIcinHazir()
        {
            // WebDriver'ı tekrar kontrol et
            if (_driver == null && _objectContainer.IsRegistered<IWebDriver>())
            {
                _driver = _objectContainer.Resolve<IWebDriver>();
            }
            
            // Test öncesi hazırlık işlemleri buraya eklenebilir
            Assert.That(_driver, Is.Not.Null, "WebDriver başlatılamadı");
            
            // 5 saniye bekle
            Thread.Sleep(5000);
        }

        [When(@"testi başlat")]
        public void WhenTestiBaslat()
        {
            // Test başlatma işlemleri
            Assert.That(_driver.Url.Contains("google.com"), Is.True, "Google sayfası açılamadı");
            
            // 5 saniye bekle
            Thread.Sleep(5000);
        }
    }
} 