# Test Automation Project

Modern web uygulamaları için geliştirilmiş kapsamlı test otomasyonu çözümü. Clean Architecture prensipleri kullanılarak tasarlanmış, çoklu tarayıcı desteği, gelişmiş raporlama ve performans monitoring özellikleri içerir.

## 📖 Kurulum Rehberi

**Yeni başlayanlar için detaylı kurulum rehberi:** [KURULUM_REHBERI.md](./KURULUM_REHBERI.md)

Bu rehber şunları içerir:
- ✅ Visual Studio Community 2022 kurulumu
- ✅ .NET 9 SDK kurulumu
- ✅ Java JDK kurulumu
- ✅ Allure CLI kurulumu
- ✅ Tarayıcı kurulumları (Chrome, Edge)
- ✅ Git kurulumu (opsiyonel)
- ✅ Adım adım kurulum talimatları
- ✅ Sorun giderme ipuçları

## 🚀 Yeni Özellikler

### 📊 Performans Monitoring Sistemi
- **Gerçek Zamanlı Metrikler**: CPU, Memory, Thread kullanımı
- **İşlem Süreleri**: Browser başlatma, sayfa yükleme, element etkileşimi
- **Yavaş İşlem Tespiti**: Konfigüre edilebilir threshold değerleri
- **Performans Raporları**: JSON formatında detaylı raporlar
- **Trend Analizi**: Zaman içinde performans değişimleri

### 🔄 Otomatik Driver Güncelleme
- **Chrome Driver**: Otomatik versiyon kontrolü ve güncelleme
- **Edge Driver**: Otomatik versiyon kontrolü ve güncelleme
- **Uyumluluk Kontrolü**: Browser-Driver versiyon uyumluluğu
- **Güvenli Güncelleme**: Hata durumunda geri alma
- **Konfigürasyon**: Güncelleme sıklığı ve timeout ayarları

### 🎯 Gelişmiş Browser Optimizasyonu
- **Performans Ayarları**: GPU, extensions, plugins devre dışı
- **Memory Optimizasyonu**: Bellek kullanımı optimizasyonu
- **Hızlı Başlatma**: Optimize edilmiş browser konfigürasyonu
- **Stabilite**: Kararlı test çalıştırma

## 🏗️ Mimari

### Clean Architecture
- **Domain Layer** (Core/) - İş mantığı ve konfigürasyon
- **Application Layer** (Pages/) - Sayfa nesneleri ve işlemler  
- **Infrastructure Layer** (StepDefinitions/) - Test implementasyonları

### Teknoloji Stack
- **.NET 9** - Modern .NET platformu
- **Selenium WebDriver 4.10.0** - Web otomasyon
- **SpecFlow 3.9.74** - BDD framework
- **NUnit 4** - Test framework
- **Allure.Commons 3.5.0.73** - Gelişmiş raporlama
- **Serilog 3.1.1** - Loglama
- **ExtentReports 4.1.0** - HTML raporlama

## 🌐 Tarayıcı Desteği

| Tarayıcı | Driver | Versiyon | Durum | Otomatik Güncelleme |
|-----------|--------|----------|-------|-------------------|
| **Microsoft Edge** | EdgeDriver | 3.14393.0.1 | ✅ Aktif | ✅ Destekleniyor |
| **Google Chrome** | ChromeDriver | 114.0.5735.9000 | ✅ Aktif | ✅ Destekleniyor |
| **Mozilla Firefox** | GeckoDriver | 0.33.0 | ✅ Aktif | ⚠️ Manuel |
| **Safari** | SafariDriver | - | ✅ Aktif | ⚠️ Manuel |

## 📊 Performans Özellikleri

### ✅ Mevcut
- **Gerçek Zamanlı Monitoring** - CPU, Memory, Thread takibi
- **İşlem Süresi Ölçümü** - Browser, sayfa, element işlemleri
- **Yavaş İşlem Tespiti** - Konfigüre edilebilir threshold
- **Performans Raporları** - JSON formatında detaylı raporlar
- **Memory Sızıntısı Kontrolü** - Bellek kullanımı izleme
- **Trend Analizi** - Zaman içinde performans değişimleri

### 🔧 Konfigürasyon
```json
{
  "PerformanceSettings": {
    "EnablePerformanceMonitoring": true,
    "GeneratePerformanceReport": true,
    "LogSlowOperations": true,
    "SlowOperationThresholdMs": 5000,
    "MemoryWarningThresholdMB": 1000,
    "CpuWarningThresholdMs": 10000
  },
  "DriverSettings": {
    "AutoUpdateDrivers": true,
    "CheckDriverCompatibility": true,
    "DriverUpdateIntervalDays": 7,
    "DriverDownloadTimeoutSeconds": 300,
    "EnableDriverLogging": true
  }
}
```

## 🚀 Kullanım

### Test Çalıştırma
```bash
# Tüm testler
dotnet test

# Performans testleri
dotnet test --filter "Category=Performance"

# Browser performans testleri
dotnet test --filter "Category=BrowserPerformance"

# Driver güncelleme testleri
dotnet test --filter "Category=DriverUpdate"
```

### Performans Raporu
```bash
# Performans raporu oluştur
dotnet test --filter "Category=Performance"

# Raporu görüntüle
# Results/performance-report.json dosyasını kontrol et
```

### Driver Güncelleme
```bash
# Manuel driver güncelleme
# Chrome için
dotnet test --filter "Category=DriverUpdate" --filter "TestName=ChromeDriverUpdate"

# Edge için
dotnet test --filter "Category=DriverUpdate" --filter "TestName=EdgeDriverUpdate"
```

## 📈 Performans Metrikleri

### İzlenen Metrikler
- **Browser Başlatma Süresi**: Tarayıcı başlatma performansı
- **Sayfa Yükleme Süresi**: Web sayfalarının yüklenme hızı
- **Element Bulma Süresi**: DOM elementlerinin bulunma hızı
- **Element Etkileşim Süresi**: Click, type gibi işlemlerin hızı
- **Memory Kullanımı**: Bellek tüketimi ve sızıntı kontrolü
- **CPU Kullanımı**: İşlemci kullanım oranı
- **Thread Sayısı**: Aktif thread sayısı

### Raporlama
- **JSON Raporları**: Detaylı performans metrikleri
- **Console Logları**: Gerçek zamanlı performans bilgileri
- **Allure Entegrasyonu**: Performans verilerinin Allure'da görüntülenmesi
- **Trend Analizi**: Zaman içinde performans değişimleri

## 🔧 Geliştirme

### Yeni Performans Testi Ekleme
1. **Feature dosyası oluştur** (`Features/PerformanceTest.feature`)
2. **Step definitions yaz** (`StepDefinitions/PerformanceSteps.cs`)
3. **Performans metrikleri ekle** (`PerformanceHelper.cs`)
4. **Test çalıştır**

### Driver Güncelleme Sistemi
1. **DriverManager.cs** - Otomatik güncelleme mantığı
2. **EnhancedBrowser.cs** - Gelişmiş browser yönetimi
3. **Configuration.cs** - Driver ayarları
4. **Hooks.cs** - Test lifecycle entegrasyonu

## 🐛 Sorun Giderme

### Performans Sorunları
- **Yavaş Browser Başlatma** → Driver güncelleme kontrolü yap
- **Yüksek Memory Kullanımı** → Browser optimizasyonları kontrol et
- **Yavaş Element İşlemleri** → Locator stratejilerini gözden geçir
- **Timeout Hataları** → Bekleme sürelerini artır

### Driver Sorunları
- **Driver Uyumsuzluğu** → Otomatik güncelleme çalıştır
- **Download Hatası** → Network bağlantısını kontrol et
- **Kurulum Hatası** → Antivirus/firewall ayarlarını kontrol et

### Debug İpuçları
- `EnablePerformanceMonitoring: true` ile performans izleme
- `LogSlowOperations: true` ile yavaş işlemleri logla
- `GeneratePerformanceReport: true` ile detaylı raporlar
- Console loglarını kontrol et

## 📚 Teknik Detaylar

### Proje Yapısı
```
TestAutomationProject/
├── Core/                    # Domain Layer
│   ├── PerformanceMonitor.cs # Performans monitoring
│   ├── DriverManager.cs     # Driver güncelleme
│   ├── EnhancedBrowser.cs   # Gelişmiş browser
│   └── Configuration.cs     # Konfigürasyon
├── Helpers/                 # Yardımcı sınıflar
│   └── PerformanceHelper.cs # Performans yardımcıları
├── Features/               # BDD Feature dosyaları
│   └── PerformanceTest.feature
├── StepDefinitions/        # Test implementasyonları
│   └── PerformanceSteps.cs
└── Results/                # Test sonuçları
    └── performance-report.json
```

### Performans Monitoring API
```csharp
// Timer başlat
PerformanceMonitor.StartTimer("OperationName");

// Timer durdur
var elapsed = PerformanceMonitor.StopTimer("OperationName");

// Metrikleri al
var metrics = PerformanceMonitor.GetCurrentMetrics();

// İstatistikleri al
var statistics = PerformanceMonitor.GetPerformanceStatistics();

// Rapor oluştur
PerformanceHelper.GeneratePerformanceReport();
```

### Driver Güncelleme API
```csharp
// Chrome driver güncelle
await DriverManager.UpdateChromeDriverAsync();

// Edge driver güncelle
await DriverManager.UpdateEdgeDriverAsync();

// Uyumluluk kontrolü
var isCompatible = DriverManager.CheckDriverCompatibility("chrome");

// Otomatik uyumluluk kontrolü
await EnhancedBrowser.EnsureDriverCompatibilityAsync("edge");
```

---

**Versiyon:** 2.0.0  
**Teknoloji:** .NET 9, Selenium 4, SpecFlow 3.9.74, Allure 3.5.0  
**Durum:** ✅ Production Ready with Performance Monitoring 