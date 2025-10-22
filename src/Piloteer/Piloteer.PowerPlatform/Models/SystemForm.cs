using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using System.Xml.Serialization;
using Piloteer.PowerPlatform.Connectivity;

namespace Piloteer.PowerPlatform.Models
{
    public class SystemForm
    {
        private static class Fields
        {
            public const string FormXml = "formxml";
            public const string Name = "name";
            public const string ObjectTypeCode = "objecttypecode";
            public const string Type = "type";
        }

        public const string EntityLogicalName = "systemform";

        private readonly Entity _record;
        private readonly Lazy<FormXmlDefinition> _parsedFormXml;

        public FormXmlDefinition FormXml => _parsedFormXml.Value;
        public Guid Id => _record.Id;
        public string Name => _record.GetAttributeValue<string>(Fields.Name);
        public string EntityName => _record.GetAttributeValue<string>(Fields.ObjectTypeCode);
        public SystemFormType Type => (SystemFormType)_record.GetAttributeValue<OptionSetValue>(Fields.Type).Value;

        public SystemForm(Entity record)
        {
            _record = record;
            _parsedFormXml = new Lazy<FormXmlDefinition>(ParseFormXml);
        }

        private FormXmlDefinition ParseFormXml()
        {
            var serializer = new XmlSerializer(typeof(FormXmlDefinition));
            using (var reader = new StringReader(_record.GetAttributeValue<string>(Fields.FormXml)))
            {
                var definition = serializer.Deserialize(reader) as FormXmlDefinition;
                if (definition == null)
                    throw new TestExecutionException(PowerPlatformConstants.ErrorCodes.FormParsingFailed);
                return definition;
            }
        }

        public static async Task<SystemForm?> GetSystemFormAsync(IDataverseService service, string name, string objectTypeCode)
        {
            var result = (await service.RetrieveMultipleAsync(new QueryExpression(EntityLogicalName)
            {
                ColumnSet = new ColumnSet(Fields.FormXml, Fields.Name, Fields.ObjectTypeCode),
                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(Fields.Name, ConditionOperator.Equal, name),
                        new ConditionExpression(Fields.ObjectTypeCode, ConditionOperator.Equal, objectTypeCode)
                    }
                },
                TopCount = 1
            })).Entities.FirstOrDefault();

            return result != null ? new SystemForm(result) : null;
        }

        public static async Task<SystemForm> GetById(IDataverseService service, Guid formId)
        {
            return new SystemForm(await service.RetrieveAsync(EntityLogicalName, formId,
                new ColumnSet(Fields.FormXml, Fields.Name, Fields.ObjectTypeCode, Fields.Type)));
        }
    }
}
