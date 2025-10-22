using Microsoft.Playwright;
using Piloteer.Playwright;
using Piloteer.Playwright.Actions;

namespace Piloteer.PowerPlatform.Playwright.Actions
{
    public class ClickRibbonButton : IPlaywrightAction
    {
        private readonly string _entityName;
        private readonly string _buttonName;

        public ClickRibbonButton(string entityName, string buttonName)
        {
            _entityName = entityName;
            _buttonName = buttonName;
        }

        public async Task<ActionResult> ExecuteAsync(IPage page)
        {
            var commandBar = XPathProvider.GetLocator(page, PowerPlatformConstants.XPaths.EntityFormCommandBar, _entityName);
            var button = XPathProvider.GetLocator(commandBar, PowerPlatformConstants.XPaths.EntityFormCommandBarButton, _buttonName);

            if (await button.IsVisibleAsync())
            {
                await button.ClickAsync();
                return ActionResult.Success();
            }
            else
            {
                var overflowButton = XPathProvider.GetLocator(commandBar, PowerPlatformConstants.XPaths.EntityFormCommandBarOverflowButton, _entityName);
                await overflowButton.ClickAsync();

                var overflowMenu = XPathProvider.GetLocator(page, PowerPlatformConstants.XPaths.CommandBarOverflowMenu);
                button = XPathProvider.GetLocator(overflowMenu, PowerPlatformConstants.XPaths.EntityFormCommandBarButton, _buttonName);
                await button.ClickAsync();
                return ActionResult.Success();
            }


        }
    }
}
