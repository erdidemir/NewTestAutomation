using BoDi;
using NUnit.Framework;
using OpenQA.Selenium;
using TechTalk.SpecFlow;
using TestAutomationProject.Pages;
using System.Threading;

namespace TestAutomationProject.StepDefinitions
{
    [Binding]
    public class GoogleSearchSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;
        private IWebDriver _driver;
        private GoogleSearchPage _googleSearchPage;

        public GoogleSearchSteps(IObjectContainer objectContainer, ScenarioContext scenarioContext)
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
                _googleSearchPage = new GoogleSearchPage(_driver);
            }
        }

        [Given(@"kullanıcı Google ana sayfasında")]
        public void GivenKullaniciGoogleAnaSayfasinda()
        {
            // _googleSearchPage null kontrolü
            if (_googleSearchPage == null)
            {
                if (_driver == null && _objectContainer.IsRegistered<IWebDriver>())
                {
                    _driver = _objectContainer.Resolve<IWebDriver>();
                }
                _googleSearchPage = new GoogleSearchPage(_driver);
            }

            Assert.That(_googleSearchPage, Is.Not.Null, "GoogleSearchPage oluşturulamadı");
            _googleSearchPage.NavigateToGoogle();
            
            // Logo kontrolü yerine arama kutusu kontrolü yap
            Assert.That(_googleSearchPage.IsSearchInputVisible(), Is.True, "Arama kutusu görünür değil");
            
            // 5 saniye bekle
            Thread.Sleep(5000);
        }

        [When(@"kullanıcı arama kutusuna ""(.*)"" yazar")]
        public void WhenKullaniciAramaKutusunaYazar(string searchText)
        {
            // _googleSearchPage null kontrolü
            if (_googleSearchPage == null)
            {
                if (_driver == null && _objectContainer.IsRegistered<IWebDriver>())
                {
                    _driver = _objectContainer.Resolve<IWebDriver>();
                }
                _googleSearchPage = new GoogleSearchPage(_driver);
            }

            _googleSearchPage.EnterSearchText(searchText);
            
            // 5 saniye bekle
            Thread.Sleep(5000);
        }

        [When(@"kullanıcı arama butonuna tıklar")]
        public void WhenKullaniciAramaButonunaTiklar()
        {
            // _googleSearchPage null kontrolü
            if (_googleSearchPage == null)
            {
                if (_driver == null && _objectContainer.IsRegistered<IWebDriver>())
                {
                    _driver = _objectContainer.Resolve<IWebDriver>();
                }
                _googleSearchPage = new GoogleSearchPage(_driver);
            }

            _googleSearchPage.ClickSearchButton();
            
            // 5 saniye bekle
            Thread.Sleep(5000);
        }

        [When(@"kullanıcı Google'da ""(.*)"" arar")]
        public void WhenKullaniciGoogleDaArar(string searchText)
        {
            // _googleSearchPage null kontrolü
            if (_googleSearchPage == null)
            {
                if (_driver == null && _objectContainer.IsRegistered<IWebDriver>())
                {
                    _driver = _objectContainer.Resolve<IWebDriver>();
                }
                _googleSearchPage = new GoogleSearchPage(_driver);
            }

            _googleSearchPage.SearchInGoogle(searchText);
            
            // 5 saniye bekle
            Thread.Sleep(5000);
        }

        [When(@"kullanıcı şanslı ol butonuna tıklar")]
        public void WhenKullaniciSansliOlButonunaTiklar()
        {
            // _googleSearchPage null kontrolü
            if (_googleSearchPage == null)
            {
                if (_driver == null && _objectContainer.IsRegistered<IWebDriver>())
                {
                    _driver = _objectContainer.Resolve<IWebDriver>();
                }
                _googleSearchPage = new GoogleSearchPage(_driver);
            }

            _googleSearchPage.ClickLuckyButton();
            
            // 5 saniye bekle
            Thread.Sleep(5000);
        }

        [Then(@"Google logosu görünür olmalı")]
        public void ThenGoogleLogosuGorunurOlmali()
        {
            // _googleSearchPage null kontrolü
            if (_googleSearchPage == null)
            {
                if (_driver == null && _objectContainer.IsRegistered<IWebDriver>())
                {
                    _driver = _objectContainer.Resolve<IWebDriver>();
                }
                _googleSearchPage = new GoogleSearchPage(_driver);
            }

            Assert.That(_googleSearchPage.IsGoogleLogoVisible(), Is.True, "Google logosu görünür değil");
            
            // 5 saniye bekle
            Thread.Sleep(5000);
        }

        [Then(@"arama kutusu görünür olmalı")]
        public void ThenAramaKutusuGorunurOlmali()
        {
            // _googleSearchPage null kontrolü
            if (_googleSearchPage == null)
            {
                if (_driver == null && _objectContainer.IsRegistered<IWebDriver>())
                {
                    _driver = _objectContainer.Resolve<IWebDriver>();
                }
                _googleSearchPage = new GoogleSearchPage(_driver);
            }

            Assert.That(_googleSearchPage.IsSearchInputVisible(), Is.True, "Arama kutusu görünür değil");
            
            // 5 saniye bekle
            Thread.Sleep(5000);
        }

        [Then(@"arama sonuçları görünür olmalı")]
        public void ThenAramaSonuclariGorunurOlmali()
        {
            // _googleSearchPage null kontrolü
            if (_googleSearchPage == null)
            {
                if (_driver == null && _objectContainer.IsRegistered<IWebDriver>())
                {
                    _driver = _objectContainer.Resolve<IWebDriver>();
                }
                _googleSearchPage = new GoogleSearchPage(_driver);
            }

            Assert.That(_googleSearchPage.AreSearchResultsVisible(), Is.True, "Arama sonuçları görünür değil");
            
            // 5 saniye bekle
            Thread.Sleep(5000);
        }

        [Then(@"arama kutusunda ""(.*)"" yazmalı")]
        public void ThenAramaKutusundaYazmali(string expectedText)
        {
            // _googleSearchPage null kontrolü
            if (_googleSearchPage == null)
            {
                if (_driver == null && _objectContainer.IsRegistered<IWebDriver>())
                {
                    _driver = _objectContainer.Resolve<IWebDriver>();
                }
                _googleSearchPage = new GoogleSearchPage(_driver);
            }

            string actualText = _googleSearchPage.GetSearchInputText();
            Assert.That(actualText, Is.EqualTo(expectedText), $"Beklenen metin: {expectedText}, Gerçek metin: {actualText}");
            
            // 5 saniye bekle
            Thread.Sleep(5000);
        }

        [Then(@"sayfa başlığı ""(.*)"" içermeli")]
        public void ThenSayfaBasligiIcermeli(string expectedTitle)
        {
            // _googleSearchPage null kontrolü
            if (_googleSearchPage == null)
            {
                if (_driver == null && _objectContainer.IsRegistered<IWebDriver>())
                {
                    _driver = _objectContainer.Resolve<IWebDriver>();
                }
                _googleSearchPage = new GoogleSearchPage(_driver);
            }

            string actualTitle = _googleSearchPage.GetPageTitle();
            Assert.That(actualTitle, Does.Contain(expectedTitle), $"Sayfa başlığı '{expectedTitle}' içermiyor. Gerçek başlık: {actualTitle}");
            
            // 5 saniye bekle
            Thread.Sleep(5000);
        }

        [Then(@"Google ana sayfası yüklenmiş olmalı")]
        public void ThenGoogleAnaSayfasiYuklenmisOlmali()
        {
            // _googleSearchPage null kontrolü
            if (_googleSearchPage == null)
            {
                if (_driver == null && _objectContainer.IsRegistered<IWebDriver>())
                {
                    _driver = _objectContainer.Resolve<IWebDriver>();
                }
                _googleSearchPage = new GoogleSearchPage(_driver);
            }

            Assert.That(_googleSearchPage.IsGoogleLogoVisible(), Is.True, "Google ana sayfası yüklenemedi");
            
            // 5 saniye bekle
            Thread.Sleep(5000);
        }
    }
} 