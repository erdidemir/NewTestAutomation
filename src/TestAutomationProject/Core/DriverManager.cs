using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Serilog;

namespace TestAutomationProject.Core
{
    public class DriverManager
    {
        private static readonly HttpClient _httpClient = new();
        private static readonly string _driversPath = Path.Combine(Directory.GetCurrentDirectory(), "Drivers");
        
        public static async Task<bool> UpdateChromeDriverAsync()
        {
            try
            {
                Log.Information("Chrome Driver güncelleme başlatılıyor...");
                
                // Chrome versiyonunu al
                var chromeVersion = await GetChromeVersionAsync();
                if (string.IsNullOrEmpty(chromeVersion))
                {
                    Log.Warning("Chrome versiyonu alınamadı");
                    return false;
                }
                
                Log.Information($"Chrome versiyonu: {chromeVersion}");
                
                // ChromeDriver versiyonunu al
                var driverVersion = await GetChromeDriverVersionAsync(chromeVersion);
                if (string.IsNullOrEmpty(driverVersion))
                {
                    Log.Warning("ChromeDriver versiyonu alınamadı");
                    return false;
                }
                
                Log.Information($"ChromeDriver versiyonu: {driverVersion}");
                
                // Driver'ı indir ve kur
                var success = await DownloadAndInstallDriverAsync("chromedriver", driverVersion);
                
                if (success)
                {
                    Log.Information("ChromeDriver başarıyla güncellendi");
                }
                else
                {
                    Log.Error("ChromeDriver güncellenirken hata oluştu");
                }
                
                return success;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "ChromeDriver güncelleme sırasında hata oluştu");
                return false;
            }
        }
        
        public static async Task<bool> UpdateEdgeDriverAsync()
        {
            try
            {
                Log.Information("Edge Driver güncelleme başlatılıyor...");
                
                // Edge versiyonunu al
                var edgeVersion = await GetEdgeVersionAsync();
                if (string.IsNullOrEmpty(edgeVersion))
                {
                    Log.Warning("Edge versiyonu alınamadı");
                    return false;
                }
                
                Log.Information($"Edge versiyonu: {edgeVersion}");
                
                // EdgeDriver versiyonunu al
                var driverVersion = await GetEdgeDriverVersionAsync(edgeVersion);
                if (string.IsNullOrEmpty(driverVersion))
                {
                    Log.Warning("EdgeDriver versiyonu alınamadı");
                    return false;
                }
                
                Log.Information($"EdgeDriver versiyonu: {driverVersion}");
                
                // Driver'ı indir ve kur
                var success = await DownloadAndInstallDriverAsync("edgedriver", driverVersion);
                
                if (success)
                {
                    Log.Information("EdgeDriver başarıyla güncellendi");
                }
                else
                {
                    Log.Error("EdgeDriver güncellenirken hata oluştu");
                }
                
                return success;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "EdgeDriver güncelleme sırasında hata oluştu");
                return false;
            }
        }
        
        private static async Task<string> GetChromeVersionAsync()
        {
            try
            {
                var response = await _httpClient.GetStringAsync("https://omahaproxy.appspot.com/all.json");
                var versions = JsonSerializer.Deserialize<JsonElement[]>(response);
                
                foreach (var version in versions)
                {
                    if (version.TryGetProperty("os", out var os) && 
                        os.GetString() == "win" &&
                        version.TryGetProperty("channel", out var channel) && 
                        channel.GetString() == "stable")
                    {
                        if (version.TryGetProperty("versions", out var versionsArray))
                        {
                            foreach (var v in versionsArray.EnumerateArray())
                            {
                                if (v.TryGetProperty("version", out var versionElement))
                                {
                                    return versionElement.GetString();
                                }
                            }
                        }
                    }
                }
                
                return null;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Chrome versiyonu alınırken hata oluştu");
                return null;
            }
        }
        
        private static async Task<string> GetEdgeVersionAsync()
        {
            try
            {
                var response = await _httpClient.GetStringAsync("https://edgeupdater.microsoft.com/api/products?view=enterprise");
                var products = JsonSerializer.Deserialize<JsonElement[]>(response);
                
                foreach (var product in products)
                {
                    if (product.TryGetProperty("Product", out var productName) && 
                        productName.GetString() == "Stable")
                    {
                        if (product.TryGetProperty("Releases", out var releases))
                        {
                            foreach (var release in releases.EnumerateArray())
                            {
                                if (release.TryGetProperty("ProductVersion", out var version))
                                {
                                    return version.GetString();
                                }
                            }
                        }
                    }
                }
                
                return null;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Edge versiyonu alınırken hata oluştu");
                return null;
            }
        }
        
        private static async Task<string> GetChromeDriverVersionAsync(string chromeVersion)
        {
            try
            {
                var majorVersion = chromeVersion.Split('.')[0];
                var response = await _httpClient.GetStringAsync($"https://chromedriver.storage.googleapis.com/LATEST_RELEASE_{majorVersion}");
                return response.Trim();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "ChromeDriver versiyonu alınırken hata oluştu");
                return null;
            }
        }
        
        private static async Task<string> GetEdgeDriverVersionAsync(string edgeVersion)
        {
            try
            {
                var majorVersion = edgeVersion.Split('.')[0];
                var response = await _httpClient.GetStringAsync($"https://msedgedriver.azureedge.net/LATEST_STABLE_{majorVersion}");
                return response.Trim();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "EdgeDriver versiyonu alınırken hata oluştu");
                return null;
            }
        }
        
        private static async Task<bool> DownloadAndInstallDriverAsync(string driverType, string version)
        {
            try
            {
                // Drivers klasörünü oluştur
                if (!Directory.Exists(_driversPath))
                {
                    Directory.CreateDirectory(_driversPath);
                }
                
                string downloadUrl;
                string fileName;
                
                if (driverType == "chromedriver")
                {
                    downloadUrl = $"https://chromedriver.storage.googleapis.com/{version}/chromedriver_win32.zip";
                    fileName = "chromedriver.exe";
                }
                else if (driverType == "edgedriver")
                {
                    downloadUrl = $"https://msedgedriver.azureedge.net/{version}/edgedriver_win64.zip";
                    fileName = "msedgedriver.exe";
                }
                else
                {
                    return false;
                }
                
                var zipPath = Path.Combine(_driversPath, $"{driverType}_{version}.zip");
                var exePath = Path.Combine(_driversPath, fileName);
                
                // Driver'ı indir
                Log.Information($"Driver indiriliyor: {downloadUrl}");
                var driverBytes = await _httpClient.GetByteArrayAsync(downloadUrl);
                await File.WriteAllBytesAsync(zipPath, driverBytes);
                
                // Zip'i çıkart
                Log.Information("Driver dosyası çıkartılıyor...");
                System.IO.Compression.ZipFile.ExtractToDirectory(zipPath, _driversPath, true);
                
                // Eski zip dosyasını sil
                if (File.Exists(zipPath))
                {
                    File.Delete(zipPath);
                }
                
                // Çalıştırma izni ver
                if (File.Exists(exePath))
                {
                    var fileInfo = new FileInfo(exePath);
                    fileInfo.Attributes &= ~FileAttributes.ReadOnly;
                }
                
                Log.Information($"Driver başarıyla kuruldu: {exePath}");
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Driver kurulumu sırasında hata oluştu: {driverType}");
                return false;
            }
        }
        
        public static bool CheckDriverCompatibility(string browserType)
        {
            try
            {
                var driverPath = browserType.ToLower() == "chrome" 
                    ? Path.Combine(_driversPath, "chromedriver.exe")
                    : Path.Combine(_driversPath, "msedgedriver.exe");
                
                if (!File.Exists(driverPath))
                {
                    Log.Warning($"{browserType} driver bulunamadı: {driverPath}");
                    return false;
                }
                
                Log.Information($"{browserType} driver mevcut: {driverPath}");
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Driver uyumluluk kontrolü sırasında hata oluştu: {browserType}");
                return false;
            }
        }
    }
} 