using Microsoft.Extensions.Configuration;
using Reqnroll.BoDi;

namespace Piloteer.UnitTests
{
    [Binding]
    public class AppSettingsExtender : IAppSettingsExtender
    {
        private readonly IObjectContainer _container;

        public AppSettingsExtender(IObjectContainer container)
        {
            _container = container;
        }

        public IConfigurationBuilder Extend(IConfigurationBuilder builder)
        {
            return builder.AddUserSecrets<AppSettingsExtender>();
        }

        [BeforeScenario(Order = 1000)]
        public void RegisterAppSettingsExtender()
        {
            _container.RegisterTypeAs<AppSettingsExtender, IAppSettingsExtender>();
        }
    }
}
