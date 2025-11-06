# 🚀 API Test Otomasyonu - Kurulum Rehberi

Bu rehber, API Test Otomasyonu projesini çalıştırmak için gerekli tüm ücretsiz uygulamaların adım adım kurulumunu içerir.

## 📋 Gereksinimler Listesi

Aşağıdaki uygulamaların kurulması gerekmektedir:

1. ✅ Visual Studio Community 2022 (Ücretsiz)
2. ✅ .NET 9 SDK (Ücretsiz)
3. ✅ Java JDK 17 veya üzeri (Ücretsiz) - Allure CLI için
4. ✅ Allure Command Line Tool (Ücretsiz)
5. ✅ Git (Opsiyonel - Ücretsiz)

---

## 📦 Adım 1: Visual Studio Community 2022 Kurulumu

### 1.1. İndirme
1. Tarayıcınızda şu adrese gidin: **https://visualstudio.microsoft.com/tr/vs/community/**
2. Sayfanın ortasında yer alan **"Ücretsiz indir"** butonuna tıklayın
3. İndirme başlayacaktır (yaklaşık 3-5 MB installer dosyası)

### 1.2. Kurulum
1. İndirilen `vs_community.exe` dosyasına çift tıklayın
2. **"Kullanıcı hesabı denetimi"** penceresinde **"Evet"** butonuna tıklayın
3. Kurulum ekranı açıldığında:
   - **"Geliştirme için Azure"** bölümünü işaretleyin (opsiyonel)
   - **"Geliştirme için .NET desktop"** bölümünü **MUTLAKA** işaretleyin
   - **"ASP.NET ve web geliştirme"** bölümünü işaretleyin (API testleri için önerilir)
4. Sağ alt köşede **"Yükle"** butonuna tıklayın
5. Kurulum tamamlanana kadar bekleyin (yaklaşık 15-30 dakika, internet hızınıza bağlı)
6. Kurulum tamamlandığında **"Başlat"** butonuna tıklayın

### 1.3. İlk Açılış
1. Visual Studio açıldığında Microsoft hesabınızla giriş yapın (veya hesap oluşturun)
2. Geliştirme ayarlarını seçin (varsayılan ayarları kullanabilirsiniz)
3. **"Visual Studio'yu başlat"** butonuna tıklayın

---

## 📦 Adım 2: .NET 9 SDK Kurulumu

### 2.1. İndirme
1. Tarayıcınızda şu adrese gidin: **https://dotnet.microsoft.com/download/dotnet/9.0**
2. **".NET 9.0 SDK"** bölümünü bulun
3. İşletim sisteminize uygun olanı seçin:
   - **Windows x64**: `dotnet-sdk-9.0.x-win-x64.exe` dosyasını indirin

### 2.2. Kurulum
1. İndirilen `.exe` dosyasına çift tıklayın
2. Kurulum sihirbazını takip edin:
   - **"İleri"** butonuna tıklayın
   - Lisans sözleşmesini kabul edin
   - **"Yükle"** butonuna tıklayın
3. Kurulum tamamlandığında **"Son"** butonuna tıklayın

### 2.3. Kurulum Doğrulama
1. **Windows PowerShell** veya **Command Prompt** açın
2. Şu komutu çalıştırın:
   ```bash
   dotnet --version
   ```
3. Çıktı `9.0.x` şeklinde bir versiyon numarası göstermelidir
4. Eğer komut bulunamadı hatası verirse, bilgisayarınızı yeniden başlatın

---

## 📦 Adım 3: Java JDK Kurulumu (Allure CLI için)

### 3.1. İndirme
1. Tarayıcınızda şu adrese gidin: **https://adoptium.net/temurin/releases/**
2. **"Version"** olarak **17** veya **21** seçin
3. **"Operating System"** olarak **Windows** seçin
4. **"Architecture"** olarak **x64** seçin
5. **"Package Type"** olarak **JDK** seçin
6. **"İndir"** butonuna tıklayın (`.msi` dosyası indirilecek)

### 3.2. Kurulum
1. İndirilen `.msi` dosyasına çift tıklayın
2. Kurulum sihirbazını takip edin:
   - **"İleri"** butonuna tıklayın
   - Lisans sözleşmesini kabul edin
   - **"İleri"** butonuna tıklayın
   - Kurulum konumunu değiştirmeyin (varsayılan yeterli)
   - **"Yükle"** butonuna tıklayın
3. Kurulum tamamlandığında **"Son"** butonuna tıklayın

### 3.3. Kurulum Doğrulama
1. **Windows PowerShell** veya **Command Prompt** açın
2. Şu komutu çalıştırın:
   ```bash
   java -version
   ```
3. Çıktı Java versiyon bilgisini göstermelidir (örnek: `openjdk version "17.0.x"`)
4. Eğer komut bulunamadı hatası verirse, bilgisayarınızı yeniden başlatın

---

## 📦 Adım 4: Allure Command Line Tool Kurulumu

### 4.1. Yöntem 1: npm ile Kurulum (Önerilen)

#### 4.1.1. Node.js Kurulumu (Eğer yoksa)
1. Tarayıcınızda şu adrese gidin: **https://nodejs.org/**
2. **"LTS"** (Long Term Support) versiyonunu indirin
3. İndirilen `.msi` dosyasına çift tıklayın
4. Kurulum sihirbazını takip edin (varsayılan ayarlar yeterli)
5. Kurulum tamamlandıktan sonra bilgisayarınızı yeniden başlatın

#### 4.1.2. Allure CLI Kurulumu
1. **Windows PowerShell** veya **Command Prompt** açın (Yönetici olarak)
2. Şu komutu çalıştırın:
   ```bash
   npm install -g allure-commandline
   ```
3. Kurulum tamamlanana kadar bekleyin (birkaç dakika sürebilir)

### 4.2. Yöntem 2: Scoop ile Kurulum (Alternatif)

#### 4.2.1. Scoop Kurulumu (Eğer yoksa)
1. **Windows PowerShell** açın (Yönetici olarak)
2. Şu komutu çalıştırın:
   ```powershell
   Set-ExecutionPolicy RemoteSigned -Scope CurrentUser
   Invoke-RestMethod get.scoop.sh | Invoke-Expression
   ```

#### 4.2.2. Allure CLI Kurulumu
1. **Windows PowerShell** açın
2. Şu komutu çalıştırın:
   ```bash
   scoop install allure
   ```

### 4.3. Kurulum Doğrulama
1. **Windows PowerShell** veya **Command Prompt** açın
2. Şu komutu çalıştırın:
   ```bash
   allure --version
   ```
3. Çıktı Allure versiyon bilgisini göstermelidir (örnek: `2.13.0` veya `2.24.0`)

---

## 📦 Adım 5: Git Kurulumu (Opsiyonel)

### 5.1. İndirme
1. Tarayıcınızda şu adrese gidin: **https://git-scm.com/download/win**
2. İndirme otomatik olarak başlayacaktır

### 5.2. Kurulum
1. İndirilen `.exe` dosyasına çift tıklayın
2. Kurulum sihirbazını takip edin:
   - Varsayılan ayarları kullanabilirsiniz
   - **"Git from the command line and also from 3rd-party software"** seçeneğini seçin
   - **"Use the OpenSSL library"** seçeneğini seçin
   - **"Checkout Windows-style, commit Unix-style line endings"** seçeneğini seçin
   - **"Use Windows' default console window"** seçeneğini seçin
3. **"Install"** butonuna tıklayın
4. Kurulum tamamlandığında **"Finish"** butonuna tıklayın

### 5.3. Kurulum Doğrulama
1. **Windows PowerShell** veya **Command Prompt** açın
2. Şu komutu çalıştırın:
   ```bash
   git --version
   ```
3. Çıktı Git versiyon bilgisini göstermelidir

---

## ✅ Kurulum Kontrol Listesi

Tüm kurulumların tamamlandığını doğrulamak için aşağıdaki komutları çalıştırın:

```bash
# .NET SDK kontrolü
dotnet --version

# Java kontrolü
java -version

# Allure CLI kontrolü
allure --version

# Node.js kontrolü (npm ile kurulum yaptıysanız)
node --version
npm --version

# Git kontrolü (opsiyonel)
git --version
```

Tüm komutlar başarılı bir şekilde versiyon numarası göstermelidir.

---

## 🚀 Projeyi Çalıştırma

### 1. Projeyi Klonlama veya Açma
1. Visual Studio Community'yi açın
2. **"Open a project or solution"** seçeneğine tıklayın
3. Proje klasöründeki `NewTestAutomation.sln` dosyasını seçin
4. Veya `ApiTestAutomationProject` klasöründeki `.csproj` dosyasını açın

### 2. NuGet Paketlerini Geri Yükleme
1. Visual Studio'da **"Solution Explorer"** penceresinde projeye sağ tıklayın
2. **"Restore NuGet Packages"** seçeneğine tıklayın
3. Veya terminal'de proje klasörüne gidin ve şu komutu çalıştırın:
   ```bash
   cd src/ApiTestAutomationProject
   dotnet restore
   ```

### 3. Testleri Çalıştırma

#### Yöntem 1: Visual Studio Test Explorer
1. Visual Studio'da **"Test"** menüsünden **"Test Explorer"** seçeneğine tıklayın
2. **"Run All Tests"** butonuna tıklayın
3. Test sonuçları Test Explorer penceresinde görünecektir

#### Yöntem 2: Terminal/Command Line
1. Terminal'de proje klasörüne gidin:
   ```bash
   cd src/ApiTestAutomationProject
   ```
2. Tüm testleri çalıştırın:
   ```bash
   dotnet test
   ```
3. Belirli tag'li testleri çalıştırın:
   ```bash
   # @need tag'li testler
   dotnet test --filter "FullyQualifiedName~need"
   
   # @need:update tag'li testler
   dotnet test --filter "FullyQualifiedName~PostUpdate"
   ```

### 4. Allure Raporu Oluşturma

#### Yöntem 1: Batch Dosyası ile (Önerilen)
1. Proje klasöründe `generate-allure-report.bat` dosyasına çift tıklayın
2. Otomatik olarak:
   - Testler çalıştırılacak
   - Allure raporu oluşturulacak
   - Rapor tarayıcıda açılacak

#### Yöntem 2: Manuel Komutlar ile
1. Terminal'de proje klasörüne gidin:
   ```bash
   cd src/ApiTestAutomationProject
   ```
2. Testleri çalıştırın:
   ```bash
   dotnet test
   ```
3. Allure raporu oluşturun:
   ```bash
   allure generate allure-results --clean -o allure-report
   ```
4. Raporu açın:
   ```bash
   allure open allure-report
   ```

---

## 📊 API Test Senaryoları

### Mevcut Test Senaryoları
- ✅ **Post Create Test**: Yeni bir post oluşturma testi
- ✅ **Post Update Test**: Mevcut bir post'u güncelleme testi

### Test Senaryosu Ekleme
1. `Features/` klasöründe yeni bir `.feature` dosyası oluşturun
2. Gherkin syntax kullanarak test senaryolarını yazın
3. `Steps/` klasöründe step definition'ları oluşturun
4. Testleri çalıştırın

---

## 🔧 Konfigürasyon

### appsettings.json
API endpoint'lerini ve timeout ayarlarını bu dosyada yapılandırabilirsiniz:

```json
{
  "ApiSettings": {
    "BaseUrl": "https://reqres.in/api",
    "Timeout": 30000
  }
}
```

---

## 🏷️ Tag Sistemi

### Kullanılabilir Tag'ler
- `@need` - Genel test hook'ları
- `@need:create` - Create testleri için özel hook'lar
- `@need:update` - Update testleri için özel hook'lar
- `@need:delete` - Delete testleri için özel hook'lar
- `@need:get` - Get testleri için özel hook'lar

### Tag Kullanımı
```gherkin
@need:update
Scenario: Update post test
    Given the user is logged in
    When the post is updated
    Then the update should be successful
```

---

## 🐛 Yaygın Sorunlar ve Çözümleri

### Sorun 1: "dotnet komutu bulunamadı"
**Çözüm:**
- Bilgisayarınızı yeniden başlatın
- PATH ortam değişkenlerini kontrol edin
- .NET SDK'yı yeniden kurun

### Sorun 2: "java komutu bulunamadı"
**Çözüm:**
- Bilgisayarınızı yeniden başlatın
- JAVA_HOME ortam değişkenini kontrol edin
- Java'yı yeniden kurun

### Sorun 3: "allure komutu bulunamadı"
**Çözüm:**
- npm global paketlerinin PATH'te olduğundan emin olun
- `npm config get prefix` komutunu çalıştırın ve çıktıyı PATH'e ekleyin
- Scoop ile kurulum yaptıysanız, Scoop'un PATH'te olduğundan emin olun

### Sorun 4: "Testler çalışmıyor"
**Çözüm:**
- Visual Studio'da **"Build"** menüsünden **"Rebuild Solution"** seçeneğine tıklayın
- `dotnet clean` ve `dotnet build` komutlarını çalıştırın
- Test Explorer'da hataları kontrol edin

### Sorun 5: "API çağrıları başarısız"
**Çözüm:**
- İnternet bağlantınızı kontrol edin
- API endpoint'lerini `appsettings.json` dosyasında kontrol edin
- Firewall veya proxy ayarlarınızı kontrol edin

### Sorun 6: "Allure raporu oluşturulamıyor"
**Çözüm:**
- `allure-results` klasörünün var olduğundan emin olun
- Testlerin çalıştırıldığından emin olun
- Allure CLI'nın doğru kurulduğundan emin olun (`allure --version`)

---

## 📁 Proje Yapısı

```
ApiTestAutomationProject/
├── Features/                 # Gherkin feature dosyaları
│   ├── Post.Create.feature
│   └── Post.Update.feature
├── Steps/                   # Step definition'ları
│   ├── PostCreateSteps.cs
│   ├── PostUpdateSteps.cs
│   └── ApiAssertionHelper.cs
├── Hooks/                   # SpecFlow hook'ları
│   └── SpecFlowHooks.cs
├── Drivers/                 # API client ve driver'lar
│   ├── EnhancedApiClient.cs
│   └── EndpointManager.cs
├── Models/                  # API model sınıfları
│   ├── ApiModels.cs
│   ├── CreatePostRequest.cs
│   ├── GetPostResponse.cs
│   └── UpdatePostRequest.cs
├── TestData/                # Test verileri
│   ├── PostTestData.cs
│   └── TestDataManager.cs
├── Constants/               # Sabitler
│   └── ApiEndpoints.cs
├── allure-results/          # Allure sonuçları
├── allure-report/           # Allure raporları
└── logs/                    # Log dosyaları
```

---

## 📞 Yardım ve Destek

Sorun yaşarsanız:
1. Proje dokümantasyonunu kontrol edin
2. GitHub Issues bölümünde sorun arayın
3. Yeni bir issue açın

---

## 📚 Ek Kaynaklar

- **.NET Documentation**: https://docs.microsoft.com/dotnet
- **SpecFlow Documentation**: https://docs.specflow.org/
- **Allure Framework**: https://docs.qameta.io/allure/
- **REST API Testing Best Practices**: https://restfulapi.net/

---

**Son Güncelleme:** 2024  
**Versiyon:** 1.0.0

