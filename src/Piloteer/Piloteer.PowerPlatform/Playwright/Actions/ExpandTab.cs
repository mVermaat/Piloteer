using Microsoft.Playwright;
using Piloteer.Playwright;
using Piloteer.Playwright.Actions;

namespace Piloteer.PowerPlatform.Playwright.Actions
{
    public class ExpandTab : IPlaywrightAction
    {
        private readonly string _tabName;

        public ExpandTab(string tabName)
        {
            _tabName = tabName;
        }

        public async Task<ActionResult> ExecuteAsync(IPage page)
        {
            var locator = XPathProvider.GetLocator(page, PowerPlatformConstants.XPaths.EntityFormTab, _tabName);

            if (await IsTabExpanded(locator))
                return ActionResult.Success();

            await locator.ClickAsync();

            return ActionResult.Success();
        }

        private async Task<bool> IsTabExpanded(ILocator locator)
        {
            if(bool.TryParse(await locator.GetAttributeAsync("aria-selected"), out var isExpanded))
                return isExpanded;

            return false;
        }
    }
}
