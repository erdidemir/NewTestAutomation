using OpenQA.Selenium;
using TestAutomationProject.Core;

namespace TestAutomationProject.Pages
{
    public class GoogleSearchPage : CommonPage
    {
        // Google Search sayfası elementleri - basit seçiciler
        private readonly By _searchInput = By.Name("q");
        private readonly By _searchButton = By.Name("btnK");
        
        // Arama sonuçları için alternatif seçiciler
        private readonly By _searchResults = By.Id("search");
        private readonly By _searchResultsAlt1 = By.Id("rso");
        private readonly By _searchResultsAlt2 = By.XPath("//div[@id='search']");
        private readonly By _searchResultsAlt3 = By.XPath("//div[contains(@class,'search')]");
        private readonly By _searchResultsAlt4 = By.XPath("//div[@data-ved]");
        private readonly By _searchResultsAlt5 = By.XPath("//div[@class='g']");
        
        // Google logo için alternatif seçiciler
        private readonly By _googleLogo = By.XPath("//img[@alt='Google']");
        private readonly By _googleLogoAlt1 = By.XPath("//img[contains(@src,'google')]");
        private readonly By _googleLogoAlt2 = By.XPath("//div[@class='logo']//img");
        private readonly By _googleLogoAlt3 = By.XPath("//div[@id='logo']//img");
        private readonly By _googleLogoAlt4 = By.CssSelector("img[alt='Google']");
        private readonly By _googleLogoAlt5 = By.XPath("//img[contains(@src,'logo')]");
        
        private readonly By _luckyButton = By.Name("btnI");

        public GoogleSearchPage(IWebDriver driver) : base(driver)
        {
        }

        /// <summary>
        /// Google ana sayfasına gider
        /// </summary>
        public void NavigateToGoogle()
        {
            _driver.Navigate().GoToUrl("https://www.google.com");
            WaitForPageLoad();
        }

        /// <summary>
        /// Arama kutusuna metin girer
        /// </summary>
        public void EnterSearchText(string searchText)
        {
            WaitAndSendKeys(_searchInput, searchText);
        }

        /// <summary>
        /// Arama butonuna tıklar
        /// </summary>
        public void ClickSearchButton()
        {
            WaitAndClick(_searchButton);
        }

        /// <summary>
        /// Google'da arama yapar
        /// </summary>
        public void SearchInGoogle(string searchText)
        {
            NavigateToGoogle();
            EnterSearchText(searchText);
            ClickSearchButton();
        }

        /// <summary>
        /// Arama sonuçlarının görünür olup olmadığını kontrol eder (alternatif seçicilerle)
        /// </summary>
        public bool AreSearchResultsVisible()
        {
            try
            {
                // Ana seçici ile dene
                if (WaitAndIsDisplayed(_searchResults))
                    return true;
            }
            catch
            {
                // Ana seçici başarısız olursa alternatifleri dene
            }

            try
            {
                // İlk alternatif seçici ile dene
                if (WaitAndIsDisplayed(_searchResultsAlt1))
                    return true;
            }
            catch
            {
                // İlk alternatif başarısız olursa diğerlerini dene
            }

            try
            {
                // İkinci alternatif seçici ile dene
                if (WaitAndIsDisplayed(_searchResultsAlt2))
                    return true;
            }
            catch
            {
                // İkinci alternatif başarısız olursa diğerlerini dene
            }

            try
            {
                // Üçüncü alternatif seçici ile dene
                if (WaitAndIsDisplayed(_searchResultsAlt3))
                    return true;
            }
            catch
            {
                // Üçüncü alternatif başarısız olursa diğerlerini dene
            }

            try
            {
                // Dördüncü alternatif seçici ile dene
                if (WaitAndIsDisplayed(_searchResultsAlt4))
                    return true;
            }
            catch
            {
                // Dördüncü alternatif başarısız olursa sonuncuyu dene
            }

            try
            {
                // Beşinci alternatif seçici ile dene
                if (WaitAndIsDisplayed(_searchResultsAlt5))
                    return true;
            }
            catch
            {
                // Tüm seçiciler başarısız olursa false döndür
            }

            return false;
        }

        /// <summary>
        /// Google logosunun görünür olup olmadığını kontrol eder
        /// </summary>
        public bool IsGoogleLogoVisible()
        {
            try
            {
                // Ana seçici ile dene
                if (WaitAndIsDisplayed(_googleLogo))
                    return true;
            }
            catch
            {
                // Ana seçici başarısız olursa alternatifleri dene
            }

            try
            {
                // CSS seçici ile dene
                if (WaitAndIsDisplayed(_googleLogoAlt4))
                    return true;
            }
            catch
            {
                // CSS seçici başarısız olursa diğerlerini dene
            }

            try
            {
                // Alternatif XPath seçicileri dene
                if (WaitAndIsDisplayed(_googleLogoAlt1))
                    return true;
            }
            catch
            {
                // İlk alternatif başarısız olursa diğerlerini dene
            }

            try
            {
                if (WaitAndIsDisplayed(_googleLogoAlt2))
                    return true;
            }
            catch
            {
                // İkinci alternatif başarısız olursa diğerlerini dene
            }

            try
            {
                if (WaitAndIsDisplayed(_googleLogoAlt3))
                    return true;
            }
            catch
            {
                // Üçüncü alternatif başarısız olursa sonuncuyu dene
            }

            try
            {
                if (WaitAndIsDisplayed(_googleLogoAlt5))
                    return true;
            }
            catch
            {
                // Tüm seçiciler başarısız olursa false döndür
            }

            return false;
        }

        /// <summary>
        /// Arama kutusunun görünür olup olmadığını kontrol eder
        /// </summary>
        public bool IsSearchInputVisible()
        {
            return WaitAndIsDisplayed(_searchInput);
        }

        /// <summary>
        /// Arama kutusundaki metni alır
        /// </summary>
        public string GetSearchInputText()
        {
            return WaitAndGetAttribute(_searchInput, "value");
        }

        /// <summary>
        /// Sayfa başlığını alır
        /// </summary>
        public string GetPageTitle()
        {
            return _driver.Title;
        }

        /// <summary>
        /// Şanslı ol butonuna tıklar
        /// </summary>
        public void ClickLuckyButton()
        {
            WaitAndClick(_luckyButton);
        }
    }
} 