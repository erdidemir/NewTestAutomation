# Visual Studio SpecFlow Kurulum Rehberi

## 🚀 Gerekli Eklentiler

### 📋 Zorunlu Eklentiler

1. **SpecFlow for Visual Studio 2022**
   - ID: `TechTalkSpecFlowTeam.SpecFlowVSExtension2022`
   - SpecFlow entegrasyonu ve IntelliSense

2. **SpecFlow LivingDoc Generator**
   - Living documentation oluşturur
   - HTML raporları üretir

### 🔧 Faydalı Eklentiler

3. **NUnit Test Adapter**
   - NUnit testleri için Visual Studio entegrasyonu

4. **Test Explorer**
   - Test keşfi ve çalıştırma

## 📥 Kurulum Adımları

### Yöntem 1: Visual Studio Extensions
1. Visual Studio'da **Tools** → **Extensions and Updates** menüsüne gidin
2. **Online** sekmesine tıklayın
3. Arama kutusuna "SpecFlow" yazın
4. **"SpecFlow for Visual Studio 2022"** eklentisini bulun ve yükleyin
5. Visual Studio'yu yeniden başlatın

### Yöntem 2: Visual Studio Marketplace
1. [SpecFlow for Visual Studio 2022](https://marketplace.visualstudio.com/items?itemName=TechTalkSpecFlowTeam.SpecFlowVSExtension2022) sayfasına gidin
2. "Download" butonuna tıklayın
3. İndirilen `.vsix` dosyasını çalıştırın
4. Visual Studio'yu yeniden başlatın

## ⚙️ Proje Ayarları

### SpecFlow Konfigürasyonu
Proje zaten `specflow.json` dosyası ile yapılandırılmıştır:

```json
{
  "bindingCulture": {
    "name": "en-US"
  },
  "language": {
    "feature": "en-US"
  },
  "generator": {
    "allowRowTests": true,
    "allowDebugGeneratedFiles": true,
    "generateAsyncTests": false,
    "allowParallelExecution": true
  }
}
```

### NuGet Paketleri
Proje zaten gerekli NuGet paketlerini içerir:
- `SpecFlow.NUnit`
- `SpecFlow.Tools.MsBuild.Generation`
- `SpecFlow.Plus.LivingDocPlugin`

## 🎯 Özellikler

Kurulum tamamlandıktan sonra:

### ✅ SpecFlow Desteği
- Feature dosyalarında syntax highlighting
- Step tanımları için IntelliSense
- F12 ile step definition'lara gitme
- Renklendirme ve autocomplete

### ✅ Test Explorer Entegrasyonu
- Test Explorer'da SpecFlow testleri görünür
- Test çalıştırma ve debug
- Test sonuçları görüntüleme

### ✅ Living Documentation
- HTML raporları oluşturma
- Feature dosyalarından otomatik dokümantasyon
- Test sonuçları ile entegrasyon

## 🔍 Sorun Giderme

### SpecFlow IntelliSense Çalışmıyor
1. Visual Studio'yu yeniden başlatın
2. **Tools** → **Options** → **Text Editor** → **C#** → **Advanced**
3. "Enable IntelliSense" seçeneğinin işaretli olduğundan emin olun

### Feature Dosyaları Renkli Görünmüyor
1. SpecFlow eklentisinin yüklü olduğundan emin olun
2. Feature dosyalarının `.feature` uzantısında olduğunu kontrol edin
3. Visual Studio'yu yeniden başlatın

### Test Explorer'da Testler Görünmüyor
1. **Test** → **Test Explorer** menüsünü açın
2. **Build** → **Build Solution** ile projeyi derleyin
3. Test Explorer'da "Refresh" butonuna tıklayın

### Step Definition'lar Bulunamıyor
1. Step definition dosyalarının doğru klasörde olduğunu kontrol edin
2. `[Binding]` attribute'unun eklendiğinden emin olun
3. Projeyi yeniden derleyin

## 📚 Faydalı Kısayollar

- `F12`: Go to definition
- `Ctrl+Shift+F12`: Go to implementation
- `Ctrl+Space`: IntelliSense
- `Ctrl+K, Ctrl+D`: Format document
- `Ctrl+K, Ctrl+C`: Comment selection
- `Ctrl+K, Ctrl+U`: Uncomment selection

## 🚀 Test Çalıştırma

### Visual Studio Test Explorer
1. **Test** → **Test Explorer** menüsünü açın
2. İstediğiniz testi seçin
3. "Run" butonuna tıklayın

### Command Line
```bash
# Tüm testleri çalıştır
dotnet test

# Belirli testleri çalıştır
dotnet test --filter "GoogleSearch"

# API testlerini çalıştır
dotnet test src\ApiTestAutomationProject\
```

## 📊 Raporlama

### Allure Raporları
```bash
# Allure raporu oluştur
allure serve allure-results

# HTML raporu oluştur
allure generate allure-results --clean
```

### Living Documentation
```bash
# Living documentation oluştur
dotnet livingdoc feature-folder src\TestAutomationProject\Features -t src\TestAutomationProject\bin\Debug\net9.0\TestExecution.json
```

## 🔧 Önerilen Ayarlar

### Visual Studio Ayarları
1. **Tools** → **Options** → **Text Editor** → **C#**
   - "Enable IntelliSense" ✓
   - "Auto list members" ✓
   - "Parameter information" ✓

2. **Tools** → **Options** → **Environment** → **Documents**
   - "Detect when file is changed outside the environment" ✓

3. **Tools** → **Options** → **Projects and Solutions** → **Build and Run**
   - "On Run, when projects are out of date" → "Always build" 