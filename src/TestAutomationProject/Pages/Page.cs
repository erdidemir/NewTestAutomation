using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using TestAutomationProject.Core;

namespace TestAutomationProject.Pages
{
    public abstract class Page
    {
        protected IWebDriver _driver;

        protected Page(IWebDriver driver)
        {
            _driver = driver;
        }

        public IWebElement FindElementByXpath(string xpath, int? timeoutSeconds = null)
        {
            return WaitUntilElementIsVisible(By.XPath(xpath), timeoutSeconds);
        }

        public IWebElement FindElementById(string id, int? timeoutSeconds = null)
        {
            return WaitUntilElementIsVisible(By.Id(id), timeoutSeconds);
        }

        public IWebElement WaitUntilElementIsVisible(By by, int? timeoutSeconds = null)
        {
            int waitTime = timeoutSeconds ?? Configuration.ElementDelayMilliSeconds;
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(waitTime));
            return wait.Until(driver =>
            {
                try
                {
                    var element = driver.FindElement(by);
                    return element.Displayed ? element : null;
                }
                catch (NoSuchElementException)
                {
                    return null;
                }
                catch (StaleElementReferenceException)
                {
                    return null;
                }
            });
        }

        public IWebElement WaitUntilElementIsClickable(By by, int? timeoutSeconds = null)
        {
            int waitTime = timeoutSeconds ?? Configuration.ElementDelayMilliSeconds;
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(waitTime));
            return wait.Until(driver =>
            {
                try
                {
                    var element = driver.FindElement(by);
                    return (element != null && element.Displayed && element.Enabled) ? element : null;
                }
                catch (NoSuchElementException)
                {
                    return null;
                }
                catch (StaleElementReferenceException)
                {
                    return null;
                }
            });
        }

        public bool IsElementPresent(string xpath)
        {
            try
            {
                _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(Configuration.ElementDelayMilliSeconds);
                _driver.FindElement(By.XPath(xpath));
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
            finally
            {
                _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0); // Geri alma
            }
        }

        public void NavigateTo(string url)
        {
            _driver.Navigate().GoToUrl(url);
        }

        public void RefreshPage()
        {
            _driver.Navigate().Refresh();
        }
    }
}
