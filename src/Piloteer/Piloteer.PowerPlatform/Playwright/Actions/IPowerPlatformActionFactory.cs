using Microsoft.Xrm.Sdk;
using Piloteer.PowerPlatform.Models;

namespace Piloteer.PowerPlatform.Playwright.Actions
{
    public interface IPowerPlatformActionFactory
    {
        CheckForDuplicateDetection GetCheckForDuplicateDetection(bool saveIfDuplicate);
        ClickRibbonButton GetClickRibbonButton(string entityName, string buttonName);
        CloseCopilotPane GetCloseCopilotPane();
        CloseFormAssist GetCloseFormAssist();
        ExpandTab GetExpandTab(string tabName);
        FillForm GetFillForm(Entity dataToFill, FormMetadata form);
        GetCompositeControlFields GetGetCompositeControlFields(string compositeAttributeName);
        GetCurrentForm GetGetCurrentForm(bool isQuickCreate);
        GetRecordId GetGetRecordId();
        IsControlLocked GetIsControlLocked(Fields.FormControl control);
        IsControlVisible GetIsControlVisible(Fields.FormControl control);
        OpenRecord GetOpenRecord(OpenFormOptions formOptions);
        SaveForm GetSaveForm(string entityName);
        SetDateTimeColumn GetSetDateTimeColumn(Fields.FormControl control, DateTime? value, bool dateOnly);
        SetLookupColumn GetSetLookupColumn(Fields.FormControl control, EntityReference? value);
        SetOptionSetColumn GetSetOptionSetColumn(Fields.FormControl control, string? label);
        SetStringColumn GetSetStringColumn(Fields.FormControl control, string? value);
    }
}