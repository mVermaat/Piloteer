namespace Piloteer
{
    /// <summary>
    /// Class to hold error codes and their messages which is globally available. To be used with TestExecutionException.
    /// </summary>
    public static class ErrorCodes
    {
        private static readonly Dictionary<int, string> _errorMessages;

        static ErrorCodes()
        {
            _errorMessages = new Dictionary<int, string>();
            FillDictionary();
        }

        private static void FillDictionary()
        {
            AddError(Constants.ErrorCodes.AppSettingNotFound, "Required app setting '{0}' not found");
            AddError(Constants.ErrorCodes.AppSettingsNotLoaded, "Configuration has not been loaded. Make sure when you read appsettings, it's after BeforeScenario Order 2000");
            AddError(Constants.ErrorCodes.InvalidTestTarget, "Test target not specified. Use 'API' or 'Chrome' tag.");
            AddError(Constants.ErrorCodes.UnknownTarget, "Test target {0} unknown. Please contact the package developers.");
            AddError(Constants.ErrorCodes.BrowserNotSupportedForApiTesting, "Command {0} only supports execution via the browser while you are running API tests.");
            AddError(Constants.ErrorCodes.UserProfileNotFound, "User profile {0} doesn't exist in the appsettings.json");
            AddError(Constants.ErrorCodes.UserProfileWithoutUsername, "User profile {0} exists, but it doesn't contain a username");
            AddError(Constants.ErrorCodes.BrowserSessionNotInitialized, "Browser session has not been initialized. Make sure you login with a user profile before using the browser.");
            AddError(Constants.ErrorCodes.XPathNotFound, "XPath with key '{0}' not found");
            AddError(Constants.ErrorCodes.ActionWithoutResult, "Action {0} returned without a result, while a result was expected.");
            AddError(Constants.ErrorCodes.FailedToSetValue, "Failed to set {0}");
        }

        /// <summary>
        /// Adds an error message to be used later.
        /// </summary>
        /// <param name="errorCode">Error number</param>
        /// <param name="message">Error message belonging to this error code</param>
        public static void AddError(int errorCode, string message)
        {
            _errorMessages[errorCode] = message;
        }

        /// <summary>
        /// Gets a message based on its error code.
        /// </summary>
        /// <param name="errorCode">Error number</param>
        /// <param name="formatArgs">Any formatted values like string.Format</param>
        /// <returns>The formatted error message</returns>
        public static string GetErrorMessage(int errorCode, params object[] formatArgs)
        {
            return GetErrorMessage(errorCode, null, formatArgs);
        }

        /// <summary>
        /// Gets a message based on its error code with additional details.
        /// </summary>
        /// <param name="errorCode">Error number</param>
        /// <param name="additionalDetails">Any formatted values like string.Format</param>
        /// <param name="formatArgs">Any formatted values like string.Format</param>
        /// <returns>The formatted error message</returns>
        public static string GetErrorMessage(int errorCode, string? additionalDetails, params object[] formatArgs)
        {
            if (!_errorMessages.ContainsKey(errorCode))
                return $"Error code {errorCode} doesn't exist";

            return string.Format($"[{errorCode}] {_errorMessages[errorCode]}{additionalDetails}", formatArgs);
        }
    }
}
