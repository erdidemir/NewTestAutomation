@GoogleSearch
Feature: Google Search Tests
  As a user I want to search on Google
  So that I can find the information I need

  Background:
    Given the user is ready for testing
    And the user is on Google homepage

  @Positive @Smoke
  Scenario: Successful Google Search
    When the user types "Selenium WebDriver" in the search box
    And the user clicks the search button
    Then search results should be visible
    And page title should contain "Selenium WebDriver" 