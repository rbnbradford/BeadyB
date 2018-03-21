@feature
#noinspection CucumberUndefinedStep
Feature: Allow calculators to add bigger numbers

  Scenario: Adding numbers correctly
    Given the number '10'
    And also the number '15'
    When I add those numbers
    Then I get '25'

  Scenario: Adding numbers incorrectly
    Given the number '20'
    And also the number '30'
    When I add those numbers
    Then I get '50'
