using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Playwright;
using Piloteer.Playwright;
using Piloteer.Playwright.Actions;
using Piloteer.PowerPlatform.Metadata;
using Piloteer.PowerPlatform.Models;
using Reqnroll;

namespace Piloteer.PowerPlatform.Playwright.Actions
{
    public class OpenRecord : IPlaywrightAction<FormMetadata>
    {
        private readonly IPowerPlatformTestingContext _context;
        private readonly IPowerPlatformActionFactory _actionFactory;
        private readonly OpenFormOptions _formOptions;

        public OpenRecord(IPowerPlatformTestingContext context, IPowerPlatformActionFactory actionFactory, OpenFormOptions formOptions)
        {
            _context = context;
            _actionFactory = actionFactory;
            _formOptions = formOptions;
        }

       
        public async Task<ActionResult<FormMetadata>> ExecuteAsync(IPage page)
        {
            _context.Logger.WriteLine($"Opening record {_formOptions.EntityName} with ID {_formOptions.EntityId}");

            if (_context.CurrentModelAppId == null)
                throw new TestExecutionException(PowerPlatformConstants.ErrorCodes.ModelAppNotSet);

            await page.GotoAsync(_formOptions.GetUrl(_context.CurrentModelAppId));

            await XPathProvider.GetLocator(page, PowerPlatformConstants.XPaths.EntityFormRoot).WaitForAsync(new LocatorWaitForOptions() { State = WaitForSelectorState.Visible, Timeout = 30000 });
            var form = await _context.ActionProcessor.ExecuteActionAsync(page, _actionFactory.GetGetCurrentForm(false));

            await _context.ActionProcessor.ExecuteActionAsync(page, _actionFactory.GetCloseFormAssist());
            await _context.ActionProcessor.ExecuteActionAsync(page, _actionFactory.GetCloseCopilotPane());

            return ActionResult<FormMetadata>.Success(await _context.MetadataCache.GetFormMetadataAsync(_context, _actionFactory, form));
        }
    }
}
