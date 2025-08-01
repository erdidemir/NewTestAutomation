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

  @Negative
  Scenario: Geçersiz Email ile Login
    When kullanıcı email adresini girer: "invalid@email.com"
    And kullanıcı email sonrası Next butonuna tıklar
    Then hata mesajı görünür olmalı
    And hata mesajı şu metni içermeli: "Couldn't find your Google Account"

  @Negative
  Scenario: Geçersiz Şifre ile Login
    When kullanıcı email adresini girer: "testuser@gmail.com"
    And kullanıcı email sonrası Next butonuna tıklar
    And kullanıcı şifresini girer: "wrongpassword"
    And kullanıcı şifre sonrası Next butonuna tıklar
    Then hata mesajı görünür olmalı
    And hata mesajı şu metni içermeli: "Wrong password"

  @Smoke
  Scenario: Test Başlatma
    When testi başlat
    Then login sayfası yüklenmiş olmalı

  @Complete
  Scenario: Tam Login Akışı
    When kullanıcı Google hesabına giriş yapar: "testuser@gmail.com" ve "testpassword123"
    Then Google logosu görünür olmalı 