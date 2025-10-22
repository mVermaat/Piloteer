using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.Xrm.Sdk.Metadata;
using Piloteer.Playwright.Actions;
using Piloteer.PowerPlatform;
using Piloteer.PowerPlatform.Connectivity;
using Piloteer.PowerPlatform.Models;
using Piloteer.PowerPlatform.Playwright.Actions;

namespace Piloteer.PowerPlatform.Playwright.Fields
{
    internal static class FormXmlParser
    {
        private static readonly Dictionary<string, HashSet<string>> _compositeFields;

        static FormXmlParser()
        {
            _compositeFields = [];

            AddCompositeField("account", "address1_composite");
            AddCompositeField("account", "address2_composite");
            AddCompositeField("account", "address3_composite");

            AddCompositeField("contact", "address1_composite");
            AddCompositeField("contact", "address2_composite");
            AddCompositeField("contact", "address3_composite");
            AddCompositeField("contact", "fullname");

            AddCompositeField("lead", "address1_composite");
            AddCompositeField("lead", "address2_composite");
            AddCompositeField("lead", "address3_composite");
            AddCompositeField("lead", "fullname");
        }

        private static void AddCompositeField(string entityName, string compositeFieldName)
        {
            if (!_compositeFields.TryGetValue(entityName, out var fieldList))
            {
                fieldList = new HashSet<string>();
                _compositeFields.Add(entityName, fieldList);
            }
            fieldList.Add(compositeFieldName);
        }

        public static async Task<Dictionary<string, List<FormControl>>> ParseForm(IDataverseService service, IAppSettingsProvider appSettingsProvider,
            IPage page, IPowerPlatformActionFactory actionFactory, IActionProcessor actionProcessor,
            SystemForm form, EntityMetadata metadata)
        {
            var definition = form.FormXml;
            var metadataDic = metadata.Attributes.ToDictionary(a => a.LogicalName);
            var formControls = new Dictionary<string, List<FormControl>>();
            var userSettings = await service.GetUserSettingsAsync();
            var inputLanguageCode = int.Parse(appSettingsProvider.GetRequiredAppSettingsValue("Formatting:LanguageCode"));

            foreach (var tab in definition.Tabs)
            {
                var tabName = tab.Name ?? tab.Id;
                if(string.IsNullOrEmpty(tabName))
                    throw new TestExecutionException(PowerPlatformConstants.ErrorCodes.NoTabName, metadata.LogicalName);

                var tabLabel = tab.Labels.GetLabelInLanguage(userSettings.UILanguageId, inputLanguageCode);
                foreach (var column in tab.Columns)
                {
                    foreach (var section in column.Sections)
                    {
                        var sectionName = section.Name;
                        var sectionLabel = section.Labels.GetLabelInLanguage(userSettings.UILanguageId, inputLanguageCode);
                        foreach (var row in section.Rows)
                        {
                            await ProcessFormRow(page, actionFactory, actionProcessor, formControls, metadataDic, tabName, tabLabel, sectionName, sectionLabel, row);
                        }
                    }
                }
            }

            return formControls;

        }

        private static async Task ProcessFormRow(IPage page, IPowerPlatformActionFactory actionFactory, IActionProcessor actionProcessor, Dictionary<string, List<FormControl>> formControls, Dictionary<string,AttributeMetadata> metadata, string tabName,
            string? tabLabel, string? sectionName, string? sectionLabel, FormRow row)
        {
            if (row.Cells == null)
                return;

            foreach (var cell in row.Cells)
            {
                // mapcontrol, subgrids and alike will be skipped
                // empty column will be a cell without control
                if (cell == null || cell.IsSpacer || string.IsNullOrEmpty(cell.Control?.AttributeName) || string.IsNullOrEmpty(cell.Control?.ControlName))
                    continue;

                var attributeName = cell.Control.AttributeName;
                var attributeMetadata = metadata[cell.Control.AttributeName];

                if (_compositeFields.TryGetValue(attributeMetadata.EntityLogicalName, out var compositeFields) && compositeFields.Contains(attributeName))
                {
                    await AddCompositeControlSet(page, actionFactory, actionProcessor, compositeFields, formControls, tabName, cell, attributeName, metadata);
                }
                else
                {
                    AddRegularControl(formControls, tabName, cell, attributeName, attributeMetadata);
                }
            }
        }

        private static async Task AddCompositeControlSet(IPage page, IPowerPlatformActionFactory actionFactory, IActionProcessor actionProcessor, HashSet<string> compositeFields, 
            Dictionary<string, List<FormControl>> formControls, string tabName, FormCell cell, string attributeName, Dictionary<string, AttributeMetadata> metadata)
        {
            var fields = await actionProcessor.ExecuteActionAsync(page, actionFactory.GetGetCompositeControlFields(attributeName));

            foreach (var field in fields)
            {
                if (!formControls.TryGetValue(field, out var controls))
                {
                    controls = new List<FormControl>();
                    formControls[field] = controls;
                }


                var control = new FormControl(metadata[field], $"{cell.Control?.ControlName}_compositionLinkControl_{field}", field, tabName);
                controls.Add(control);
            }
        }

        private static void AddRegularControl(Dictionary<string, List<FormControl>> formControls, string tabName, FormCell cell, string attributeName, AttributeMetadata attributeMetadata)
        {
            if (!formControls.TryGetValue(attributeName, out var controls))
            {
                controls = new List<FormControl>();
                formControls[attributeName] = controls;
            }

            if (string.IsNullOrEmpty(tabName))
                throw new Exception("is this not null?");

            var controlName = cell.Control?.ControlName;
            if(string.IsNullOrEmpty(controlName))
                throw new TestExecutionException(PowerPlatformConstants.ErrorCodes.NoControlName, attributeName, tabName);

            controls.Add(new FormControl(attributeMetadata, controlName, attributeName, tabName));
        }
    }
}
