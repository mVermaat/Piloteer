namespace Piloteer.UnitTests.AppSettings
{
    [Scope(Feature = "AppSettingsProvider")]
    [Binding]
    public class AppSettingsProviderSteps
    {
        private readonly IAppSettingsProvider _provider;

        private TestExecutionException? _exception;
        private string? _appSettingsValue;

        public AppSettingsProviderSteps(IAppSettingsProvider provider)
        {
            _provider = provider;
        }

        [When(@"the required appsetting '(.*)' is loaded")]
        public void WhenRequiredAppSettingIsLoaded(string key)
        {
            try
            {
                _appSettingsValue = _provider.GetRequiredAppSettingsValue(key);
            }
            catch (TestExecutionException ex)
            {
                _exception = ex;
            }
        }

        [When(@"the optional appsetting '(.*)' is loaded")]
        public void WhenOptionalAppSettingIsLoaded(string key)
        {
            try
            {
                _appSettingsValue = _provider.GetOptionalAppSettingsValue(key);
            }
            catch (TestExecutionException ex)
            {
                _exception = ex;
            }
        }

        [Then(@"it has the value '(.*)'")]
        public void ThenTheAppSettingsShouldBeLoadedSuccessfully(string expectedValue)
        {
            Assert.Equal(expectedValue, _appSettingsValue);
        }

        [Then(@"it has NULL as value")]
        public void ThenTheAppSettingsShouldBeNull()
        {
            Assert.Null(_appSettingsValue);
        }

        [Then(@"a test execution exception with code (.*) was thrown")]
        public void ThenExceptionWasThrown(int code)
        {
            if (_exception == null)
            {
                Assert.Fail($"No exception was thrown while code {code} was expected");
            }

            Assert.Equal(code, _exception.ErrorCode);
        }
    }
}
