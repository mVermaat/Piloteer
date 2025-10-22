using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;

namespace Piloteer.PowerPlatform.UnitTests.Metadata
{
    internal static class MetadataHelper
    {
        public static EntityMetadata CreateEntityMetadata(string entityName, params AttributeMetadata[] attributes)
        {
            var md = new EntityMetadata
            {
                LogicalName = entityName,

            };

            var primaryKeyAttribute = new UniqueIdentifierAttributeMetadata(entityName + "id")
            {
                LogicalName = entityName + "id"
            };

            SetSealedPropertyValue(md, "Attributes", attributes.Concat([primaryKeyAttribute]).ToArray());
            SetSealedPropertyValue(md, "PrimaryIdAttribute", primaryKeyAttribute.LogicalName);

            return md;
        }

        public static BooleanAttributeMetadata CreateBooleanAttributeMetadata(string logicalName, string trueOption = "Yes", string falseOption = "No")
            => new BooleanAttributeMetadata(logicalName)
            {
                LogicalName = logicalName,
                OptionSet = new BooleanOptionSetMetadata
                {
                    TrueOption = new OptionMetadata(CreateLabel(trueOption), 1),
                    FalseOption = new OptionMetadata(CreateLabel(falseOption), 0)
                }
            };

        public static DecimalAttributeMetadata CreateDecimalAttributeMetadata(string logicalName)
            => new DecimalAttributeMetadata(logicalName)
            {
                LogicalName = logicalName,
            };

        public static DoubleAttributeMetadata CreateDoubleAttributeMetadata(string logicalName)
            => new DoubleAttributeMetadata(logicalName)
            {
                LogicalName = logicalName,
            };

        public static DateTimeAttributeMetadata CreateDateTimeAttributeMetadata(string logicalName, DateTimeFormat format, DateTimeBehavior behavior)
                => new DateTimeAttributeMetadata(format, logicalName)
                {
                    LogicalName = logicalName,
                    DateTimeBehavior = behavior,
                };

        public static IntegerAttributeMetadata CreateIntegerAttributeMetadata(string logicalName)
            => new IntegerAttributeMetadata(logicalName)
            {
                LogicalName = logicalName,
            };

        public static LookupAttributeMetadata CreateLookupAttributeMetadata(string logicalName)
            => new LookupAttributeMetadata(LookupFormat.None)
            {
                LogicalName = logicalName,
            };


        public static MoneyAttributeMetadata CreateMoneyAttributeMetadata(string logicalName)
            => new MoneyAttributeMetadata(logicalName)
            {
                LogicalName = logicalName,
            };

        public static PicklistAttributeMetadata CreatePicklistAttributeMetadata(string logicalName, params (int Value, string Label)[] options)
        {
            var md = new PicklistAttributeMetadata(logicalName)
            {
                LogicalName = logicalName,
                OptionSet = new OptionSetMetadata()
            };

            foreach (var option in options)
            {
                md.OptionSet.Options.Add(new OptionMetadata(CreateLabel(option.Label), option.Value));
            }

            return md;
        }


        public static UniqueIdentifierAttributeMetadata CreateUniqueIdentifierAttributeMetadata(string logicalName)
            => new UniqueIdentifierAttributeMetadata(logicalName)
            {
                LogicalName = logicalName,
            };

        private static Label CreateLabel(string value)
        {
            var label = new LocalizedLabel(value, 1033);
            return new Label(label, new LocalizedLabel[] { label });
        }

        private static void SetSealedPropertyValue(EntityMetadata entityMetadata, string propertyName, object value)
        {
            var prop = entityMetadata.GetType().GetProperty(propertyName);
            if (prop == null)
                throw new ArgumentException($"Property {propertyName} doesn't exist");

            prop.SetValue(entityMetadata, value, null);
        }
    }
}
