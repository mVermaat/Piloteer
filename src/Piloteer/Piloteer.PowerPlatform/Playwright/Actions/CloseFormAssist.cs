using Microsoft.Playwright;
using Piloteer.Playwright;
using Piloteer.Playwright.Actions;

namespace Piloteer.PowerPlatform.Playwright.Actions
{
    public class CloseFormAssist : IPlaywrightAction
    {
        private readonly IPowerPlatformTestingContext _context;

        public CloseFormAssist(IPowerPlatformTestingContext context)
        {
            _context = context;
        }

        public async Task<ActionResult> ExecuteAsync(IPage page)
        {
            if (_context.BrowserSession.GetSetting<bool>(PowerPlatformConstants.BrowserSettings.IsFormAssistDisabled))
                return ActionResult.Success();

            try
            {
                var hideLocator = XPathProvider.GetLocator(page, PowerPlatformConstants.XPaths.EntityFormAssistHideButton);
                await hideLocator.ClickAsync(new LocatorClickOptions() { Timeout = 5000 });

                _context.BrowserSession.SetSetting(PowerPlatformConstants.BrowserSettings.IsFormAssistDisabled, true);

                return ActionResult.Success();
            }
            catch(TimeoutException)
            {
                // Already closed
                _context.BrowserSession.SetSetting(PowerPlatformConstants.BrowserSettings.IsFormAssistDisabled, true);
                return ActionResult.Success();
            }
        }
    }
}
