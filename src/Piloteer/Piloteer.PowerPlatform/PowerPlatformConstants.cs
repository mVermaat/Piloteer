namespace Piloteer.PowerPlatform
{
    public static class PowerPlatformConstants
    {
        internal class ErrorCodes
        {
            public const int MissingTranslation = 10000;
            public const int OptionSetValueUnknown = 10001;
            public const int AliasDoesntExist = 10002;
            public const int DisplayNameNotFound = 10003;
            public const int DisplayNameFoundMultipleTimes = 10004;
            public const int FailedToConnectToDataverse = 10005;
            public const int InvalidDecimalNumber = 10006;
            public const int InvalidWholeNumber = 10007;
            public const int InvalidGuid = 10008;
            public const int InvalidDateTime = 10009;
            public const int FormIdNotFound = 10010;
            public const int FormParsingFailed = 10011;
            public const int ColumnNotOnForm = 10012;
            public const int ColumnNotVisible = 10013;
            public const int ColumnLocked = 10014;
            public const int ModelAppNotFound = 10015;
            public const int ModelAppNotSet = 10016;
            public const int SaveTimeoutReached = 10017;
            public const int DuplicateDetectionRejected = 10018;
            public const int DateTimeFieldNotSet = 10019;
            public const int FormSaveFailed = 10020;
            public const int RecordIdNotFound = 10021;
            public const int NoControlName = 10022;
            public const int NoTabName = 10023;

        }
        
        public class BrowserSettings
        {
            public const string CopilotPaneChecksDone = "CopilotPaneChecksDone";
            public const string IsCopilotDisabled = "IsCopilotDisabled";
            public const string IsFormAssistDisabled = "IsFormAssistDisabled";
        }

        internal class UITexts
        {
            public const string ButtonTextSave = "Save";

            public const string SaveStatusSaved = "Saved";
            public const string SaveStatusSaving = "Saving";
            public const string SaveStatusUnsaved = "Unsaved";
            
        }

        public class XPaths
        {
            public const string CommandBarOverflowMenu = "CommandBarOverflowMenu";
            public const string CopilotPane = "CopilotPane";
            public const string CopilotPaneCloseButton = "CopilotPaneCloseButton";
            public const string DuplicateDetectionGridRows = "DuplicateDetectionGridRows";
            public const string DuplicateDetectionSaveButton = "DuplicateDetectionSaveButton";
            public const string DuplicateDetectionWindow = "DuplicateDetectionWindow";
            public const string EntityColumnDateTimeDateInput = "EntityColumnDateTimeDateInput";
            public const string EntityColumnDateTimeTimeInput = "EntityColumnDateTimeTimeInput";
            public const string EntityColumnLockedIcon = "EntityColumnLockedIcon";
            public const string EntityColumnOptionSetInput = "EntityColumnOptionSetInput";
            public const string EntityColumnOptionSetValueItem = "EntityColumnOptionSetValueItem";
            public const string EntityColumnTextArea = "EntityColumnTextArea";
            public const string EntityColumnTextInput = "EntityColumnTextInput";
            public const string EntityFieldContainer = "EntityFieldContainer";
            public const string EntityFormAssistHideButton = "EntityFormAssistHideButton";
            public const string EntityFormAssistShowButton = "EntityFormAssistShowButton";
            public const string EntityFormCommandBar = "EntityFormCommandBar";
            public const string EntityFormCommandBarButton = "EntityFormCommandBarButton";
            public const string EntityFormCommandBarOverflowButton = "EntityFormCommandBarOverflowButton";
            public const string EntityFormCompositeControls = "EntityFormCompositeControls";
            public const string EntityFormId = "EntityFormId";
            public const string EntityFormRoot = "EntityFormRoot";
            public const string EntityFormSaveStatus = "EntityFormSaveStatus";
            public const string EntityFormTab = "EntityFormTab";
            public const string EntityQuickCreateDialogRoot = "EntityQuickCreateDialogRoot";
            public const string ErrorDialogRoot = "ErrorDialogRoot";
            public const string ErrorDialogText = "ErrorDialogText";
            public const string FluentItemContainer = "EntityColumnOptionSetValueContainer";
        }
    }
}
