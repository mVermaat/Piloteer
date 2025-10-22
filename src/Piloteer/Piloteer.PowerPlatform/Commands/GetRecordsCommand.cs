using Piloteer.Commands;
using Piloteer.PowerPlatform.Connectivity;
using Piloteer.PowerPlatform.Metadata;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;

namespace Piloteer.PowerPlatform.Commands
{
    public class GetRecordsCommand : ApiOnlyCommandFunc<DataCollection<Entity>>
    {
        private readonly IPowerPlatformTestingContext _context;
        private readonly string _entityLogicalName;
        private readonly IEnumerable<UnparsedAttribute> _criteria;

        public GetRecordsCommand(IPowerPlatformTestingContext commandContext,
            string entityLogicalName,
            IEnumerable<UnparsedAttribute> criteria)
        {
            _context = commandContext;
            _entityLogicalName = entityLogicalName;
            _criteria = criteria;
        }

        protected override async Task<DataCollection<Entity>> ExecuteApiAsync()
        {
            var metadata = await _context.MetadataCache.GetEntityMetadataAsync(_context.PowerPlatformConnection, _entityLogicalName);

            var query = new QueryExpression(_entityLogicalName);
            foreach (var attribute in _criteria)
            {
                var parsedAttribute = _context.EntityParser.ParseAttribute(metadata, attribute, (await _context.PowerPlatformConnection.Service.GetUserSettingsAsync()).TimeZoneInfo, _context.AliasedRecordCache);

                if (parsedAttribute.ParsedValue != null)
                {
                    query.Criteria.AddCondition(parsedAttribute.AttributeMetadata.LogicalName, ConditionOperator.Equal, ToPrimitive(parsedAttribute.AttributeMetadata, parsedAttribute.ParsedValue));
                }
                else
                {
                    query.Criteria.AddCondition(parsedAttribute.AttributeMetadata.LogicalName, ConditionOperator.Null);
                }
            }

            var result = await _context.PowerPlatformConnection.Service.RetrieveMultipleAsync(query);
            return result.Entities;
        }

        private object ToPrimitive(AttributeMetadata metadata, object value)
        {
            if (value is EntityReference entityReference)
                return entityReference.Id;
            else if (value is OptionSetValue optionSetValue)
                return optionSetValue.Value;
            else if (value is Money money)
                return money.Value;

            return value;


        }
    }
}
