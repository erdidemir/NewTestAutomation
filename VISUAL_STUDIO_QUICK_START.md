# Visual Studio Hızlı Başlangıç Rehberi

## 🚀 Proje Açıldıktan Sonra Yapılacaklar

### 1. SpecFlow Eklentisini Yükleyin
1. **Tools** → **Extensions and Updates** menüsüne gidin
2. **Online** sekmesine tıklayın
3. Arama kutusuna "SpecFlow" yazın
4. **"SpecFlow for Visual Studio 2022"** eklentisini bulun ve yükleyin
5. Visual Studio'yu yeniden başlatın

### 2. Solution'ı Build Edin
1. **Build** → **Build Solution** (Ctrl+Shift+B)
2. Build'in başarılı olduğundan emin olun

### 3. Test Explorer'ı Açın
1. **Test** → **Test Explorer** menüsüne gidin
2. Testlerin yüklendiğini kontrol edin

## 🎯 SpecFlow Özellikleri

### ✅ Feature Dosyaları
- `.feature` dosyaları renkli görünür
- Syntax highlighting aktif
- Step tanımları otomatik tamamlanır

### ✅ Step Definitions
- F12 ile step definition'lara gitme
- IntelliSense desteği
- Refactoring araçları

### ✅ Test Çalıştırma
- Test Explorer'da testleri çalıştırma
- Debug desteği
- Test sonuçları görüntüleme

## 📁 Proje Yapısı

```
NewTestAutomation/
├── src/
│   ├── TestAutomationProject/          # Web UI Testleri
│   │   ├── Features/                   # Feature dosyaları
│   │   ├── StepDefinitions/           # Step tanımları
│   │   ├── Pages/                     # Page Object Model
│   │   └── Core/                      # Temel sınıflar
│   └── ApiTestAutomationProject/      # API Testleri
│       ├── Features/                  # API Feature dosyaları
│       ├── Steps/                     # API Step tanımları
│       └── Drivers/                   # API sürücüleri
├── specflow.json                      # SpecFlow konfigürasyonu
└── NewTestAutomation.sln              # Solution dosyası
```

## 🧪 Test Çalıştırma

### Visual Studio Test Explorer
1. **Test** → **Test Explorer** menüsünü açın
2. İstediğiniz testi seçin
3. "Run" butonuna tıklayın

### Command Line
```bash
# Tüm testleri çalıştır
dotnet test

# Web testlerini çalıştır
dotnet test src\TestAutomationProject\ --filter "GoogleSearch"

# API testlerini çalıştır
dotnet test src\ApiTestAutomationProject\ --filter "PostCreate"
```

## 🔧 Önemli Dosyalar

### SpecFlow Konfigürasyonu
- `specflow.json`: SpecFlow ayarları
- `.editorconfig`: Kod formatı ayarları

### AppSettings
- `src/TestAutomationProject/appsettings.json`: Web test ayarları
- `src/ApiTestAutomationProject/appsettings.json`: API test ayarları

### NuGet Paketleri
- `Directory.Packages.props`: Merkezi paket yönetimi
- Her proje kendi `.csproj` dosyasında paket referansları

## 🎯 Özellikler

### Web UI Testleri
- Selenium WebDriver
- Multi-browser desteği
- Page Object Model
- Allure raporlama

### API Testleri
- RestSharp
- FluentAssertions
- Serilog loglama
- Dependency Injection

### Ortak Özellikler
- SpecFlow BDD
- NUnit test framework
- Clean Architecture
- Centralized configuration

## 🔍 Sorun Giderme

### SpecFlow IntelliSense Çalışmıyor
1. SpecFlow eklentisinin yüklü olduğundan emin olun
2. Visual Studio'yu yeniden başlatın
3. Solution'ı yeniden build edin

### Testler Görünmüyor
1. **Build** → **Build Solution**
2. Test Explorer'da "Refresh" butonuna tıklayın
3. NuGet paketlerinin restore edildiğinden emin olun

### Step Definition'lar Bulunamıyor
1. `[Binding]` attribute'unun eklendiğini kontrol edin
2. Step definition dosyalarının doğru klasörde olduğunu kontrol edin
3. Solution'ı yeniden build edin

## 📚 Faydalı Kısayollar

- `F12`: Go to definition
- `Ctrl+Shift+F12`: Go to implementation
- `Ctrl+Space`: IntelliSense
- `Ctrl+K, Ctrl+D`: Format document
- `Ctrl+Shift+B`: Build Solution
- `Ctrl+Shift+T`: Test Explorer

## 🚀 Sonraki Adımlar

1. **SpecFlow eklentisini yükleyin**
2. **Solution'ı build edin**
3. **Test Explorer'da testleri çalıştırın**
4. **Feature dosyalarında step tanımlarını test edin**
5. **F12 ile navigasyonu deneyin**

Artık Visual Studio'da SpecFlow ile tam entegrasyon hazır! 🎉 