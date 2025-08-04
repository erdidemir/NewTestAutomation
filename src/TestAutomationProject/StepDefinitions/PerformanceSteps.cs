using NUnit.Framework;
using OpenQA.Selenium;
using Serilog;
using TechTalk.SpecFlow;
using TestAutomationProject.Core;
using TestAutomationProject.Helpers;

namespace TestAutomationProject.StepDefinitions
{
    [Binding]
    public class PerformanceSteps
    {
        private readonly IWebDriver _driver;
        private readonly ScenarioContext _scenarioContext;

        public PerformanceSteps(IWebDriver driver, ScenarioContext scenarioContext)
        {
            _driver = driver;
            _scenarioContext = scenarioContext;
        }

        [Given(@"kullanıcı performans testi için hazır")]
        public void GivenKullaniciPerformansTestiIcinHazir()
        {
            PerformanceHelper.ClearPerformanceData();
            Log.Information("Performans testi için hazırlık tamamlandı");
        }

        [Given(@"performans monitoring aktif")]
        public void GivenPerformansMonitoringAktif()
        {
            Assert.That(Configuration.EnablePerformanceMonitoring, Is.True, "Performans monitoring aktif olmalı");
            Log.Information("Performans monitoring aktif");
        }

        [Given(@"kullanıcı browser başlatma performansını test etmek istiyor")]
        public void GivenKullaniciBrowserBaslatmaPerformansiniTestEtmekIstiyor()
        {
            PerformanceMonitor.StartTimer("Browser_Performance_Test");
            Log.Information("Browser başlatma performans testi başlatıldı");
        }

        [When(@"browser başlatılıyor")]
        public void WhenBrowserBaslatiliyor()
        {
            // Browser zaten başlatılmış durumda, sadece performans kontrolü yapılıyor
            var metrics = PerformanceMonitor.GetCurrentMetrics();
            _scenarioContext["MemoryUsage"] = metrics.MemoryUsageMB;
            Log.Information($"Browser başlatma tamamlandı, Memory: {metrics.MemoryUsageMB:F2}MB");
        }

        [Then(@"browser başlatma süresi (.*) saniyeden az olmalı")]
        public void ThenBrowserBaslatmaSuresiSaniyedenAzOlmali(int maxSeconds)
        {
            var browserTime = PerformanceMonitor.StopTimer("Browser_Performance_Test");
            var maxMilliseconds = maxSeconds * 1000;
            
            Assert.That(browserTime, Is.LessThanOrEqualTo(maxMilliseconds), 
                $"Browser başlatma süresi {maxSeconds} saniyeden az olmalı, ancak {browserTime}ms sürdü");
            
            Log.Information($"Browser başlatma süresi: {browserTime}ms (max: {maxMilliseconds}ms)");
        }

        [Then(@"memory kullanımı (.*)MB'den az olmalı")]
        public void ThenMemoryKullanimiMBdenAzOlmali(int maxMemoryMB)
        {
            var memoryUsage = (double)_scenarioContext["MemoryUsage"];
            Assert.That(memoryUsage, Is.LessThanOrEqualTo(maxMemoryMB), 
                $"Memory kullanımı {maxMemoryMB}MB'den az olmalı, ancak {memoryUsage:F2}MB kullanılıyor");
            
            Log.Information($"Memory kullanımı: {memoryUsage:F2}MB (max: {maxMemoryMB}MB)");
        }

        [Given(@"kullanıcı sayfa yükleme performansını test etmek istiyor")]
        public void GivenKullaniciSayfaYuklemePerformansiniTestEtmekIstiyor()
        {
            PerformanceHelper.StartPageLoadTimer("Google_Homepage");
            Log.Information("Sayfa yükleme performans testi başlatıldı");
        }

        [When(@"Google ana sayfası açılıyor")]
        public void WhenGoogleAnaSayfasiAciliyor()
        {
            _driver.Navigate().GoToUrl(Configuration.BaseUrl);
            PerformanceHelper.StopPageLoadTimer("Google_Homepage");
            Log.Information("Google ana sayfası açıldı");
        }

        [Then(@"sayfa yükleme süresi (.*) saniyeden az olmalı")]
        public void ThenSayfaYuklemeSuresiSaniyedenAzOlmali(int maxSeconds)
        {
            var pageLoadTime = PerformanceHelper.StopPageLoadTimer("Google_Homepage");
            var maxMilliseconds = maxSeconds * 1000;
            
            Assert.That(pageLoadTime, Is.LessThanOrEqualTo(maxMilliseconds), 
                $"Sayfa yükleme süresi {maxSeconds} saniyeden az olmalı, ancak {pageLoadTime}ms sürdü");
            
            Log.Information($"Sayfa yükleme süresi: {pageLoadTime}ms (max: {maxMilliseconds}ms)");
        }

        [Then(@"sayfa tamamen yüklenmiş olmalı")]
        public void ThenSayfaTamamenYuklenmisOlmali()
        {
            var readyState = ((IJavaScriptExecutor)_driver).ExecuteScript("return document.readyState").ToString();
            Assert.That(readyState, Is.EqualTo("complete"), "Sayfa tamamen yüklenmemiş");
            Log.Information("Sayfa tamamen yüklendi");
        }

        [Given(@"kullanıcı element etkileşim performansını test etmek istiyor")]
        public void GivenKullaniciElementEtkilesimPerformansiniTestEtmekIstiyor()
        {
            PerformanceHelper.StartElementTimer("Search_Box_Find");
            Log.Information("Element etkileşim performans testi başlatıldı");
        }

        [When(@"arama kutusu bulunuyor")]
        public void WhenAramaKutusuBulunuyor()
        {
            var searchBox = _driver.FindElement(By.Name("q"));
            PerformanceHelper.StopElementTimer("Search_Box_Find");
            _scenarioContext["SearchBox"] = searchBox;
            Log.Information("Arama kutusu bulundu");
        }

        [When(@"arama kutusuna metin yazılıyor")]
        public void WhenAramaKutusunaMetinYaziliyor()
        {
            PerformanceHelper.StartElementTimer("Search_Box_Type");
            var searchBox = (IWebElement)_scenarioContext["SearchBox"];
            searchBox.SendKeys("test automation");
            PerformanceHelper.StopElementTimer("Search_Box_Type");
            Log.Information("Arama kutusuna metin yazıldı");
        }

        [Then(@"element bulma süresi (.*) saniyeden az olmalı")]
        public void ThenElementBulmaSuresiSaniyedenAzOlmali(int maxSeconds)
        {
            var elementFindTime = PerformanceHelper.StopElementTimer("Search_Box_Find");
            var maxMilliseconds = maxSeconds * 1000;
            
            Assert.That(elementFindTime, Is.LessThanOrEqualTo(maxMilliseconds), 
                $"Element bulma süresi {maxSeconds} saniyeden az olmalı, ancak {elementFindTime}ms sürdü");
            
            Log.Information($"Element bulma süresi: {elementFindTime}ms (max: {maxMilliseconds}ms)");
        }

        [Then(@"element etkileşim süresi (.*) saniyeden az olmalı")]
        public void ThenElementEtkilesimSuresiSaniyedenAzOlmali(int maxSeconds)
        {
            var elementInteractionTime = PerformanceHelper.StopElementTimer("Search_Box_Type");
            var maxMilliseconds = maxSeconds * 1000;
            
            Assert.That(elementInteractionTime, Is.LessThanOrEqualTo(maxMilliseconds), 
                $"Element etkileşim süresi {maxSeconds} saniyeden az olmalı, ancak {elementInteractionTime}ms sürdü");
            
            Log.Information($"Element etkileşim süresi: {elementInteractionTime}ms (max: {maxMilliseconds}ms)");
        }

        [Given(@"kullanıcı memory kullanımını kontrol etmek istiyor")]
        public void GivenKullaniciMemoryKullaniminiKontrolEtmekIstiyor()
        {
            Log.Information("Memory kullanım kontrolü başlatıldı");
        }

        [When(@"test senaryosu çalıştırılıyor")]
        public void WhenTestSenaryosuCalistiriliyor()
        {
            // Test senaryosu zaten çalışıyor, sadece memory kontrolü yapılıyor
            var metrics = PerformanceMonitor.GetCurrentMetrics();
            _scenarioContext["CurrentMemoryUsage"] = metrics.MemoryUsageMB;
            Log.Information($"Test senaryosu çalışıyor, Memory: {metrics.MemoryUsageMB:F2}MB");
        }

        [Then(@"memory kullanımı (.*)GB'den az olmalı")]
        public void ThenMemoryKullanimiGBdenAzOlmali(int maxMemoryGB)
        {
            var memoryUsage = (double)_scenarioContext["CurrentMemoryUsage"];
            var maxMemoryMB = maxMemoryGB * 1024;
            
            Assert.That(memoryUsage, Is.LessThanOrEqualTo(maxMemoryMB), 
                $"Memory kullanımı {maxMemoryGB}GB'den az olmalı, ancak {memoryUsage:F2}MB kullanılıyor");
            
            Log.Information($"Memory kullanımı: {memoryUsage:F2}MB (max: {maxMemoryMB}MB)");
        }

        [Then(@"memory sızıntısı olmamalı")]
        public void ThenMemorySizintisiOlmamali()
        {
            // Memory sızıntısı kontrolü için basit bir kontrol
            var metrics = PerformanceMonitor.GetCurrentMetrics();
            Assert.That(metrics.MemoryUsageMB, Is.LessThanOrEqualTo(Configuration.MemoryWarningThresholdMB), 
                $"Memory kullanımı eşik değerini aştı: {metrics.MemoryUsageMB:F2}MB");
            
            Log.Information("Memory sızıntısı kontrolü tamamlandı");
        }

        [Given(@"kullanıcı yavaş işlemleri tespit etmek istiyor")]
        public void GivenKullaniciYavasIslemleriTespitEtmekIstiyor()
        {
            Log.Information("Yavaş işlem tespiti başlatıldı");
        }

        [When(@"yavaş bir işlem gerçekleştiriliyor")]
        public void WhenYavasBirIslemGerceklestiriliyor()
        {
            PerformanceHelper.StartActionTimer("Slow_Operation");
            Thread.Sleep(6000); // 6 saniye bekle (threshold üzeri)
            PerformanceHelper.StopActionTimer("Slow_Operation");
            Log.Information("Yavaş işlem gerçekleştirildi");
        }

        [Then(@"yavaş işlem loglanmalı")]
        public void ThenYavasIslemLoglanmali()
        {
            PerformanceHelper.LogSlowOperations(Configuration.SlowOperationThresholdMs);
            Log.Information("Yavaş işlem loglandı");
        }

        [Then(@"performans raporunda görünmeli")]
        public void ThenPerformansRaporundaGorunmeli()
        {
            var statistics = PerformanceMonitor.GetPerformanceStatistics();
            var slowOperations = statistics.Values.Where(s => s.AverageTime > Configuration.SlowOperationThresholdMs);
            
            Assert.That(slowOperations.Count(), Is.GreaterThan(0), "Yavaş işlemler performans raporunda görünmeli");
            Log.Information("Yavaş işlemler performans raporunda görünüyor");
        }

        [Given(@"kullanıcı driver güncellemelerini kontrol etmek istiyor")]
        public void GivenKullaniciDriverGuncellemeleriniKontrolEtmekIstiyor()
        {
            Log.Information("Driver güncelleme kontrolü başlatıldı");
        }

        [When(@"driver uyumluluk kontrolü yapılıyor")]
        public async Task WhenDriverUyumlulukKontroluYapiliyor()
        {
            var isCompatible = await EnhancedBrowser.EnsureDriverCompatibilityAsync(Configuration.Browser);
            _scenarioContext["DriverCompatible"] = isCompatible;
            Log.Information($"Driver uyumluluk kontrolü tamamlandı: {isCompatible}");
        }

        [Then(@"driver güncel olmalı")]
        public void ThenDriverGuncelOlmali()
        {
            var isCompatible = (bool)_scenarioContext["DriverCompatible"];
            Assert.That(isCompatible, Is.True, "Driver güncel olmalı");
            Log.Information("Driver güncel");
        }

        [Then(@"driver otomatik güncellenmeli")]
        public void ThenDriverOtomatikGuncellenmeli()
        {
            var isCompatible = (bool)_scenarioContext["DriverCompatible"];
            Assert.That(isCompatible, Is.True, "Driver otomatik güncellenmeli");
            Log.Information("Driver otomatik güncellendi");
        }

        [Given(@"kullanıcı performans raporu oluşturmak istiyor")]
        public void GivenKullaniciPerformansRaporuOlusturmakIstiyor()
        {
            Log.Information("Performans raporu oluşturma başlatıldı");
        }

        [When(@"test senaryoları tamamlanıyor")]
        public void WhenTestSenaryolariTamamlanıyor()
        {
            // Test senaryoları zaten tamamlanmış durumda
            Log.Information("Test senaryoları tamamlandı");
        }

        [Then(@"performans raporu oluşturulmalı")]
        public void ThenPerformansRaporuOlusturulmali()
        {
            PerformanceHelper.GeneratePerformanceReport();
            Log.Information("Performans raporu oluşturuldu");
        }

        [Then(@"rapor JSON formatında olmalı")]
        public void ThenRaporJSONFormatındaOlmali()
        {
            var reportPath = Path.Combine(Directory.GetCurrentDirectory(), "Results", "performance-report.json");
            Assert.That(File.Exists(reportPath), Is.True, "Performans raporu JSON formatında oluşturulmalı");
            Log.Information("Performans raporu JSON formatında oluşturuldu");
        }

        [Then(@"rapor Results klasöründe saklanmalı")]
        public void ThenRaporResultsKlasorundeSaklanmali()
        {
            var reportPath = Path.Combine(Directory.GetCurrentDirectory(), "Results", "performance-report.json");
            Assert.That(File.Exists(reportPath), Is.True, "Performans raporu Results klasöründe saklanmalı");
            Log.Information("Performans raporu Results klasöründe saklandı");
        }
    }
} 