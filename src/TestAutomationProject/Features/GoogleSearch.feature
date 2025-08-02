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