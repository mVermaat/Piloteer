using Reqnroll;

namespace Piloteer.PowerPlatform
{
    [Binding]
    public class PowerPlatformErrors
    {
        [BeforeTestRun(Order = 1000)]
        public static void AddErrors()
        {
            ErrorCodes.AddError(PowerPlatformConstants.ErrorCodes.MissingTranslation, "Missing translation for label: {0}.");
            ErrorCodes.AddError(PowerPlatformConstants.ErrorCodes.OptionSetValueUnknown, "Optionset {1} doesn't have value {0}. Available options: {2}");
            ErrorCodes.AddError(PowerPlatformConstants.ErrorCodes.AliasDoesntExist, "alias '{0}' doesn't exist.");
            ErrorCodes.AddError(PowerPlatformConstants.ErrorCodes.DisplayNameNotFound, "Attribute {0} not found in metadata for entity {1}.");
            ErrorCodes.AddError(PowerPlatformConstants.ErrorCodes.DisplayNameFoundMultipleTimes, "Attribute {0} found multiple times in metadata for entity {1}. Hits: {2}.");
            ErrorCodes.AddError(PowerPlatformConstants.ErrorCodes.FailedToConnectToDataverse, "Failed to connect to Dataverse API. Error: {0}.");
            ErrorCodes.AddError(PowerPlatformConstants.ErrorCodes.InvalidDecimalNumber, "Number '{0}' should be a decimal number in the invariant format (decimal seperator is .).");
            ErrorCodes.AddError(PowerPlatformConstants.ErrorCodes.InvalidWholeNumber, "Number '{0}' should be a whole number in the invariant format.");
            ErrorCodes.AddError(PowerPlatformConstants.ErrorCodes.InvalidDateTime, "DateTime '{0}' has the wrong format. Format used {1} with invariant culture.");
            ErrorCodes.AddError(PowerPlatformConstants.ErrorCodes.FormParsingFailed, "FormXml parsing failed.");
            ErrorCodes.AddError(PowerPlatformConstants.ErrorCodes.ColumnNotOnForm, "Column {0} is not present on the form.");
            ErrorCodes.AddError(PowerPlatformConstants.ErrorCodes.ColumnNotVisible, "Column {0} is not visible on the form.");
            ErrorCodes.AddError(PowerPlatformConstants.ErrorCodes.ColumnLocked, "Column {0} is read-only.");
            ErrorCodes.AddError(PowerPlatformConstants.ErrorCodes.ModelAppNotFound, "Model Driven Application {0} doesn't exist");
            ErrorCodes.AddError(PowerPlatformConstants.ErrorCodes.ModelAppNotSet, "Model Driven Application isn't set yet.");
            ErrorCodes.AddError(PowerPlatformConstants.ErrorCodes.SaveTimeoutReached, "Save timeout reached");
            ErrorCodes.AddError(PowerPlatformConstants.ErrorCodes.DuplicateDetectionRejected, "Duplicate detection rejected");
            ErrorCodes.AddError(PowerPlatformConstants.ErrorCodes.DateTimeFieldNotSet, "Failed to set datetime value for attribute {0} to {1}. Current value: {2}");
            ErrorCodes.AddError(PowerPlatformConstants.ErrorCodes.FormSaveFailed, "Saving record failed: {0}");
            ErrorCodes.AddError(PowerPlatformConstants.ErrorCodes.RecordIdNotFound, "Record ID not found");
            ErrorCodes.AddError(PowerPlatformConstants.ErrorCodes.NoControlName, "Control for attribute {0} is missing a name on tab {1}");
            ErrorCodes.AddError(PowerPlatformConstants.ErrorCodes.NoTabName, "Tab for entity {0} is missing a name");
        }
    }
}
