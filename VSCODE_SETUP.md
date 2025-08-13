# VS Code Kurulum Rehberi

## 🚀 Gerekli Eklentiler

Bu proje için aşağıdaki VS Code eklentilerini yüklemeniz gerekiyor:

### 📋 Zorunlu Eklentiler

1. **C# Dev Kit** - Microsoft
   - ID: `ms-dotnettools.csdevkit`
   - C# dil desteği ve IntelliSense

2. **C#** - Microsoft
   - ID: `ms-dotnettools.csharp`
   - C# temel desteği

3. **.NET Runtime Install Tool** - Microsoft
   - ID: `ms-dotnettools.vscode-dotnet-runtime`
   - .NET runtime desteği

4. **Cucumber (Gherkin)** - Cucumber
   - ID: `cucumberopen.cucumber-official`
   - Feature dosyaları için syntax highlighting

5. **Cucumber (Gherkin) Full Support** - Alexander Krechik
   - ID: `alexkrechik.cucumberautocomplete`
   - Cucumber autocomplete ve IntelliSense

### 🔧 Faydalı Eklentiler

6. **JSON Language Features** - Microsoft
   - ID: `ms-vscode.vscode-json`
   - JSON dosyaları için destek

7. **PowerShell** - Microsoft
   - ID: `ms-vscode.powershell`
   - PowerShell desteği

## 📥 Kurulum Adımları

### Yöntem 1: VS Code Extensions Panel
1. VS Code'u açın
2. `Ctrl+Shift+X` ile Extensions panelini açın
3. Her eklentiyi arayın ve "Install" butonuna tıklayın

### Yöntem 2: Command Line
```bash
# C# Dev Kit
code --install-extension ms-dotnettools.csdevkit

# C#
code --install-extension ms-dotnettools.csharp

# .NET Runtime
code --install-extension ms-dotnettools.vscode-dotnet-runtime

# Cucumber Official
code --install-extension cucumberopen.cucumber-official

# Cucumber Autocomplete
code --install-extension alexkrechik.cucumberautocomplete

# JSON Support
code --install-extension ms-vscode.vscode-json

# PowerShell
code --install-extension ms-vscode.powershell
```

### Yöntem 3: Otomatik Kurulum
1. VS Code'u proje klasöründe açın
2. `.vscode/extensions.json` dosyasındaki önerileri kabul edin

## ⚙️ Ayarlar

Proje `.vscode/settings.json` dosyası ile önceden yapılandırılmıştır:

- **Cucumber Autocomplete**: Feature dosyaları ve step definitions otomatik tamamlama
- **OmniSharp**: C# IntelliSense ve refactoring
- **File Associations**: `.feature` dosyaları Gherkin olarak tanımlanır

## 🎯 Özellikler

Kurulum tamamlandıktan sonra:

### ✅ Cucumber/Gherkin Desteği
- Feature dosyalarında syntax highlighting
- Step tanımları için autocomplete
- F12 ile step definition'lara gitme
- Renklendirme ve IntelliSense

### ✅ C# Desteği
- IntelliSense ve autocomplete
- Refactoring araçları
- Debug desteği
- NuGet paket yönetimi

### ✅ SpecFlow Entegrasyonu
- Feature dosyalarında step tanımları otomatik tamamlanır
- Step definition'larda F12 ile navigasyon
- Renklendirme ve syntax highlighting

## 🔍 Sorun Giderme

### Eklenti Yüklenmiyor
- VS Code'u yeniden başlatın
- İnternet bağlantınızı kontrol edin
- Eklenti ID'sini doğru yazdığınızdan emin olun

### IntelliSense Çalışmıyor
- `Ctrl+Shift+P` → "OmniSharp: Restart OmniSharp"
- `.vscode/settings.json` dosyasını kontrol edin
- C# Dev Kit eklentisinin yüklü olduğundan emin olun

### Cucumber Autocomplete Çalışmıyor
- Feature dosyalarının `.feature` uzantısında olduğundan emin olun
- Step definition dosyalarının doğru klasörde olduğunu kontrol edin
- Cucumber eklentilerinin yüklü olduğundan emin olun

## 📚 Faydalı Kısayollar

- `Ctrl+Shift+X`: Extensions paneli
- `Ctrl+Shift+P`: Command palette
- `F12`: Go to definition
- `Ctrl+Space`: Trigger suggestions
- `Ctrl+Shift+O`: Go to symbol in file
- `Ctrl+T`: Go to symbol in workspace 