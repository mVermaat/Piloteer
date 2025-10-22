using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.Xrm.Sdk.Metadata;
using Piloteer.Playwright.Actions;
using Piloteer.Playwright;
using Piloteer.PowerPlatform.Playwright.Fields;
using Microsoft.Xrm.Sdk;

namespace Piloteer.PowerPlatform.Playwright.Actions
{
    public class SetOptionSetColumn : IPlaywrightAction
    {
        private readonly FormControl _control;
        private readonly string? _optionSetLabel;

        public SetOptionSetColumn(FormControl control, string? optionSetLabel)
        {
            _control = control;
            _optionSetLabel = optionSetLabel;
        }

        public async Task<ActionResult> ExecuteAsync(IPage page)
        {
            var optionsetInput = XPathProvider.GetLocator(page, PowerPlatformConstants.XPaths.EntityColumnOptionSetInput, _control.ControlName);
            await optionsetInput.ClickAsync(new LocatorClickOptions { Timeout = 5000 });

            var itemContainer = XPathProvider.GetLocator(page, PowerPlatformConstants.XPaths.FluentItemContainer);
            var item = XPathProvider.GetLocator(itemContainer, PowerPlatformConstants.XPaths.EntityColumnOptionSetValueItem, _optionSetLabel);

            await item.ClickAsync(new LocatorClickOptions { Timeout = 2000 });

            return ActionResult.Success();
        }
    }
}
