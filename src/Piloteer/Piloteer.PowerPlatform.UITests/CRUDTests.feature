Feature: CreateRecordCommand

Scenario: Test setting the different datatypes
	Given a logged in Salesperson
	And the model driven app msdynce_saleshub
	When an account named MyAccount is created with the following values
		| Property     | Value       |
		| Account Name | TestAccount |
		| Industry     | Accounting  |
		| Credit Limit | 150.34      |
		| Description  | MultiLine   |
		| Credit Hold  | No          |
	And a contact named MyContact is created with the following values
		| Property     | Value     |
		| First Name   | Test      |
		| Last Name    | Tester    |
		| Company Name | MyAccount |
		| Birthday     | 31-1-1995 |
	And an appointment named MyAppointment is created with the following values
		| Property   | Value          |
		| Subject    | Test           |
		| Start Time | 1-1-2025 9:45  |
		| End Time   | 1-1-2025 12:45 |
		| Regarding  | MyAccount      |	


#
#Scenario: Test that you can create an account in Dataverse
#	Given a logged in Salesperson
#	And the model driven app msdynce_saleshub
#	When an account named MyAccount is created with the following values
#		| Property                       | Value            |
#		| Account Name                   | TestAccount      |
#		| Industry                       | Accounting       |
#		| Credit Limit                   | 150.34           |
#		| Last On Hold Time              | 21-10-2023 12:00 |
#		| Last Date Included in Campaign | 31-12-2024       |
#		| Price List                     |                  |
#		| Description                    | MultiLine        |
#		| Number of Employees            | 100              |
#		| Credit Hold                    | No               |
#	Then an account exists with the following values
#		| Property                       | Value            |
#		| Account Name                   | TestAccount      |
#		| Industry                       | Accounting       |
#		| Credit Limit                   | 150.34           |
#		| Last On Hold Time              | 21-10-2023 12:00 |
#		| Last Date Included in Campaign | 31-12-2024       |
#		| Price List                     |                  |
#		| Description                    | MultiLine        |
#		| Number of Employees            | 100              |
#		| Credit Hold                    | No               |
#
#Scenario: Test that you can update an account in Dataverse
#	Given a logged in Salesperson
#	And an account named MyAccount with the following values
#		| Property                       | Value            |
#		| Account Name                   | TestAccount      |
#		| Industry                       | Accounting       |
#		| Credit Limit                   | 150.34           |
#		| Last On Hold Time              | 21-10-2023 12:00 |
#		| Last Date Included in Campaign | 31-12-2024       |
#		| Price List                     |                  |
#		| Description                    | MultiLine        |
#		| Number of Employees            | 100              |
#		| Credit Hold                    | No               |
#	When MyAccount is updated with the following values
#		| Property     | Value        |
#		| Account Name | TestAccount2 |
#	Then an account exists with the following values
#		| Property                       | Value            |
#		| Account Name                   | TestAccount2     |
#		| Industry                       | Accounting       |
#		| Credit Limit                   | 150.34           |
#		| Last On Hold Time              | 21-10-2023 12:00 |
#		| Last Date Included in Campaign | 31-12-2024       |
#		| Price List                     |                  |
#		| Description                    | MultiLine        |
#		| Number of Employees            | 100              |
#		| Credit Hold                    | No               |