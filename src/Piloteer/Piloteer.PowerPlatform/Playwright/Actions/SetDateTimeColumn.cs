using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure;
using Microsoft.Playwright;
using Piloteer.Playwright;
using Piloteer.Playwright.Actions;
using Piloteer.PowerPlatform.Playwright.Fields;

namespace Piloteer.PowerPlatform.Playwright.Actions
{
    public class SetDateTimeColumn : IPlaywrightAction
    {
        private readonly IPowerPlatformTestingContext _context;
        private readonly FormControl _control;
        private readonly DateTime? _value;
        private readonly bool _dateOnly;

        public SetDateTimeColumn(IPowerPlatformTestingContext context, FormControl control, DateTime? value, bool dateOnly)
        {
            _context = context;
            _control = control;
            _value = value;
            _dateOnly = dateOnly;
        }

        public async Task<ActionResult> ExecuteAsync(IPage page)
        {
            var container = XPathProvider.GetLocator(page, PowerPlatformConstants.XPaths.EntityFieldContainer, _control.ControlName);
            var dateInput = XPathProvider.GetLocator(container, PowerPlatformConstants.XPaths.EntityColumnDateTimeDateInput, _control.ControlName);
            var timeInput = XPathProvider.GetLocator(container, PowerPlatformConstants.XPaths.EntityColumnDateTimeTimeInput, _control.ControlName);

            if(_value == null)
            {
                await dateInput.ClearAsync();
                return ActionResult.Success();
            }

            var userSettings = await _context.PowerPlatformConnection.Service.GetUserSettingsAsync();

            var dateText = _value.Value.ToString(userSettings.DateFormat);
            await dateInput.FillAsync(dateText);
            

            if(_dateOnly)
                return ActionResult.Success();

            await page.Keyboard.PressAsync("Tab");
            var timeText = _value.Value.ToString(userSettings.TimeFormat);

            await FillAndVerify(page, timeInput, timeText);
            
                return ActionResult.Success();
        }

        private async Task FillAndVerify(IPage page, ILocator timeInput, string timeText, int tries = 10)
        {
            await timeInput.FillAsync(timeText);
            //await page.Keyboard.PressAsync("Tab");

            var currentText = await timeInput.InputValueAsync();
            if(!string.Equals(timeText, currentText))
            {
                await Task.Delay(50);
                if (tries > 0)
                    await FillAndVerify(page, timeInput, timeText, tries - 1);
                else
                    throw new TestExecutionException(PowerPlatformConstants.ErrorCodes.DateTimeFieldNotSet, _control.AttributeName, timeText, currentText);
            }

        }
    }
}
