@feature
#noinspection CucumberUndefinedStep
Feature: Allow calculators to add numbers

  Scenario: Adding numbers correctly
    Given the number '1'
    And also the number '2'
    When I add those numbers
    Then I get '3'

  Scenario: Adding numbers incorrectly
    Given the number '2'
    And also the number '3'
    When I add those numbers
    Then I get '5'
