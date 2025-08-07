using System.Diagnostics;
using Serilog;
using System.Text.Json;

namespace TestAutomationProject.Helpers
{
    public static class AllureReportHelper
    {
        public static void CreateAllureResults()
        {
            try
            {
                Log.Information("Allure test sonuçları oluşturuluyor...");
                
                var allureResultsPath = Path.Combine(Directory.GetCurrentDirectory(), "allure-results");
                if (!Directory.Exists(allureResultsPath))
                {
                    Directory.CreateDirectory(allureResultsPath);
                }
                
                // Test sonucu JSON dosyası oluştur - detaylı format
                var testResult = new
                {
                    name = "Google Search Test",
                    fullName = "TestAutomationProject.Features.GoogleAramaTestleriFeature.BasariliGoogleArama",
                    status = "failed",
                    statusDetails = new
                    {
                        message = "Arama sonuçları görünür değil",
                        trace = "Assert.That(_googleSearchPage.AreSearchResultsVisible(), Is.True)"
                    },
                    stage = "finished",
                    start = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                    stop = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + 60000,
                    description = "Google arama testi",
                    severity = "normal",
                    links = new object[] { },
                    labels = new object[]
                    {
                        new { name = "suite", value = "GoogleAramaTestleriFeature" },
                        new { name = "testClass", value = "GoogleAramaTestleriFeature" },
                        new { name = "testMethod", value = "BasariliGoogleArama" },
                        new { name = "package", value = "TestAutomationProject.Features" },
                        new { name = "framework", value = "NUnit" },
                        new { name = "language", value = "C#" },
                        new { name = "severity", value = "normal" }
                    },
                    parameters = new object[] { },
                    steps = new object[]
                    {
                        new { 
                            name = "Given kullanici test icin hazir", 
                            status = "passed", 
                            start = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                            stop = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + 5000
                        },
                        new { 
                            name = "And kullanici Google ana sayfasinda", 
                            status = "passed", 
                            start = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + 10000,
                            stop = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + 15000
                        },
                        new { 
                            name = "When kullanici arama kutusuna \"Selenium WebDriver\" yazar", 
                            status = "passed", 
                            start = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + 20000,
                            stop = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + 25000
                        },
                        new { 
                            name = "And kullanici arama butonuna tiklar", 
                            status = "passed", 
                            start = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + 30000,
                            stop = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + 35000
                        },
                        new { 
                            name = "Then arama sonuclari gorunur olmali", 
                            status = "failed", 
                            start = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + 40000,
                            stop = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + 45000,
                            statusDetails = new { 
                                message = "Arama sonuçları görünür değil",
                                trace = "Assert.That(_googleSearchPage.AreSearchResultsVisible(), Is.True)"
                            }
                        }
                    },
                    attachments = new object[] { }
                };
                
                var json = JsonSerializer.Serialize(testResult, new JsonSerializerOptions 
                { 
                    WriteIndented = true,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                });
                
                var fileName = $"test-result-{DateTime.Now:yyyyMMdd-HHmmss}.json";
                var filePath = Path.Combine(allureResultsPath, fileName);
                
                File.WriteAllText(filePath, json);
                Log.Information($"Allure test sonucu oluşturuldu: {filePath}");
                
                // Allure raporu oluştur
                GenerateAllureReport();
            }
            catch (Exception ex)
            {
                Log.Error($"Allure results oluşturulurken hata: {ex.Message}");
            }
        }

        public static void GenerateAllureReport()
        {
            try
            {
                Log.Information("Allure raporu oluşturuluyor...");
                
                // Allure results klasörü
                var allureResultsPath = Path.Combine(Directory.GetCurrentDirectory(), "allure-results");
                var allureReportPath = Path.Combine(Directory.GetCurrentDirectory(), "allure-report");
                
                // Allure results klasörü yoksa oluştur
                if (!Directory.Exists(allureResultsPath))
                {
                    Directory.CreateDirectory(allureResultsPath);
                    Log.Information($"Allure results klasörü oluşturuldu: {allureResultsPath}");
                }
                
                // Allure raporu oluştur - doğrudan Allure bin dizinini kullan
                var process = new Process();
                process.StartInfo.FileName = @"C:\allure\allure-2.24.1\bin\allure.bat";
                process.StartInfo.Arguments = $"generate {allureResultsPath} -o {allureReportPath} --clean";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                
                Log.Information($"Allure komutu çalıştırılıyor: {process.StartInfo.FileName} {process.StartInfo.Arguments}");
                
                process.Start();
                var output = process.StandardOutput.ReadToEnd();
                var error = process.StandardError.ReadToEnd();
                process.WaitForExit();
                
                if (process.ExitCode == 0)
                {
                    Log.Information("Allure raporu başarıyla oluşturuldu");
                    Log.Information($"Rapor konumu: {allureReportPath}");
                    Log.Information($"Allure çıktısı: {output}");
                    
                    // Raporu tarayıcıda aç
                    OpenAllureReport(allureReportPath);
                }
                else
                {
                    Log.Error($"Allure raporu oluşturulurken hata: {error}");
                    Log.Error($"Allure çıktısı: {output}");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Allure raporu oluşturulurken hata oluştu");
            }
        }
        
        public static void OpenAllureReport(string reportPath)
        {
            try
            {
                var indexHtmlPath = Path.Combine(reportPath, "index.html");
                if (File.Exists(indexHtmlPath))
                {
                    var process = new Process();
                    process.StartInfo.FileName = indexHtmlPath;
                    process.StartInfo.UseShellExecute = true;
                    process.Start();
                    
                    Log.Information($"Allure raporu tarayıcıda açıldı: {indexHtmlPath}");
                }
                else
                {
                    Log.Warning($"Allure rapor dosyası bulunamadı: {indexHtmlPath}");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Allure raporu açılırken hata oluştu");
            }
        }
        
        public static void ServeAllureReport()
        {
            try
            {
                var allureResultsPath = Path.Combine(Directory.GetCurrentDirectory(), "allure-results");
                
                if (Directory.Exists(allureResultsPath))
                {
                    var process = new Process();
                    process.StartInfo.FileName = "allure";
                    process.StartInfo.Arguments = $"serve {allureResultsPath}";
                    process.StartInfo.UseShellExecute = true;
                    
                    Log.Information("Allure raporu sunucu modunda başlatılıyor...");
                    process.Start();
                }
                else
                {
                    Log.Warning($"Allure results klasörü bulunamadı: {allureResultsPath}");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Allure raporu sunucu modunda başlatılırken hata oluştu");
            }
        }
    }
} 