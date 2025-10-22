using Microsoft.Playwright;
using Piloteer.Playwright;
using Piloteer.Playwright.Actions;
using Piloteer.PowerPlatform.Models;

namespace Piloteer.PowerPlatform.Playwright.Actions
{
    public class SaveForm : IPlaywrightAction
    {
        private readonly IPowerPlatformTestingContext _context;
        private readonly IPowerPlatformActionFactory _actionFactory;
        private readonly string _entityName;

        public SaveForm(IPowerPlatformTestingContext context, IPowerPlatformActionFactory actionFactory, string entityName)
        {
            _context = context;
            _actionFactory = actionFactory;
            _entityName = entityName;
        }

        public async Task<ActionResult> ExecuteAsync(IPage page)
        {
            await _context.ActionProcessor.ExecuteActionAsync(page, _actionFactory.GetClickRibbonButton(_entityName, PowerPlatformConstants.UITexts.ButtonTextSave));

            var saveStatusLocator = XPathProvider.GetLocator(page, PowerPlatformConstants.XPaths.EntityFormSaveStatus);
            var timeout = DateTime.Now.AddSeconds(20);
            bool saveCompleted = false;
            while (!saveCompleted && DateTime.Now < timeout)
            {
                await Task.Delay(200);
                var status = await GetSaveStatus(saveStatusLocator);

                if (status == SaveStatus.Saved)
                    saveCompleted = true;
                else if (status == SaveStatus.Unsaved)
                {
                    var duplicateResult = await _context.ActionProcessor.ExecuteActionAsync(page, _actionFactory.GetCheckForDuplicateDetection(true));
                    if (duplicateResult == DuplicateDetectionResult.DuplicateDetectionRejected)
                        return ActionResult.Fail(false, PowerPlatformConstants.ErrorCodes.DuplicateDetectionRejected);

                    var saveError = await GetErrorDialogMessage(page);
                    if (!string.IsNullOrEmpty(saveError))
                        return ActionResult.Fail(false, PowerPlatformConstants.ErrorCodes.FormSaveFailed, saveError);
                }

            }

            if (saveCompleted)
                return ActionResult.Success();
            else
                return ActionResult.Fail(false, PowerPlatformConstants.ErrorCodes.SaveTimeoutReached);
        }

        private async Task<string?> GetErrorDialogMessage(IPage page)
        {
            var errorDialog = XPathProvider.GetLocator(page, PowerPlatformConstants.XPaths.ErrorDialogRoot);
            if (!await errorDialog.IsVisibleAsync())
                return null;

            var errorMessageLocator = XPathProvider.GetLocator(errorDialog, PowerPlatformConstants.XPaths.ErrorDialogText);
            return await errorMessageLocator.InnerTextAsync();
        }

        private async Task<SaveStatus?> GetSaveStatus(ILocator saveStatus)
        {
            var saveStatusText = await saveStatus.InnerTextAsync();

            if (string.IsNullOrEmpty(saveStatusText))
                return SaveStatus.Unknown;
            else if (saveStatusText.Equals($"- {PowerPlatformConstants.UITexts.SaveStatusUnsaved}", StringComparison.OrdinalIgnoreCase))
                return SaveStatus.Unsaved;
            else if (saveStatusText.Equals($"- {PowerPlatformConstants.UITexts.SaveStatusSaving}", StringComparison.OrdinalIgnoreCase))
                return SaveStatus.Saving;
            else if (saveStatusText.Equals($"- {PowerPlatformConstants.UITexts.SaveStatusSaved}", StringComparison.OrdinalIgnoreCase))
                return SaveStatus.Saved;
            else
                return SaveStatus.Unknown;
        }
    }
}
