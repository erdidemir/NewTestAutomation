using OpenQA.Selenium;
using TestAutomationProject.Core;

namespace TestAutomationProject.Pages
{
    public class LoginPage : CommonPage
    {
        // Google Login sayfası elementleri
        private readonly By _emailInput = By.Name("identifier");
        private readonly By _passwordInput = By.Name("password");
        private readonly By _nextButton = By.Id("identifierNext");
        private readonly By _passwordNextButton = By.Id("passwordNext");
        private readonly By _errorMessage = By.XPath("//div[contains(@class, 'error')]");
        private readonly By _googleLogo = By.XPath("//img[@alt='Google']");

        public LoginPage(IWebDriver driver) : base(driver)
        {
        }

        /// <summary>
        /// Google login sayfasına gider
        /// </summary>
        public void NavigateToGoogleLogin()
        {
            _driver.Navigate().GoToUrl("https://accounts.google.com/signin");
            WaitForPageLoad();
        }

        /// <summary>
        /// Email adresini girer
        /// </summary>
        public void EnterEmail(string email)
        {
            WaitAndSendKeys(_emailInput, email);
        }

        /// <summary>
        /// Email sonrası Next butonuna tıklar
        /// </summary>
        public void ClickNextAfterEmail()
        {
            WaitAndClick(_nextButton);
        }

        /// <summary>
        /// Şifreyi girer
        /// </summary>
        public void EnterPassword(string password)
        {
            WaitAndSendKeys(_passwordInput, password);
        }

        /// <summary>
        /// Şifre sonrası Next butonuna tıklar
        /// </summary>
        public void ClickNextAfterPassword()
        {
            WaitAndClick(_passwordNextButton);
        }

        /// <summary>
        /// Google hesabına giriş yapar
        /// </summary>
        public void LoginToGoogle(string email, string password)
        {
            NavigateToGoogleLogin();
            EnterEmail(email);
            ClickNextAfterEmail();
            Wait(2000); // Şifre alanının yüklenmesi için bekleme
            EnterPassword(password);
            ClickNextAfterPassword();
        }

        /// <summary>
        /// Hata mesajını alır
        /// </summary>
        public string GetErrorMessage()
        {
            return WaitAndGetText(_errorMessage);
        }

        /// <summary>
        /// Google logosunun görünür olup olmadığını kontrol eder
        /// </summary>
        public bool IsGoogleLogoVisible()
        {
            return WaitAndIsDisplayed(_googleLogo);
        }

        /// <summary>
        /// Login sayfasının yüklendiğini kontrol eder
        /// </summary>
        public bool IsLoginPageLoaded()
        {
            return WaitAndIsDisplayed(_emailInput);
        }
    }
}
