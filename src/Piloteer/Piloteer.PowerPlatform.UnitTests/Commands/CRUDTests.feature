Feature: CreateRecordCommand

Implict testing of parsing Tables into Dataverse objects

Scenario: Test that you can create an account in Dataverse
	When an account named MyAccount is created with the following values
		| Property                       | Value            |
		| Account Name                   | TestAccount      |
		| Category                       | Standard         |
		| Credit Limit                   | 150.34           |
		| Last On Hold Time              | 21-10-2023 12:00 |
		| Last Date Included in Campaign | 31-12-2024       |
		| Price List                     |                  |
		| Description                    | MultiLine        |
		| Number of Employees            | 100              |
		| Credit Hold                    | No               |
	Then an account exists with the following values
		| Property                       | Value            |
		| Account Name                   | TestAccount      |
		| Category                       | Standard         |
		| Credit Limit                   | 150.34           |
		| Last On Hold Time              | 21-10-2023 12:00 |
		| Last Date Included in Campaign | 31-12-2024       |
		| Price List                     |                  |
		| Description                    | MultiLine        |
		| Number of Employees            | 100              |
		| Credit Hold                    | No               |

Scenario: Test that you can update an account in Dataverse
	Given an account named MyAccount with the following values
		| Property                       | Value            |
		| Account Name                   | TestAccount      |
		| Category                       | Standard         |
		| Credit Limit                   | 150.34           |
		| Last On Hold Time              | 21-10-2023 12:00 |
		| Last Date Included in Campaign | 31-12-2024       |
		| Price List                     |                  |
		| Description                    | MultiLine        |
		| Number of Employees            | 100              |
		| Credit Hold                    | No               |
	When MyAccount is updated with the following values
		| Property     | Value        |
		| Account Name | TestAccount2 |
	Then an account exists with the following values
		| Property                       | Value            |
		| Account Name                   | TestAccount2     |
		| Category                       | Standard         |
		| Credit Limit                   | 150.34           |
		| Last On Hold Time              | 21-10-2023 12:00 |
		| Last Date Included in Campaign | 31-12-2024       |
		| Price List                     |                  |
		| Description                    | MultiLine        |
		| Number of Employees            | 100              |
		| Credit Hold                    | No               |