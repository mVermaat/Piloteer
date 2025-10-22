using Microsoft.Playwright;
using Piloteer.Playwright;
using Piloteer.Playwright.Actions;

namespace Piloteer.PowerPlatform.Playwright.Actions
{
    public class CloseCopilotPane : IPlaywrightAction
    {
        private readonly IPowerPlatformTestingContext _context;

        public CloseCopilotPane(IPowerPlatformTestingContext context)
        {
            _context = context;
        }


        public async Task<ActionResult> ExecuteAsync(IPage page)
        {
            if (_context.BrowserSession.GetSetting<bool>(PowerPlatformConstants.BrowserSettings.IsCopilotDisabled))
                return ActionResult.Success();

            var checkCount = _context.BrowserSession.GetSetting<int>(PowerPlatformConstants.BrowserSettings.CopilotPaneChecksDone); 
            var copilotPane = XPathProvider.GetLocator(page, PowerPlatformConstants.XPaths.CopilotPane);
            var closeButton = XPathProvider.GetLocator(copilotPane, PowerPlatformConstants.XPaths.CopilotPaneCloseButton);

            if (checkCount > 3)
            {
                if (await closeButton.IsVisibleAsync())
                {
                    await closeButton.ClickAsync();
                    _context.BrowserSession.SetSetting(PowerPlatformConstants.BrowserSettings.IsCopilotDisabled, true);
                }

                return ActionResult.Success();
            }
            else
            {
                try
                {
                    await closeButton.ClickAsync(new LocatorClickOptions() { Timeout = 15000 });
                    _context.BrowserSession.SetSetting(PowerPlatformConstants.BrowserSettings.IsCopilotDisabled, true);
                }
                catch(TimeoutException) { }

                _context.BrowserSession.SetSetting(PowerPlatformConstants.BrowserSettings.CopilotPaneChecksDone, checkCount + 1);
                return ActionResult.Success();
            }
        }
    }
}
