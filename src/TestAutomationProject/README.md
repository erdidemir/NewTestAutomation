# Google Login Test Otomasyonu

Bu proje, Google login sayfası için Selenium WebDriver kullanarak test otomasyonu sağlar.

## Özellikler

- **Edge ve Chrome** tarayıcı desteği
- **SpecFlow** ile BDD (Behavior Driven Development) yaklaşımı
- **NUnit 4** test framework'ü
- **Allure** raporlama
- **Serilog** loglama
- **.NET 9** uyumluluğu

## Kurulum

### Gereksinimler
- .NET 9 SDK
- Edge veya Chrome tarayıcısı
- Visual Studio 2022 veya VS Code

### Paketler
Proje aşağıdaki NuGet paketlerini kullanır:
- Selenium.WebDriver (4.34.0)
- Selenium.WebDriver.EdgeDriver (3.14393.0.1)
- Selenium.WebDriver.ChromeDriver (138.0.7204.18300)
- SeleniumExtras.WaitHelpers (1.0.2)
- SpecFlow.NUnit (3.9.74)
- NUnit (4.2.2)
- Allure.Commons (3.5.0.73)
- Serilog (3.1.1)

## Konfigürasyon

### appsettings.json
```json
{
  "Configuration": {
    "Browser": "edge",
    "Headless": false,
    "BaseUrl": "https://accounts.google.com",
    "ElementDelayMilliSeconds": 5000
  }
}
```

## Test Senaryoları

### Google Login Testleri
1. **Başarılı Google Login** - Geçerli kullanıcı bilgileriyle giriş
2. **Geçersiz Email** - Var olmayan email ile giriş denemesi
3. **Geçersiz Şifre** - Yanlış şifre ile giriş denemesi
4. **Test Başlatma** - Sayfa yükleme kontrolü

## Test Çalıştırma

### Tüm Testleri Çalıştırma
```bash
dotnet test
```

### Belirli Tag ile Test Çalıştırma
```bash
dotnet test --filter "Category=Smoke"
dotnet test --filter "Category=Positive"
dotnet test --filter "Category=Negative"
```

### Belirli Test Dosyasını Çalıştırma
```bash
dotnet test --filter "FullyQualifiedName~GoogleLogin"
```

## SpecFlow Feature Dosyası

### GoogleLogin.feature
```gherkin
@GoogleLogin
Feature: Google Login Testleri
  Kullanıcı olarak Google hesabıma giriş yapabilmek istiyorum
  Böylece Google servislerini kullanabilirim

  Background:
    Given kullanıcı test için hazır
    And kullanıcı Google login sayfasında

  @Positive @Smoke
  Scenario: Başarılı Google Login
    When kullanıcı email adresini girer: "testuser@gmail.com"
    And kullanıcı email sonrası Next butonuna tıklar
    And kullanıcı şifresini girer: "testpassword123"
    And kullanıcı şifre sonrası Next butonuna tıklar
    Then Google logosu görünür olmalı
```

## Step Definitions

### GoogleLoginSteps.cs
- `GivenKullaniciGoogleLoginSayfasinda()` - Google login sayfasına gitme
- `WhenKullaniciEmailAdresiniGirer(string email)` - Email girme
- `WhenKullaniciSifresiniGirer(string password)` - Şifre girme
- `ThenGoogleLogosuGorunurOlmali()` - Logo kontrolü
- `WhenTestiBaslat()` - Test başlatma

## Page Object Model

### LoginPage.cs
- `NavigateToGoogleLogin()` - Google login sayfasına gitme
- `EnterEmail(string email)` - Email girme
- `EnterPassword(string password)` - Şifre girme
- `LoginToGoogle(string email, string password)` - Tam login akışı
- `GetErrorMessage()` - Hata mesajı alma
- `IsGoogleLogoVisible()` - Logo görünürlük kontrolü

### CommonPage.cs
- `WaitForPageLoad()` - Sayfa yükleme bekleme
- `WaitUntilElementVisible(By by)` - Element görünürlük bekleme
- `WaitAndClick(By by)` - Bekleyip tıklama
- `WaitAndSendKeys(By by, string text)` - Bekleyip metin gönderme
- `ClickWithJavaScript(By by)` - JavaScript ile tıklama
- `ScrollToElement(By by)` - Elemente kaydırma

## Tarayıcı Konfigürasyonu

### Edge Tarayıcısı
- Otomasyon algılama bypass
- Performance optimizasyonları
- Güncel user agent
- Güvenlik ayarları

### Chrome Tarayıcısı
- Otomasyon algılama bypass
- Performance optimizasyonları
- Güncel user agent
- Güvenlik ayarları

## Raporlama

### Allure Raporları
- Test sonuçları
- Screenshot'lar
- Log'lar
- Performance metrikleri

### Serilog Logları
- Detaylı loglama
- Dosya bazlı loglama
- Timestamp'li loglar

## Proje Yapısı

```
TestAutomationProject/
├── Core/
│   ├── Browser.cs
│   ├── Configuration.cs
│   └── Hooks/
│       └── Hooks.cs
├── Pages/
│   ├── CommonPage.cs
│   ├── LoginPage.cs
│   └── InteroperabilityPage.cs
├── StepDefinitions/
│   └── GoogleLoginSteps.cs
├── Features/
│   └── GoogleLogin.feature
├── Models/
├── Helpers/
└── Results/
```

## Geliştirme

### Yeni Test Ekleme
1. Feature dosyası oluştur
2. Step definitions yaz
3. Page object metodları ekle
4. Test çalıştır

### Yeni Sayfa Ekleme
1. Page sınıfı oluştur
2. Element locator'ları tanımla
3. Sayfa metodları yaz
4. Test'lerde kullan

## Sorun Giderme

### Yaygın Sorunlar
1. **Element bulunamadı** - Locator'ları kontrol et
2. **Timeout hatası** - Bekleme sürelerini artır
3. **Tarayıcı başlatılamadı** - Driver versiyonunu kontrol et
4. **Test başarısız** - Assert mesajlarını kontrol et

### Debug İpuçları
- Headless modu kapat
- Screenshot'ları kontrol et
- Log dosyalarını incele
- Browser console'u kontrol et 