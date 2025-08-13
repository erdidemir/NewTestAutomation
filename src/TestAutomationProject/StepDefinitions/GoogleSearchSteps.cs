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

        [Given(@"the user is on Google homepage")]
        public void GivenTheUserIsOnGoogleHomepage()
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

        [When(@"the user types ""(.*)"" in the search box")]
        public void WhenTheUserTypesInTheSearchBox(string searchText)
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

        [When(@"the user clicks the search button")]
        public void WhenTheUserClicksTheSearchButton()
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

        [When(@"the user searches ""(.*)"" on Google")]
        public void WhenTheUserSearchesOnGoogle(string searchText)
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

        [When(@"the user clicks the lucky button")]
        public void WhenTheUserClicksTheLuckyButton()
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

        [Then(@"Google logo should be visible")]
        public void ThenGoogleLogoShouldBeVisible()
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

        [Then(@"search box should be visible")]
        public void ThenSearchBoxShouldBeVisible()
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

        [Then(@"search results should be visible")]
        public void ThenSearchResultsShouldBeVisible()
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

        [Then(@"search box should contain ""(.*)""")]
        public void ThenSearchBoxShouldContain(string expectedText)
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

        [Then(@"page title should contain ""(.*)""")]
        public void ThenPageTitleShouldContain(string expectedTitle)
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

        [Then(@"Google homepage should be loaded")]
        public void ThenGoogleHomepageShouldBeLoaded()
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