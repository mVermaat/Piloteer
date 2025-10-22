using System.Globalization;
using Microsoft.Extensions.Options;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;

namespace Piloteer.PowerPlatform.Metadata
{
    public class EntityParser : IEntityParser
    {
        private readonly string _datetimeFormat;
        private readonly int _languageCode;
        private readonly string _dateonlyFormat;


        public EntityParser(IAppSettingsProvider appSettingsProvider)
        {
            _dateonlyFormat = appSettingsProvider.GetRequiredAppSettingsValue("Formatting:DateFormat");
            _datetimeFormat = $"{_dateonlyFormat} {appSettingsProvider.GetRequiredAppSettingsValue("Formatting:TimeFormat")}";
            _languageCode = int.Parse(appSettingsProvider.GetRequiredAppSettingsValue("Formatting:LanguageCode"));
        }

        public Entity ParseEntity(EntityMetadata metadata, IEnumerable<UnparsedAttribute> attributes,
            TimeZoneInfo timeZone, IAliasedRecordCache recordCache)
        {
            var entity = new Entity(metadata.LogicalName);
            foreach (var attribute in attributes)
            {
                var parsedResult = ParseAttribute(metadata, attribute, timeZone, recordCache);

                entity[parsedResult.AttributeMetadata.LogicalName] = ParseAttributeValue(attribute.Value, parsedResult.AttributeMetadata, timeZone, recordCache);
                if (parsedResult.AttributeMetadata.AttributeType == AttributeTypeCode.Uniqueidentifier && metadata.PrimaryIdAttribute.Equals(parsedResult.AttributeMetadata.LogicalName))
                    entity.Id = (Guid)entity[parsedResult.AttributeMetadata.LogicalName];
            }

            return entity;
        }

        public (AttributeMetadata AttributeMetadata, object? ParsedValue) ParseAttribute(EntityMetadata metadata, UnparsedAttribute attribute,
            TimeZoneInfo timeZone, IAliasedRecordCache recordCache)
        {
            var attributeMetadata = metadata.Attributes.FirstOrDefault(a => a.LogicalName.Equals(attribute.Property, StringComparison.OrdinalIgnoreCase));
            if (attributeMetadata == null)
            {
                var attributesByDisplayName = metadata.Attributes.Where(a => string.Equals(a.DisplayName.GetLabelInLanguage(_languageCode, a.LogicalName), attribute.Property, StringComparison.OrdinalIgnoreCase)).ToArray();

                if (attributesByDisplayName == null || attributesByDisplayName.Length == 0)
                    throw new TestExecutionException(PowerPlatformConstants.ErrorCodes.DisplayNameNotFound, attribute.Property, metadata.LogicalName);
                if (attributesByDisplayName.Length > 1)
                    throw new TestExecutionException(PowerPlatformConstants.ErrorCodes.DisplayNameFoundMultipleTimes, attribute.Property, metadata.LogicalName, string.Join(", ", attributesByDisplayName.Select(a => a.LogicalName)));

                attributeMetadata = attributesByDisplayName[0];
            }

            return (attributeMetadata, ParseAttributeValue(attribute.Value, attributeMetadata, timeZone, recordCache));

        }

        private object? ParseAttributeValue(string value, AttributeMetadata attributeMetadata, TimeZoneInfo timeZone, IAliasedRecordCache recordCache)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            switch (attributeMetadata.AttributeType)
            {
                case AttributeTypeCode.Boolean:
                    return ParseBoolean(attributeMetadata, value);

                case AttributeTypeCode.Double: return ParseDouble(value);
                case AttributeTypeCode.Decimal: return ParseDecimal(value);
                case AttributeTypeCode.Integer: return ParseInteger(value);
                case AttributeTypeCode.Money: return new Money(ParseDecimal(value));

                case AttributeTypeCode.Uniqueidentifier: return ParseGuid(value);

                case AttributeTypeCode.EntityName:
                case AttributeTypeCode.Memo:
                case AttributeTypeCode.String: return value;

                case AttributeTypeCode.DateTime:
                    return ParseDateTime((DateTimeAttributeMetadata)attributeMetadata, value, timeZone);

                case AttributeTypeCode.Picklist:
                case AttributeTypeCode.State:
                case AttributeTypeCode.Status:
                    return ParseOptionSet(attributeMetadata, value);

                case AttributeTypeCode.Customer:
                case AttributeTypeCode.Lookup:
                case AttributeTypeCode.Owner:
                    return ParseLookup(recordCache, value);

                default: throw new NotImplementedException(string.Format("Type {0} not implemented", attributeMetadata.AttributeType));
            }
        }


        /// <summary>
        /// Gets the TwoOptionValue. Can either be true/false or the label of the option set value.
        /// </summary>
        /// <param name="metadata">Boolean metadata</param>
        /// <param name="value">String representation of the boolean</param>
        /// <returns></returns>
        private bool ParseBoolean(AttributeMetadata metadata, string value)
        {
            if (bool.TryParse(value, out bool boolValue))
                return boolValue;

            var optionMd = (BooleanAttributeMetadata)metadata;
            var trueLabel = optionMd.OptionSet.TrueOption.Label.GetLabelInLanguage(_languageCode, $"{optionMd.LogicalName} - Option true");
            var falseLabel = optionMd.OptionSet.FalseOption.Label.GetLabelInLanguage(_languageCode, $"{optionMd.LogicalName} - Option false");

            if (string.Equals(value, trueLabel, StringComparison.InvariantCultureIgnoreCase))
                return true;
            else if (string.Equals(value, falseLabel, StringComparison.InvariantCultureIgnoreCase))
                return false;
            else
                throw new TestExecutionException(PowerPlatformConstants.ErrorCodes.OptionSetValueUnknown, metadata.LogicalName, value, $"{trueLabel}, {falseLabel}");
        }

        private double ParseDouble(string value)
        {
            if (!double.TryParse(value, NumberFormatInfo.InvariantInfo, out double doubleValue))
                throw new TestExecutionException(PowerPlatformConstants.ErrorCodes.InvalidDecimalNumber, value);
            return doubleValue;
        }

        private decimal ParseDecimal(string value)
        {
            if (!decimal.TryParse(value, NumberFormatInfo.InvariantInfo, out decimal decimalValue))
                throw new TestExecutionException(PowerPlatformConstants.ErrorCodes.InvalidDecimalNumber, value);
            return decimalValue;
        }

        private Guid ParseGuid(string value)
        {
            if (!Guid.TryParse(value, NumberFormatInfo.InvariantInfo, out Guid guidValue))
                throw new TestExecutionException(PowerPlatformConstants.ErrorCodes.InvalidGuid, value);
            return guidValue;
        }

        private int ParseInteger(string value)
        {
            if (!int.TryParse(value, NumberFormatInfo.InvariantInfo, out int integerValue))
                throw new TestExecutionException(PowerPlatformConstants.ErrorCodes.InvalidWholeNumber, value);
            return integerValue;
        }

        private EntityReference ParseLookup(IAliasedRecordCache recordCache, string value)
        {
            return recordCache.GetRequired(value);
        }

        private OptionSetValue ParseOptionSet(AttributeMetadata attributeMetadata, string value)
        {
            if (int.TryParse(value, NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out int intValue))
            {
                if(!((EnumAttributeMetadata)attributeMetadata).OptionSet.Options.Any(os => os.Value == intValue))
                    throw new TestExecutionException(PowerPlatformConstants.ErrorCodes.OptionSetValueUnknown, value, attributeMetadata.LogicalName, string.Join(",", ((EnumAttributeMetadata)attributeMetadata).OptionSet.Options.Select(os => os.Label.GetLabelInLanguage(_languageCode, $"{attributeMetadata.LogicalName} - Option {os.Value}"))));

                return new OptionSetValue(intValue);
            }

            var option = ((EnumAttributeMetadata)attributeMetadata).OptionSet.Options.FirstOrDefault(os => string.Equals(os.Label.GetLabelInLanguage(_languageCode, $"{attributeMetadata.LogicalName} - Option {os.Value}"), value)); ;

            if (option == null || option.Value == null)
                throw new TestExecutionException(PowerPlatformConstants.ErrorCodes.OptionSetValueUnknown, value, attributeMetadata.LogicalName, string.Join(",", ((EnumAttributeMetadata)attributeMetadata).OptionSet.Options.Select(os => os.Label.GetLabelInLanguage(_languageCode, $"{attributeMetadata.LogicalName} - Option {os.Value}"))));

            return new OptionSetValue(option.Value.Value);
        }

        private DateTime ParseDateTime(DateTimeAttributeMetadata metadata, string value, TimeZoneInfo timeZone)
        {
            var format = metadata.Format == DateTimeFormat.DateOnly ?
                _dateonlyFormat : _datetimeFormat;

            if(!DateTime.TryParseExact(value, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDateTime))
                throw new TestExecutionException(PowerPlatformConstants.ErrorCodes.InvalidDateTime, value, format);

            if (metadata.DateTimeBehavior == DateTimeBehavior.UserLocal)
                return TimeZoneInfo.ConvertTimeToUtc(parsedDateTime, timeZone);
            
            return parsedDateTime;
        }
    }
}
