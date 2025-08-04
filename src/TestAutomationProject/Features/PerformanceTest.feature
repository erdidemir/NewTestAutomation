@Performance @Monitoring
Feature: Performans Monitoring Testleri
  Test otomasyonu performansını ölçmek ve izlemek için
  Böylece test sürelerini optimize edebilirim

  Background:
    Given kullanıcı performans testi için hazır
    And performans monitoring aktif

  @BrowserPerformance @Smoke
  Scenario: Browser Başlatma Performansı
    Given kullanıcı browser başlatma performansını test etmek istiyor
    When browser başlatılıyor
    Then browser başlatma süresi 10 saniyeden az olmalı
    And memory kullanımı 500MB'den az olmalı

  @PageLoadPerformance @Smoke
  Scenario: Sayfa Yükleme Performansı
    Given kullanıcı sayfa yükleme performansını test etmek istiyor
    When Google ana sayfası açılıyor
    Then sayfa yükleme süresi 5 saniyeden az olmalı
    And sayfa tamamen yüklenmiş olmalı

  @ElementInteractionPerformance @Smoke
  Scenario: Element Etkileşim Performansı
    Given kullanıcı element etkileşim performansını test etmek istiyor
    And kullanıcı Google ana sayfasında
    When arama kutusu bulunuyor
    And arama kutusuna metin yazılıyor
    Then element bulma süresi 3 saniyeden az olmalı
    And element etkileşim süresi 2 saniyeden az olmalı

  @MemoryUsage @Monitoring
  Scenario: Memory Kullanım Kontrolü
    Given kullanıcı memory kullanımını kontrol etmek istiyor
    When test senaryosu çalıştırılıyor
    Then memory kullanımı 1GB'den az olmalı
    And memory sızıntısı olmamalı

  @SlowOperationDetection @Monitoring
  Scenario: Yavaş İşlem Tespiti
    Given kullanıcı yavaş işlemleri tespit etmek istiyor
    When yavaş bir işlem gerçekleştiriliyor
    Then yavaş işlem loglanmalı
    And performans raporunda görünmeli

  @DriverUpdate @Maintenance
  Scenario: Driver Güncelleme Kontrolü
    Given kullanıcı driver güncellemelerini kontrol etmek istiyor
    When driver uyumluluk kontrolü yapılıyor
    Then driver güncel olmalı
    And driver otomatik güncellenmeli

  @PerformanceReport @Reporting
  Scenario: Performans Raporu Oluşturma
    Given kullanıcı performans raporu oluşturmak istiyor
    When test senaryoları tamamlanıyor
    Then performans raporu oluşturulmalı
    And rapor JSON formatında olmalı
    And rapor Results klasöründe saklanmalı 