using Microsoft.Extensions.Configuration;
using Reqnroll;

namespace Piloteer
{
    [Binding]
    public class AppSettingsProvider : IAppSettingsProvider
    {
        private IConfigurationRoot? _configuration;
        private readonly IReqnrollOutputHelper _logger;
        private readonly IAppSettingsExtender _appSettingsExtender;

        public AppSettingsProvider(IReqnrollOutputHelper logger, IAppSettingsExtender appSettingsExtender)
        {
            _logger = logger;
            _appSettingsExtender = appSettingsExtender;
        }

        /// <summary>
        /// BeforeScenario hook that loads the app settings with order 2000
        /// </summary>
        [BeforeScenario(Order = 2000)]
        public void LoadAppSettings()
        {
            _logger.WriteLine($"Loading app settings file 'appsettings.json'.");

            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json");

            builder = _appSettingsExtender.Extend(builder);
            _configuration = builder.Build();
        }

        /// <inheritdoc/>
        /// <exception cref="TestExecutionException">Throws an exception if app settings aren't loaded or when the app setting isn't found.</exception>
        public string GetRequiredAppSettingsValue(string key)
        {
            if (_configuration == null)
                throw new TestExecutionException(Constants.ErrorCodes.AppSettingsNotLoaded);

            var value = _configuration[key];
            if (string.IsNullOrWhiteSpace(value))
                throw new TestExecutionException(Constants.ErrorCodes.AppSettingNotFound, key);

            return value;
        }

        /// <inheritdoc/>
        /// <exception cref="TestExecutionException">Throws an exception if app settings aren't loaded.</exception>
        public string? GetOptionalAppSettingsValue(string key)
        {
            if (_configuration == null)
                throw new TestExecutionException(Constants.ErrorCodes.AppSettingsNotLoaded);

            return _configuration[key];
        }

        /// <inheritdoc/>
        /// <exception cref="TestExecutionException">Throws an exception if app settings aren't loaded or when the app setting isn't found.</exception>
        public T[] GetRequiredAppSettingsArray<T>(string key)
        {
            if (_configuration == null)
                throw new TestExecutionException(Constants.ErrorCodes.AppSettingsNotLoaded);

            var section = _configuration.GetSection(key);
            if(!section.Exists())
                throw new TestExecutionException(Constants.ErrorCodes.AppSettingNotFound, key);

            return section.Get<T[]>() ?? [];
        }

        /// <inheritdoc/>
        /// <exception cref="TestExecutionException">Throws an exception if app settings aren't loaded.</exception>
        public T[] GetOptionalAppSettingsArray<T>(string key)
        {
            if (_configuration == null)
                throw new TestExecutionException(Constants.ErrorCodes.AppSettingsNotLoaded);

            var section = _configuration.GetSection(key);
            if (!section.Exists())
                return [];

            return section.Get<T[]>() ?? [];
        }
    }
}
