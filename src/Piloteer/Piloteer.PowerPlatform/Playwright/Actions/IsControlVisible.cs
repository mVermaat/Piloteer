using Microsoft.Playwright;
using Piloteer.Playwright;
using Piloteer.Playwright.Actions;
using Piloteer.PowerPlatform.Playwright.Fields;

namespace Piloteer.PowerPlatform.Playwright.Actions
{
    public class IsControlVisible : IPlaywrightAction<bool>
    {
        private readonly FormControl _control;

        public IsControlVisible(FormControl control)
        {
            _control = control;
        }
        
        public async Task<ActionResult<bool>> ExecuteAsync(IPage page)
        {
            var locator = XPathProvider.GetLocator(page, PowerPlatformConstants.XPaths.EntityFieldContainer, _control.ControlName);

            try
            {
                await Assertions.Expect(locator).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions() { Timeout = 2000, Visible = true });
                return ActionResult<bool>.Success(true);
            }
            catch(PlaywrightException)
            {
                return ActionResult<bool>.Success(false);
            }
        }
    }
}
