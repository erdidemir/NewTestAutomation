using BoDi;
using NUnit.Framework;
using OpenQA.Selenium;
using TechTalk.SpecFlow;
using TestAutomationProject.Pages;

namespace TestAutomationProject.StepDefinitions
{
    [Binding]
    public class GoogleLoginSteps
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;
        private IWebDriver _driver;
        private LoginPage _loginPage;

        public GoogleLoginSteps(IObjectContainer objectContainer, ScenarioContext scenarioContext)
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
                _loginPage = new LoginPage(_driver);
            }
        }

        [Given(@"kullanıcı Google login sayfasında")]
        public void GivenKullaniciGoogleLoginSayfasinda()
        {
            _loginPage.NavigateToGoogleLogin();
            Assert.That(_loginPage.IsLoginPageLoaded(), Is.True, "Google login sayfası yüklenemedi");
        }

        [When(@"kullanıcı email adresini girer: ""(.*)""")]
        public void WhenKullaniciEmailAdresiniGirer(string email)
        {
            _loginPage.EnterEmail(email);
        }

        [When(@"kullanıcı email sonrası Next butonuna tıklar")]
        public void WhenKullaniciEmailSonrasiNextButonunaTiklar()
        {
            _loginPage.ClickNextAfterEmail();
        }

        [When(@"kullanıcı şifresini girer: ""(.*)""")]
        public void WhenKullaniciSifresiniGirer(string password)
        {
            _loginPage.EnterPassword(password);
        }

        [When(@"kullanıcı şifre sonrası Next butonuna tıklar")]
        public void WhenKullaniciSifreSonrasiNextButonunaTiklar()
        {
            _loginPage.ClickNextAfterPassword();
        }

        [When(@"kullanıcı Google hesabına giriş yapar: ""(.*)"" ve ""(.*)""")]
        public void WhenKullaniciGoogleHesabinaGirisYapar(string email, string password)
        {
            _loginPage.LoginToGoogle(email, password);
        }

        [Then(@"Google logosu görünür olmalı")]
        public void ThenGoogleLogosuGorunurOlmali()
        {
            Assert.That(_loginPage.IsGoogleLogoVisible(), Is.True, "Google logosu görünür değil");
        }

        [Then(@"hata mesajı görünür olmalı")]
        public void ThenHataMesajiGorunurOlmali()
        {
            string errorMessage = _loginPage.GetErrorMessage();
            Assert.That(string.IsNullOrEmpty(errorMessage), Is.False, "Hata mesajı görünür değil");
        }

        [Then(@"hata mesajı şu metni içermeli: ""(.*)""")]
        public void ThenHataMesajiSuMetniIcermeli(string expectedErrorMessage)
        {
            string actualErrorMessage = _loginPage.GetErrorMessage();
            Assert.That(actualErrorMessage.Contains(expectedErrorMessage), Is.True, 
                $"Beklenen hata mesajı: {expectedErrorMessage}, Gerçek hata mesajı: {actualErrorMessage}");
        }

        [Then(@"login sayfası yüklenmiş olmalı")]
        public void ThenLoginSayfasiYuklenmisOlmali()
        {
            Assert.That(_loginPage.IsLoginPageLoaded(), Is.True, "Login sayfası yüklenmedi");
        }
    }
} 