Feature: Tracking Site Login

Scenario: 01 - Verify loging of non-genuine users redirect back to Login page
	Given I log in as NonGenuine user
	When I navigate to Tracking page from main site
	Then Login page is displayed
	And 'Only Genuine users can review the tracking data' error message is displayed on Login page

Scenario: 02 - Verify loging of genuine user
	Given I am on login screen
	When I enter a credentials for Genuine user and submit
	Then I am logged in successfully
