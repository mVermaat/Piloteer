namespace Piloteer
{
    public static class Constants
    {
        internal class ErrorCodes
        {
            public const int AppSettingNotFound = 1;
            public const int AppSettingsNotLoaded = 2;
            public const int InvalidTestTarget = 3;
            public const int UnknownTarget = 4;
            public const int BrowserNotSupportedForApiTesting = 5;
            public const int UserProfileNotFound = 6;
            public const int UserProfileWithoutUsername = 7;
            public const int BrowserSessionNotInitialized = 8;
            public const int XPathNotFound = 9;
            public const int ActionWithoutResult = 10;
            public const int FailedToSetValue = 11;
        }

        public class XPaths
        {
            public const string MicrosoftLoginUsername = "MicrosoftLoginUsername";
            public const string MicrosoftLoginPassword = "MicrosoftLoginPassword";
            public const string MicrosoftLoginNext = "MicrosoftLoginNext";
            public const string MicrosoftLoginTOTPInput = "MicrosoftLoginTOTPInput";
            public const string MicrosoftLoginTOTPVerify = "MicrosoftLoginTOTPVerify";
        }
    }
}
