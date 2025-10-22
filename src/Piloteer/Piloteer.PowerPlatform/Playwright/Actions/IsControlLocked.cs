using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Piloteer.Playwright;
using Piloteer.Playwright.Actions;
using Piloteer.PowerPlatform.Playwright.Fields;

namespace Piloteer.PowerPlatform.Playwright.Actions
{
    public class IsControlLocked : IPlaywrightAction<bool>
    {
        private readonly FormControl _control;

        public IsControlLocked(FormControl control)
        {
            _control = control;
        }

        public async Task<ActionResult<bool>> ExecuteAsync(IPage page)
        {
            var container = XPathProvider.GetLocator(page, PowerPlatformConstants.XPaths.EntityFieldContainer, _control.ControlName);
            var isLocked = XPathProvider.GetLocator(container, PowerPlatformConstants.XPaths.EntityColumnLockedIcon, _control.ControlName);

            return ActionResult<bool>.Success(await isLocked.IsVisibleAsync());

        }
    }
}
