using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Azure;
using Microsoft.Playwright;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Newtonsoft.Json.Linq;
using Piloteer.Playwright.Actions;
using Piloteer.PowerPlatform.Models;

namespace Piloteer.PowerPlatform.Playwright.Actions
{
    public class FillForm : IPlaywrightAction
    {
        private readonly IPowerPlatformTestingContext _context;
        private readonly IPowerPlatformActionFactory _actionFactory;
        private readonly Entity _dataToFill;
        private readonly FormMetadata _formMetadata;

        public FillForm(IPowerPlatformTestingContext context, IPowerPlatformActionFactory actionFactory, Entity dataToFill, FormMetadata formMetadata)
        {
            _context = context;
            _actionFactory = actionFactory;
            _dataToFill = dataToFill;
            _formMetadata = formMetadata;
        }

        public async Task<ActionResult> ExecuteAsync(IPage page)
        {
            foreach (var attribute in _dataToFill.Attributes)
            {
                var control = _formMetadata.GetControl(attribute.Key);
                if (control == null)
                    throw new TestExecutionException(PowerPlatformConstants.ErrorCodes.ColumnNotOnForm, attribute.Key);

                await _context.ActionProcessor.ExecuteActionAsync(page, _actionFactory.GetExpandTab(control.TabName));

                if (!await _context.ActionProcessor.ExecuteActionAsync(page, _actionFactory.GetIsControlVisible(control)))
                    throw new TestExecutionException(PowerPlatformConstants.ErrorCodes.ColumnNotVisible, attribute.Key);

                if (await _context.ActionProcessor.ExecuteActionAsync(page, _actionFactory.GetIsControlLocked(control)))
                    throw new TestExecutionException(PowerPlatformConstants.ErrorCodes.ColumnLocked, attribute.Key);

                switch (control.Metadata.AttributeType)
                {
                    case AttributeTypeCode.Boolean:
                        await SetBooleanColumn(page, ConvertTo<bool?>(attribute.Value), control); break;
                    case AttributeTypeCode.BigInt:
                    case AttributeTypeCode.Integer:
                        await _context.ActionProcessor.ExecuteActionAsync(page, _actionFactory.GetSetStringColumn(control, ConvertTo<int?>(attribute.Value)?.ToString())); break;
                    case AttributeTypeCode.Customer:
                    case AttributeTypeCode.Lookup:
                    case AttributeTypeCode.Owner:
                        await _context.ActionProcessor.ExecuteActionAsync(page, _actionFactory.GetSetLookupColumn(control, ConvertTo<EntityReference>(attribute.Value))); break;
                    case AttributeTypeCode.DateTime:
                        await SetDateTimeColumn(page, ConvertTo<DateTime?>(attribute.Value), control); break;
                    case AttributeTypeCode.Decimal:
                        await SetDecimalValue(page, ConvertTo<decimal>(attribute.Value), control); break;
                    case AttributeTypeCode.Double:
                        await SetDoubleValue(page, ConvertTo<double?>(attribute.Value), control); break;
                    case AttributeTypeCode.Money:
                        await SetDecimalValue(page, ConvertTo<Money>(attribute.Value)?.Value, control); break;
                    case AttributeTypeCode.Picklist:
                    case AttributeTypeCode.State:
                    case AttributeTypeCode.Status:
                        await SetOptionSetValue(page, ConvertTo<OptionSetValue>(attribute.Value), control); break;
                    case AttributeTypeCode.Memo:
                    case AttributeTypeCode.String:
                        await _context.ActionProcessor.ExecuteActionAsync(page, _actionFactory.GetSetStringColumn(control, (string)attribute.Value)); break;
                    default:
                        throw new NotImplementedException(string.Format("Type {0} not implemented", control.Metadata.AttributeType));
                }
            }

            return ActionResult.Success();


        }

        private async Task SetDateTimeColumn(IPage page, DateTime? value, Fields.FormControl control)
        {
            var metadata = (DateTimeAttributeMetadata)control.Metadata;
           
            DateTime? dateTimeToSet;
            if (value.HasValue && metadata.DateTimeBehavior == DateTimeBehavior.UserLocal)
            {
                var offset = (await _context.PowerPlatformConnection.Service.GetUserSettingsAsync()).TimeZoneInfo.GetUtcOffset(value.Value);
                dateTimeToSet = value.Value.Add(offset);
            }
            else
            {
                dateTimeToSet = value;
            }

            await _context.ActionProcessor.ExecuteActionAsync(page, _actionFactory.GetSetDateTimeColumn(control, dateTimeToSet, metadata.Format == DateTimeFormat.DateOnly));
        }

        private async Task SetBooleanColumn(IPage page, bool? value, Fields.FormControl control)
        {
            var metadata = (BooleanAttributeMetadata)control.Metadata;

            if (value == null)
            {
                await _context.ActionProcessor.ExecuteActionAsync(page, _actionFactory.GetSetOptionSetColumn(control, null));
            }
            else
            {
                var label = value.Value ? metadata.OptionSet.TrueOption.Label : metadata.OptionSet.FalseOption.Label;
                var labelText = label.GetLabelInLanguage(int.Parse(_context.AppSettingsProvider.GetRequiredAppSettingsValue("Formatting:LanguageCode")), control.AttributeName);
                if (string.IsNullOrEmpty(labelText))
                    throw new TestExecutionException(PowerPlatformConstants.ErrorCodes.MissingTranslation, control.AttributeName);

                await _context.ActionProcessor.ExecuteActionAsync(page, _actionFactory.GetSetOptionSetColumn(control, labelText));
            }
        }

        private T? ConvertTo<T>(object? value)
        {
            if (value == null)
                return default;

            return (T)value;
        }

        private async Task SetDecimalValue(IPage page, decimal? value, Fields.FormControl control)
        {
            var settings = await _context.PowerPlatformConnection.Service.GetUserSettingsAsync();
            var stringValue = value?.ToString(settings.NumberFormat);
            await _context.ActionProcessor.ExecuteActionAsync(page, _actionFactory.GetSetStringColumn(control, stringValue));
        }

        private async Task SetDoubleValue(IPage page, double? value, Fields.FormControl control)
        {
            var settings = await _context.PowerPlatformConnection.Service.GetUserSettingsAsync();
            var stringValue = value?.ToString(settings.NumberFormat);
            await _context.ActionProcessor.ExecuteActionAsync(page, _actionFactory.GetSetStringColumn(control, stringValue));
        }

        private async Task SetOptionSetValue(IPage page, OptionSetValue? value, Fields.FormControl control)
        {
            var metadata = (EnumAttributeMetadata)control.Metadata;

            if (value == null)
            {
                await _context.ActionProcessor.ExecuteActionAsync(page, _actionFactory.GetSetOptionSetColumn(control, null));
            }
            else
            {
                var label = metadata.OptionSet.Options.First(o => o.Value == value.Value).Label.GetLabelInLanguage(int.Parse(_context.AppSettingsProvider.GetRequiredAppSettingsValue("Formatting:LanguageCode")), control.AttributeName);
                if (string.IsNullOrEmpty(label))
                    throw new TestExecutionException(PowerPlatformConstants.ErrorCodes.MissingTranslation, control.AttributeName);

                await _context.ActionProcessor.ExecuteActionAsync(page, _actionFactory.GetSetOptionSetColumn(control, label));
            }
        }
    }
}
