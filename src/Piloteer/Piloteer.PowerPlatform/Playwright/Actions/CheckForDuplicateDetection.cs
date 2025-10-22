using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Playwright;
using Piloteer.Playwright;
using Piloteer.Playwright.Actions;
using Piloteer.PowerPlatform.Models;

namespace Piloteer.PowerPlatform.Playwright.Actions
{
    public class CheckForDuplicateDetection : IPlaywrightAction<DuplicateDetectionResult>
    {
        private readonly IPowerPlatformTestingContext _context;
        private readonly bool _saveIfDuplicate;

        public CheckForDuplicateDetection(IPowerPlatformTestingContext context, bool saveIfDuplicate)
        {
            _context = context;
            _saveIfDuplicate = saveIfDuplicate;
        }

        public async Task<ActionResult<DuplicateDetectionResult>> ExecuteAsync(IPage page)
        {
            var duplicateDetectionWindow = XPathProvider.GetLocator(page, PowerPlatformConstants.XPaths.DuplicateDetectionWindow);
            if (await duplicateDetectionWindow.IsVisibleAsync())
            {
                if (_saveIfDuplicate)
                {
                    await AcceptDuplicateDetection(page, duplicateDetectionWindow);
                    _context.Logger.WriteLine("Accepted duplicate");
                    return ActionResult<DuplicateDetectionResult>.Success(DuplicateDetectionResult.DuplicateDetectionAccepted);
                }
                else
                {
                    _context.Logger.WriteLine("Duplicate detection rejected");
                    return ActionResult<DuplicateDetectionResult>.Success(DuplicateDetectionResult.DuplicateDetectionRejected);
                }
            }
            else
            {
                _context.Logger.WriteLine("No duplicate detection");
                return ActionResult<DuplicateDetectionResult>.Success(DuplicateDetectionResult.NoDuplicateDetection);
            }
        }

        private async Task AcceptDuplicateDetection(IPage page, ILocator duplicateDetectionWindow)
        {
            _context.Logger.WriteLine("Accepting duplicate");
            //Select the first record in the grid
            await XPathProvider.GetLocator(duplicateDetectionWindow, PowerPlatformConstants.XPaths.DuplicateDetectionGridRows).First.ClickAsync();

            //Click Ignore and Save
            await XPathProvider.GetLocator(duplicateDetectionWindow, PowerPlatformConstants.XPaths.DuplicateDetectionSaveButton).ClickAsync();

            await duplicateDetectionWindow.WaitForAsync(new LocatorWaitForOptions() { State = WaitForSelectorState.Hidden });
        }
    }
}
