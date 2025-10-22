using Piloteer.Playwright;
using Piloteer.Playwright.Actions;
using Piloteer.PowerPlatform.Connectivity;
using Piloteer.PowerPlatform.Metadata;
using Piloteer.PowerPlatform.Playwright.Actions;
using Reqnroll;

namespace Piloteer.PowerPlatform
{
    public interface IPowerPlatformTestingContext
    {
        IActionProcessor ActionProcessor { get; }
        IAliasedRecordCache AliasedRecordCache { get; }
        IAppSettingsProvider AppSettingsProvider { get; }
        IBrowserSession BrowserSession { get; }
        IEntityParser EntityParser { get; }
        IMetadataCache MetadataCache { get; }
        IPowerPlatformConnection PowerPlatformConnection { get; }
        IReqnrollOutputHelper Logger { get; }

        Guid? CurrentModelAppId { get; set; }
    }
}