using Piloteer.Playwright;
using Piloteer.Playwright.Actions;
using Piloteer.PowerPlatform.Connectivity;
using Piloteer.PowerPlatform.Metadata;
using Piloteer.PowerPlatform.Playwright.Actions;
using Reqnroll;

namespace Piloteer.PowerPlatform
{
    public class PowerPlatformTestingContext : IPowerPlatformTestingContext
    {
        public IAppSettingsProvider AppSettingsProvider { get; }
        public IPowerPlatformConnection PowerPlatformConnection { get; }
        public IEntityParser EntityParser { get; }
        public IMetadataCache MetadataCache { get; }
        public IAliasedRecordCache AliasedRecordCache { get; }
        public IBrowserSession BrowserSession { get; }
        public IActionProcessor ActionProcessor { get; }
        public IReqnrollOutputHelper Logger { get; }

        public Guid? CurrentModelAppId { get; set; }

        public PowerPlatformTestingContext(
            IAppSettingsProvider appSettingsProvider,
            IPowerPlatformConnection powerPlatformConnection,
            IEntityParser entityParser,
            IMetadataCache metadataCache,
            IAliasedRecordCache aliasedRecordCache,
            IBrowserSession browserSession,
            IActionProcessor actionProcessor,
            IReqnrollOutputHelper logger)
        {
            AppSettingsProvider = appSettingsProvider ?? throw new ArgumentNullException(nameof(appSettingsProvider));
            PowerPlatformConnection = powerPlatformConnection ?? throw new ArgumentNullException(nameof(powerPlatformConnection));
            EntityParser = entityParser ?? throw new ArgumentNullException(nameof(entityParser));
            MetadataCache = metadataCache ?? throw new ArgumentNullException(nameof(metadataCache));
            AliasedRecordCache = aliasedRecordCache ?? throw new ArgumentNullException(nameof(aliasedRecordCache));
            BrowserSession = browserSession ?? throw new ArgumentNullException(nameof(browserSession));
            ActionProcessor = actionProcessor ?? throw new ArgumentNullException(nameof(actionProcessor));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
    }
}
