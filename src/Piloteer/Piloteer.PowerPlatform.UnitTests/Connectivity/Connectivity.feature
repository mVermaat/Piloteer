Feature: Connectivity

This tests if you can connect to Dataverse

Scenario: When connecting to the Dataverse API, it will log in with the app registration
	When I connect to dataverse
	Then I should be logged in with the app registration from the app config

Scenario: When connecting to the Dataverse API and change the user, it will log in with the salesperson user
	Given a logged in Salesperson
	When I connect to dataverse
	And a account named MyAccount is created with the following values
		| Property     | Value     |
		| Account Name | MyAccount |
	Then I should be logged in with userId '451a54fb-1779-f011-b4cc-6045bde08005'