using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.Xrm.Sdk.Metadata;
using Piloteer.Playwright;
using Piloteer.Playwright.Actions;
using Piloteer.PowerPlatform.Playwright.Fields;

namespace Piloteer.PowerPlatform.Playwright.Actions
{
    public class SetStringColumn : IPlaywrightAction
    {
        private readonly FormControl _control;
        private readonly string? _value;

        public SetStringColumn(FormControl control, string? value)
        {
            _control = control;
            _value = value;
        }

        public async Task<ActionResult> ExecuteAsync(IPage page)
        {
            var root = XPathProvider.GetLocator(page, PowerPlatformConstants.XPaths.EntityFormRoot);
            var container = XPathProvider.GetLocator(root, PowerPlatformConstants.XPaths.EntityFieldContainer, _control.ControlName);

            var inputXPathKey = _control.Metadata.AttributeType == AttributeTypeCode.Memo ? PowerPlatformConstants.XPaths.EntityColumnTextArea : PowerPlatformConstants.XPaths.EntityColumnTextInput;
            var input = XPathProvider.GetLocator(container, inputXPathKey);

            await input.ClearAsync();
            if(!string.IsNullOrEmpty(_value))
                await input.FillAsync(_value);

            await page.Keyboard.PressAsync("Tab");
            await page.Keyboard.PressAsync("Tab");

            return ActionResult.Success();
        }
    }
}
