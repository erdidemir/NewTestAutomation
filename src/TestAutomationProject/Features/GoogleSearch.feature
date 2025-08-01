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

  # DENEME AMAÇLI - SADECE 1 TEST AKTİF
  # @Positive @Smoke
  # Scenario: Tek Adımda Arama
  #   When kullanıcı Google'da "Test Otomasyonu" arar
  #   Then arama sonuçları görünür olmalı
  #   And sayfa başlığı "Test Otomasyonu" içermeli

  # @Positive
  # Scenario: Arama Kutusu Kontrolü
  #   Then arama kutusu görünür olmalı
  #   And Google logosu görünür olmalı

  # @Positive
  # Scenario: Şanslı Ol Butonu
  #   When kullanıcı arama kutusuna "Microsoft" yazar
  #   And kullanıcı şanslı ol butonuna tıklar
  #   Then sayfa başlığı "Microsoft" içermeli

  # @Smoke
  # Scenario: Test Başlatma
  #   When testi başlat
  #   Then Google ana sayfası yüklenmiş olmalı

  # @Complete
  # Scenario: Tam Arama Akışı
  #   When kullanıcı arama kutusuna "C# Programming" yazar
  #   And kullanıcı arama butonuna tıklar
  #   Then arama sonuçları görünür olmalı
  #   And arama kutusunda "C# Programming" yazmalı
  #   And sayfa başlığı "C# Programming" içermeli 