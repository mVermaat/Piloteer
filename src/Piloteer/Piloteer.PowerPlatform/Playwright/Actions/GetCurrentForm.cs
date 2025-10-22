using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Playwright;
using Microsoft.Xrm.Sdk.PluginTelemetry;
using Piloteer.Playwright;
using Piloteer.Playwright.Actions;
using Piloteer.PowerPlatform.Models;
using Reqnroll;

namespace Piloteer.PowerPlatform.Playwright.Actions
{
    public class GetCurrentForm : IPlaywrightAction<SystemForm>
    {
        private readonly IPowerPlatformTestingContext _context;
        private readonly bool _isQuickCreate;

        public GetCurrentForm(IPowerPlatformTestingContext context, bool isQuickCreate)
        {
            _context = context;
            _isQuickCreate = isQuickCreate;
        }

        public async Task<ActionResult<SystemForm>> ExecuteAsync(IPage page)
        {
            Guid formId = Guid.Empty;

            if (_isQuickCreate)
            {
                var dialogRoot = XPathProvider.GetLocator(page, PowerPlatformConstants.XPaths.EntityQuickCreateDialogRoot);
                //await dialogRoot.WaitForAsync(new LocatorWaitForOptions() { State = WaitForSelectorState.Attached, Timeout = 5000 });
                _context.Logger.WriteLine("Quick create form available");

                var formIdText = await dialogRoot.GetAttributeAsync("data-preview-id", new LocatorGetAttributeOptions() { Timeout = 5000 });
                if (!string.IsNullOrEmpty(formIdText))
                    formId = Guid.Parse(formIdText);
                else
                    return ActionResult<SystemForm>.Fail(true, PowerPlatformConstants.ErrorCodes.FormIdNotFound);
            }
            else
            {
                var formIdElement = XPathProvider.GetLocator(page, PowerPlatformConstants.XPaths.EntityFormId);
                _context.Logger.WriteLine("Form available");
                var route = await formIdElement.GetAttributeAsync("route", new LocatorGetAttributeOptions() { Timeout = 25000 });
                _context.Logger.WriteLine($"Determining form: {route}");
                if (string.IsNullOrEmpty(route) || !route.Contains('/'))
                    return ActionResult<SystemForm>.Fail(true, PowerPlatformConstants.ErrorCodes.FormIdNotFound);
                formId = Guid.Parse(route.AsSpan(route.LastIndexOf('/') + 1));
            }
            return ActionResult<SystemForm>.Success(await SystemForm.GetById(_context.PowerPlatformConnection.Service, formId));
        }
    }  
}
