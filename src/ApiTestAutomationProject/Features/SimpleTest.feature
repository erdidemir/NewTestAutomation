@Feature:SimpleTest
@Target:API
@need
Feature: Simple Test
    A simple test to verify Allure reporting

    @need:simple
    Scenario: Simple test scenario
        Given I have a simple test
        When I run the test
        Then the test should pass 