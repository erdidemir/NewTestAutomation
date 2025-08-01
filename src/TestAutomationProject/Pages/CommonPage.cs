using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using TestAutomationProject.Core;
using SeleniumExtras.WaitHelpers;
using System.Threading;

namespace TestAutomationProject.Pages
{
    public class CommonPage : Page
    {
        protected readonly int _defaultWaitMilliseconds;

        public CommonPage(IWebDriver driver) : base(driver)
        {
            _defaultWaitMilliseconds = Configuration.ElementDelayMilliseconds;
        }

        /// <summary>
        /// Sayfanın tam olarak yüklendiğinden emin olmak için bekleme işlemi.
        /// </summary>
        public void WaitForPageLoad()
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromMilliseconds(_defaultWaitMilliseconds));
            wait.Until(d => ((IJavaScriptExecutor)d)
                .ExecuteScript("return document.readyState").ToString() == "complete");
        }

        /// <summary>
        /// Belirtilen elementin görünür olmasını bekler.
        /// </summary>
        public IWebElement WaitUntilElementVisible(By by)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromMilliseconds(_defaultWaitMilliseconds));
            return wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(by));
        }

        /// <summary>
        /// Belirtilen elementin tıklanabilir olmasını bekler.
        /// </summary>
        public IWebElement WaitUntilElementClickable(By by)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromMilliseconds(_defaultWaitMilliseconds));
            return wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(by));
        }

        /// <summary>
        /// Elementin tıklanabilir hale gelmesini bekleyip tıklar.
        /// </summary>
        public void WaitAndClick(By by)
        {
            WaitUntilElementClickable(by).Click();
        }

        /// <summary>
        /// Tıklanabilir hale gelen elemente metin gönderir.
        /// </summary>
        public void WaitAndSendKeys(By by, string text)
        {
            var element = WaitUntilElementClickable(by);
            element.Clear();
            element.SendKeys(text);
        }

        /// <summary>
        /// Elementin görünür olmasını bekleyip metnini alır.
        /// </summary>
        public string WaitAndGetText(By by)
        {
            return WaitUntilElementVisible(by).Text;
        }

        /// <summary>
        /// Elementin görünür olmasını bekleyip attribute değerini alır.
        /// </summary>
        public string WaitAndGetAttribute(By by, string attributeName)
        {
            return WaitUntilElementVisible(by).GetAttribute(attributeName);
        }

        /// <summary>
        /// Elementin seçili olup olmadığını kontrol eder.
        /// </summary>
        public bool WaitAndIsSelected(By by)
        {
            return WaitUntilElementVisible(by).Selected;
        }

        /// <summary>
        /// Elementin görünür olup olmadığını kontrol eder.
        /// </summary>
        public bool WaitAndIsDisplayed(By by)
        {
            return WaitUntilElementVisible(by).Displayed;
        }

        /// <summary>
        /// Belirtilen süre kadar bekler.
        /// </summary>
        public void Wait(int milliseconds)
        {
            Thread.Sleep(milliseconds);
        }

        /// <summary>
        /// JavaScript ile elemente tıklar.
        /// </summary>
        public void ClickWithJavaScript(By by)
        {
            var element = WaitUntilElementVisible(by);
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", element);
        }

        /// <summary>
        /// JavaScript ile elemente metin gönderir.
        /// </summary>
        public void SendKeysWithJavaScript(By by, string text)
        {
            var element = WaitUntilElementVisible(by);
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].value = arguments[1];", element, text);
        }

        /// <summary>
        /// Sayfayı aşağı kaydırır.
        /// </summary>
        public void ScrollDown()
        {
            ((IJavaScriptExecutor)_driver).ExecuteScript("window.scrollBy(0, 500);");
        }

        /// <summary>
        /// Sayfayı yukarı kaydırır.
        /// </summary>
        public void ScrollUp()
        {
            ((IJavaScriptExecutor)_driver).ExecuteScript("window.scrollBy(0, -500);");
        }

        /// <summary>
        /// Belirtilen elemente kadar kaydırır.
        /// </summary>
        public void ScrollToElement(By by)
        {
            var element = WaitUntilElementVisible(by);
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);
        }

        /// <summary>
        /// Sayfanın başlığını alır.
        /// </summary>
        public string GetPageTitle()
        {
            return _driver.Title;
        }

        /// <summary>
        /// Sayfanın URL'sini alır.
        /// </summary>
        public string GetCurrentUrl()
        {
            return _driver.Url;
        }

        /// <summary>
        /// Tarayıcı geçmişinde geri gider.
        /// </summary>
        public void GoBack()
        {
            _driver.Navigate().Back();
        }

        /// <summary>
        /// Tarayıcı geçmişinde ileri gider.
        /// </summary>
        public void GoForward()
        {
            _driver.Navigate().Forward();
        }

        /// <summary>
        /// Alert'i kabul eder.
        /// </summary>
        public void AcceptAlert()
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromMilliseconds(_defaultWaitMilliseconds));
            wait.Until(ExpectedConditions.AlertIsPresent());
            _driver.SwitchTo().Alert().Accept();
        }

        /// <summary>
        /// Alert'i reddeder.
        /// </summary>
        public void DismissAlert()
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromMilliseconds(_defaultWaitMilliseconds));
            wait.Until(ExpectedConditions.AlertIsPresent());
            _driver.SwitchTo().Alert().Dismiss();
        }

        /// <summary>
        /// Alert'teki metni alır.
        /// </summary>
        public string GetAlertText()
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromMilliseconds(_defaultWaitMilliseconds));
            wait.Until(ExpectedConditions.AlertIsPresent());
            return _driver.SwitchTo().Alert().Text;
        }

        /// <summary>
        /// Alert'e metin gönderir.
        /// </summary>
        public void SendKeysToAlert(string text)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromMilliseconds(_defaultWaitMilliseconds));
            wait.Until(ExpectedConditions.AlertIsPresent());
            _driver.SwitchTo().Alert().SendKeys(text);
        }
    }
}
