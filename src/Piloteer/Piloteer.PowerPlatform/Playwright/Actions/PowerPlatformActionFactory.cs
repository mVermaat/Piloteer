using Microsoft.Xrm.Sdk;
using Piloteer.PowerPlatform.Models;

namespace Piloteer.PowerPlatform.Playwright.Actions
{
    public class PowerPlatformActionFactory : IPowerPlatformActionFactory
    {
        private readonly IPowerPlatformTestingContext _context;

        public PowerPlatformActionFactory(IPowerPlatformTestingContext context)
        {
            _context = context;
        }

        public virtual CheckForDuplicateDetection GetCheckForDuplicateDetection(bool saveIfDuplicate) => new(_context, saveIfDuplicate);
        public virtual ClickRibbonButton GetClickRibbonButton(string entityName, string buttonName) => new(entityName, buttonName);
        public virtual CloseCopilotPane GetCloseCopilotPane() => new(_context);
        public virtual CloseFormAssist GetCloseFormAssist() => new(_context);
        public virtual ExpandTab GetExpandTab(string tabName) => new(tabName);
        public virtual FillForm GetFillForm(Entity dataToFill, FormMetadata form) => new(_context, this, dataToFill, form);
        public virtual GetCompositeControlFields GetGetCompositeControlFields(string compositeAttributeName) => new(compositeAttributeName);
        public virtual GetCurrentForm GetGetCurrentForm(bool isQuickCreate) => new(_context, isQuickCreate);
        public virtual GetRecordId GetGetRecordId() => new(_context);
        public virtual IsControlLocked GetIsControlLocked(Fields.FormControl control) => new(control);
        public virtual IsControlVisible GetIsControlVisible(Fields.FormControl control) => new(control);
        public virtual OpenRecord GetOpenRecord(OpenFormOptions formOptions) => new(_context, this, formOptions);
        public virtual SaveForm GetSaveForm(string entityName) => new(_context, this, entityName);
        public virtual SetDateTimeColumn GetSetDateTimeColumn(Fields.FormControl control, DateTime? value, bool dateOnly) => new(_context, control, value, dateOnly);
        public virtual SetLookupColumn GetSetLookupColumn(Fields.FormControl control, EntityReference? value) => new(control, value);
        public virtual SetOptionSetColumn GetSetOptionSetColumn(Fields.FormControl control, string? label) => new(control, label);
        public virtual SetStringColumn GetSetStringColumn(Fields.FormControl control, string? value) => new(control, value);

    }
}
