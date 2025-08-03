# Test Automation Project - Proje Dokümantasyonu

## 📋 Proje Genel Bakış

Bu proje, modern web uygulamaları için kapsamlı test otomasyonu çözümü sunar. Clean Architecture prensipleri kullanılarak geliştirilmiş, çoklu tarayıcı desteği ve gelişmiş raporlama özellikleri içerir.

## 🏗️ Mimari Yapı

### Clean Architecture Uygulaması
```
TestAutomationProject/
├── Core/                    # Domain Layer
│   ├── Browser.cs          # Tarayıcı yönetimi
│   ├── Configuration.cs    # Konfigürasyon yönetimi
│   └── Hooks/             # Test lifecycle hooks
├── Pages/                  # Application Layer
│   ├── CommonPage.cs      # Temel sayfa işlemleri
│   ├── GoogleSearchPage.cs # Google arama sayfası
│   └── LoginPage.cs       # Giriş sayfası
├── StepDefinitions/        # Infrastructure Layer
│   ├── CommonSteps.cs     # Ortak step definitions
│   ├── GoogleSearchSteps.cs
│   └── GoogleLoginSteps.cs
├── Features/              # BDD Feature dosyaları
├── Models/               # Data models
├── Helpers/              # Yardımcı sınıflar
└── Results/              # Test sonuçları
```

## 🚀 Teknoloji Stack

### Core Framework
- **.NET 9** - Modern .NET platformu
- **NUnit 4** - Test framework
- **SpecFlow 3.9.74** - BDD framework
- **Selenium WebDriver 4.10.0** - Web otomasyon

### Tarayıcı Desteği
- **Microsoft Edge** - Ana tarayıcı (EdgeDriver 3.14393.0.1)
- **Google Chrome** - Alternatif tarayıcı (ChromeDriver 114.0.5735.9000)
- **Mozilla Firefox** - GeckoDriver 0.33.0
- **Safari** - SafariDriver desteği

### Raporlama & Logging
- **Serilog 3.1.1** - Gelişmiş loglama
- **ExtentReports 4.1.0** - HTML raporlama
- **Merkezi Paket Yönetimi** - Directory.Packages.props

### Ek Paketler
- **BoDi 1.5.0** - Dependency injection
- **SeleniumExtras.WaitHelpers 1.0.2** - Explicit wait
- **Microsoft.Extensions.Configuration** - Konfigürasyon yönetimi
- **StackExchange.Redis 2.7.33** - Cache desteği
- **Stateless 5.16.0** - State machine

## 🔧 Konfigürasyon

### appsettings.json
```json
{
  "Configuration": {
    "Browser": "edge",
    "Headless": false,
    "BaseUrl": "https://www.google.com",
    "ElementDelayMilliSeconds": 5000,
    "ReportPathBase": "Result",
    "ResultsFolder": "Results"
  }
}
```

### Tarayıcı Konfigürasyonu
```csharp
// Edge Driver Konfigürasyonu
EdgeOptions edgeOptions = new EdgeOptions();
edgeOptions.AddArguments("--ignore-certificate-errors");
edgeOptions.AddArguments("--window-size=1920,1080");
edgeOptions.AddArguments("inprivate");
if (Configuration.Headless)
    edgeOptions.AddArguments("--headless=new");
edgeOptions.AddUserProfilePreference("download.prompt_for_download", false);
edgeOptions.AddUserProfilePreference("safebrowsing.enabled", true);
```

## 📝 Test Senaryoları

### Google Arama Testleri
```gherkin
@GoogleSearch
Feature: Google Arama Testleri
  Kullanıcı olarak Google'da arama yapabilmek istiyorum
  Böylece istediğim bilgileri bulabilirim

  Background:
    Given kullanıcı test için hazır
    And kullanıcı Google ana sayfasında

  @Positive @Smoke
  Scenario: Başarılı Google Arama
    When kullanıcı arama kutusuna "Selenium WebDriver" yazar
    And kullanıcı arama butonuna tıklar
    Then arama sonuçları görünür olmalı
    And sayfa başlığı "Selenium WebDriver" içermeli
```

### Google Login Testleri
```gherkin
@GoogleLogin
Feature: Google Login Testleri
  Kullanıcı olarak Google hesabıma giriş yapabilmek istiyorum
  Böylece Google servislerini kullanabilirim

  @Positive @Smoke
  Scenario: Başarılı Google Login
    When kullanıcı email adresini girer: "testuser@gmail.com"
    And kullanıcı şifresini girer: "testpassword123"
    Then Google logosu görünür olmalı
```

## 🎯 Özellikler

### ✅ Mevcut Özellikler
- **Clean Architecture** - Katmanlı mimari
- **Multi-Browser Support** - Edge, Chrome, Firefox, Safari
- **BDD Approach** - SpecFlow ile behavior driven development
- **Page Object Model** - Sürdürülebilir test yapısı
- **Centralized Package Management** - Merkezi paket yönetimi
- **Comprehensive Logging** - Serilog ile detaylı loglama
- **HTML Reporting** - ExtentReports ile görsel raporlar
- **Configuration Management** - JSON tabanlı konfigürasyon
- **Dependency Injection** - BoDi ile DI container
- **Explicit Waits** - Güvenilir element bekleme
- **Screenshot Support** - Hata durumunda otomatik screenshot
- **Cross-Platform** - Windows, macOS, Linux desteği

### 🔄 Test Lifecycle
1. **BeforeTestRun** - Test suite başlangıcı
2. **BeforeFeature** - Feature başlangıcı
3. **BeforeScenario** - Senaryo başlangıcı
4. **BeforeStep** - Step başlangıcı
5. **AfterStep** - Step sonu
6. **AfterScenario** - Senaryo sonu
7. **AfterFeature** - Feature sonu
8. **AfterTestRun** - Test suite sonu

## 🚀 Kullanım

### Test Çalıştırma
```bash
# Tüm testleri çalıştır
dotnet test

# Belirli tag ile çalıştır
dotnet test --filter "Category=Smoke"

# Detaylı log ile çalıştır
dotnet test --logger "console;verbosity=detailed"

# Paralel çalıştır
dotnet test --logger "console;verbosity=detailed" --verbosity normal
```

### Proje Yönetimi
```bash
# Paketleri restore et
dotnet restore

# Projeyi derle
dotnet build

# Projeyi temizle
dotnet clean

# Test coverage
dotnet test --collect:"XPlat Code Coverage"
```

## 📊 Raporlama

### ExtentReports
- HTML tabanlı görsel raporlar
- Test sonuçları ve istatistikler
- Screenshot entegrasyonu
- Timeline ve trend analizi

### Serilog Logging
- Yapılandırılmış loglama
- Dosya bazlı log rotasyonu
- Timestamp ve log level desteği
- Debug ve production logları

## 🔧 Geliştirme

### Yeni Test Ekleme
1. **Feature dosyası oluştur**
   ```gherkin
   @NewFeature
   Feature: Yeni Test Özelliği
     Scenario: Test Senaryosu
       Given precondition
       When action
       Then result
   ```

2. **Step definitions yaz**
   ```csharp
   [Given(@"precondition")]
   public void GivenPrecondition()
   {
       // Implementation
   }
   ```

3. **Page object ekle**
   ```csharp
   public class NewPage : CommonPage
   {
       private By elementLocator = By.Id("element");
       
       public void PerformAction()
       {
           WaitAndClick(elementLocator);
       }
   }
   ```

### Yeni Tarayıcı Ekleme
1. **Browser.cs'e yeni case ekle**
2. **Driver paketini ekle**
3. **Konfigürasyonu güncelle**
4. **Test et**

## 🐛 Sorun Giderme

### Yaygın Sorunlar
1. **Element bulunamadı**
   - Locator'ları kontrol et
   - Explicit wait kullan
   - Sayfa yüklenme durumunu kontrol et

2. **Driver başlatılamadı**
   - Driver versiyonunu kontrol et
   - Tarayıcı versiyonu ile uyumluluğu doğrula
   - Antivirus/firewall ayarlarını kontrol et

3. **Test timeout**
   - Bekleme sürelerini artır
   - Network bağlantısını kontrol et
   - Headless modu dene

### Debug İpuçları
- `Headless: false` ile görsel debug
- Screenshot'ları kontrol et
- Log dosyalarını incele
- Browser console'u kontrol et

## 📈 Performans

### Optimizasyonlar
- **Parallel Execution** - Test paralel çalıştırma
- **Headless Mode** - Görsel olmayan mod
- **Driver Reuse** - Driver yeniden kullanımı
- **Smart Waits** - Akıllı bekleme stratejileri

### Best Practices
- Page Object Model kullan
- Explicit wait tercih et
- Screenshot'ları optimize et
- Log seviyelerini ayarla

## 🔒 Güvenlik

### Tarayıcı Güvenliği
- Otomasyon algılama bypass
- User agent güncelleme
- Certificate hatalarını ignore et
- Sandbox modu kullan

### Test Verisi Güvenliği
- Hassas verileri encrypt et
- Test ortamı izolasyonu
- Cleanup işlemleri
- Audit logging

## 📚 Kaynaklar

### Dokümantasyon
- [Selenium WebDriver](https://selenium.dev/)
- [SpecFlow](https://specflow.org/)
- [NUnit](https://nunit.org/)
- [Serilog](https://serilog.net/)

### Örnekler
- Feature dosyaları: `Features/`
- Step definitions: `StepDefinitions/`
- Page objects: `Pages/`
- Test data: `Models/`

---

**Proje Versiyonu:** 1.0.0  
**Son Güncelleme:** 2024  
**Teknoloji:** .NET 9, Selenium 4, SpecFlow 3.9.74 