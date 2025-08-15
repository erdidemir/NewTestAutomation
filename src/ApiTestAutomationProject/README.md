# API Test Automation Project

Bu proje, API testlerini otomatize etmek için geliştirilmiş bir test automation framework'üdür.

## 🚀 Özellikler

- **SpecFlow BDD Framework**: Behavior Driven Development yaklaşımı
- **Allure Reporting**: Detaylı test raporları
- **@need Tag Sistemi**: Test çalıştırma kontrolü
- **Hook'lar**: Test öncesi ve sonrası otomatik işlemler
- **API Client**: Gelişmiş HTTP client
- **Test Data Management**: Merkezi test verisi yönetimi

## 📋 Gereksinimler

- .NET 9.0
- Allure Command Line Tool
- Visual Studio 2022 veya VS Code

## 🛠️ Kurulum

1. **Allure CLI Kurulumu**:
   ```bash
   # Windows için
   scoop install allure
   
   # veya npm ile
   npm install -g allure-commandline
   ```

2. **Proje Bağımlılıkları**:
   ```bash
   dotnet restore
   ```

## 🏃‍♂️ Test Çalıştırma

### Tüm Testleri Çalıştırma
```bash
dotnet test
```

### Belirli Tag'li Testleri Çalıştırma
```bash
# @need tag'li testler
dotnet test --filter "FullyQualifiedName~need"

# @need:update tag'li testler
dotnet test --filter "FullyQualifiedName~PostUpdate"
```

### Allure Report ile Çalıştırma
```bash
# Batch dosyası ile
generate-allure-report.bat

# Manuel olarak
dotnet test
allure generate allure-results --clean -o allure-report
allure open allure-report
```

## 🏷️ Tag Sistemi

### @need Tag'leri
- `@need` - Genel test hook'ları
- `@need:create` - Create testleri için özel hook'lar
- `@need:update` - Update testleri için özel hook'lar
- `@need:delete` - Delete testleri için özel hook'lar
- `@need:get` - Get testleri için özel hook'lar

### Kullanım
```gherkin
@need:update
Scenario: Update post test
    Given the user is logged in
    When the post is updated
    Then the update should be successful
```

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
│   └── EnhancedApiClient.cs
├── Models/                  # API model sınıfları
├── TestData/                # Test verileri
│   └── PostTestData.cs
├── allure-results/          # Allure sonuçları
├── allure-report/           # Allure raporları
└── logs/                    # Log dosyaları
```

## 🔧 Konfigürasyon

### appsettings.json
```json
{
  "ApiSettings": {
    "BaseUrl": "https://reqres.in/api",
    "Timeout": 30000
  }
}
```

## 📊 Allure Report

Allure report şunları içerir:
- Test sonuçları özeti
- API çağrı detayları
- Hata mesajları
- Test süreleri
- Attachment'lar (JSON, log dosyaları)

## 🐛 Sorun Giderme

### Yaygın Sorunlar

1. **Allure CLI Bulunamadı**:
   ```bash
   # PATH'e ekleyin veya yeniden kurun
   npm install -g allure-commandline
   ```

2. **Test Başarısız**:
   - API endpoint'lerini kontrol edin
   - Test verilerini doğrulayın
   - Log dosyalarını inceleyin

3. **Hook Çalışmıyor**:
   - Tag'lerin doğru yazıldığından emin olun
   - Dependency injection'ı kontrol edin

## 🤝 Katkıda Bulunma

1. Fork yapın
2. Feature branch oluşturun (`git checkout -b feature/amazing-feature`)
3. Commit yapın (`git commit -m 'Add amazing feature'`)
4. Push yapın (`git push origin feature/amazing-feature`)
5. Pull Request oluşturun

## 📝 Lisans

Bu proje MIT lisansı altında lisanslanmıştır.

## 📞 İletişim

Sorularınız için issue açabilir veya pull request gönderebilirsiniz. 