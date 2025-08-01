using OpenQA.Selenium;

namespace TestAutomationProject.Pages
{
    public class LoginPage : Page
    {
        private readonly string _usernameInputId = "Username";
        private readonly string _passwordInputId = "Password";
        private readonly string _loginButtonXPath = "//button[@type='submit']";
        private readonly string _errorMessageXPath = "//div[contains(@class, 'error')]";

        public LoginPage(IWebDriver driver) : base(driver)
        {
        }

        public void EnterUsername(string username)
        {
            var usernameInput = FindElementById(_usernameInputId);
            usernameInput.Clear();
            usernameInput.SendKeys(username);
        }

        public void EnterPassword(string password)
        {
            var passwordInput = FindElementById(_passwordInputId);
            passwordInput.Clear();
            passwordInput.SendKeys(password);
        }

        public void ClickLoginButton()
        {
            var loginButton = FindElementByXpath(_loginButtonXPath);
            loginButton.Click();
        }

        public void Login(string username, string password)
        {
            EnterUsername(username);
            EnterPassword(password);
            ClickLoginButton();
        }

        public bool IsLoginErrorVisible()
        {
            try
            {
                var error = FindElementByXpath(_errorMessageXPath, 5);
                return error.Displayed;
            }
            catch
            {
                return false;
            }
        }
    }
}
