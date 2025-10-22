using Microsoft.Playwright;
using Microsoft.Xrm.Sdk;
using Newtonsoft.Json.Linq;
using Piloteer.Playwright.Actions;
using Piloteer.PowerPlatform.Playwright.Fields;

namespace Piloteer.PowerPlatform.Playwright.Actions
{
    public class SetLookupColumn : IPlaywrightAction
    {

        private readonly FormControl _control;
        private readonly EntityReference? _value;

        public SetLookupColumn(FormControl control, EntityReference? value)
        {
            _control = control;
            _value = value;
        }

        public async Task<ActionResult> ExecuteAsync(IPage page)
        {
            if (_value != null)
            {
                await page.EvaluateAsync($"Xrm.Page.getAttribute('{_control.AttributeName}').setValue([ {{ id: '{_value.Id}', name: '{_value.Name?.Replace("'", @"\'")}', entityType: '{_value.LogicalName}' }} ])");
                await page.EvaluateAsync($"Xrm.Page.getAttribute('{_control.AttributeName}').fireOnChange()");
                return ActionResult.Success();
            }
            else
            {
                await page.EvaluateAsync($"Xrm.Page.getAttribute('{_control.AttributeName}').setValue(null)");
                await page.EvaluateAsync($"Xrm.Page.getAttribute('{_control.AttributeName}').fireOnChange()");
                return ActionResult.Success();
            }
        }
    }
}
