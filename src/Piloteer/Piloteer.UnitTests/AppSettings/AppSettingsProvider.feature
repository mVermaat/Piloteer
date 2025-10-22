Feature: AppSettingsProvider

This features tests retrieving appsettings.

Scenario Outline: Getting required app settings from either the root and section should return the value available in the appsettings.json file
	When the required appsetting '<SettingName>' is loaded
	Then it has the value '<ExpectedValue>'

Examples:
	| SettingName             | ExpectedValue  |
	| Test                    | TestValue1     |
	| TestSection:TestKey     | TestValue2     |
	| MySecret                | MySecretValue1 |
	| TestSection:OtherSecret | MySecretValue2 |

Scenario Outline: Getting optional app settings from either the root and section should return the value available in the appsettings.json file
	When the required appsetting '<SettingName>' is loaded
	Then it has the value '<ExpectedValue>'

Examples:
	| SettingName             | ExpectedValue  |
	| Test                    | TestValue1     |
	| TestSection:TestKey     | TestValue2     |
	| MySecret                | MySecretValue1 |
	| TestSection:OtherSecret | MySecretValue2 |

Scenario: Getting non-existing app setting that is required throws an exception with code 1
	When the required appsetting 'NonExistingSetting' is loaded
	Then a test execution exception with code 1 was thrown


Scenario: Getting non-existing app setting that is optional returns null as value
	When the optional appsetting 'NonExistingSetting' is loaded
	Then it has NULL as value
