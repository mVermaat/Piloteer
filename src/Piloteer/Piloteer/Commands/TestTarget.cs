namespace Piloteer.Commands
{
    internal static class TestTargetLoader
    {
        public static TestTarget Get(IAppSettingsProvider appSettingsProvider)
        {
            var target = appSettingsProvider.GetRequiredAppSettingsValue("General:Target");
            if (string.IsNullOrWhiteSpace(target) || !Enum.TryParse<TestTarget>(target, true, out var parsedTarget))
            {
                throw new TestExecutionException(Constants.ErrorCodes.InvalidTestTarget);
            }
            return parsedTarget;
        }
    }

    internal enum TestTarget
    {
        API, Chrome
    }
}
