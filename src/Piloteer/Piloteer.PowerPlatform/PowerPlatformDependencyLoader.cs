using Piloteer.PowerPlatform.Commands;
using Piloteer.PowerPlatform.Connectivity;
using Piloteer.PowerPlatform.Metadata;
using Piloteer.PowerPlatform.Playwright.Actions;
using Reqnroll;
using Reqnroll.BoDi;

namespace Piloteer.PowerPlatform
{
    /// <summary>
    /// Class to load dependencies for the test framework specific to the Power Platform.
    /// </summary>
    [Binding]
    public class PowerPlatformDependencyLoader
    {
        private readonly IObjectContainer _objectContainer;

        public PowerPlatformDependencyLoader(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
        }

        /// <summary>
        /// BeforeScenario hook that loads dependencies with order 1000
        /// </summary>
        [BeforeScenario(Order = 1000)]
        public void LoadDependencies()
        {
            _objectContainer.RegisterTypeAs<PowerPlatformConnection, IPowerPlatformConnection>();
            _objectContainer.RegisterTypeAs<PowerPlatformCommandFactory, IPowerPlatformCommandFactory>();
            _objectContainer.RegisterTypeAs<AliasedRecordCache, IAliasedRecordCache>();
            _objectContainer.RegisterTypeAs<EntityParser, IEntityParser>();
            _objectContainer.RegisterTypeAs<MetadataCache, IMetadataCache>();
            _objectContainer.RegisterTypeAs<PowerPlatformActionFactory, IPowerPlatformActionFactory>();
            _objectContainer.RegisterTypeAs<PowerPlatformTestingContext, IPowerPlatformTestingContext>();
        }
    }
}
